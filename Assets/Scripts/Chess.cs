using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// It contains the main variables and functions that allow the correct functioning of the chess game.
/// </summary>
public static class Chess
{
    #region Delegates

    /// <summary>
    /// Delegate who is in charge of controlling the colours of the game squares.
    /// </summary>
    /// <param name="piecePosition">The position of the selected piece.</param>
    /// <param name="greenPositions">The position where the selected part can legally move.</param>
    public delegate void NewColourDelegate(Vector2 piecePosition, List<Vector2> greenPositions);

    /// <summary>
    /// Update the color of the squares in the game.
    /// </summary>
    public static event NewColourDelegate UpdateColour;

    /// <summary>
    /// Delegate whose function is to control the use of the game squares.
    /// </summary>
    public delegate void BoardsDelegate();

    /// <summary>
    /// Returns the squares to their initial colour.
    /// </summary>
    public static event BoardsDelegate OriginalColour;

    /// <summary>
    /// Allows selection of squares (disables lock).
    /// </summary>
    public static event BoardsDelegate EnableSelection;

    /// <summary>
    /// Prohibits the selection of squares (activates the lock).
    /// </summary>
    public static event BoardsDelegate DisableSelection;

    #endregion

    #region Players and game mode

    /// <summary>
    /// The color of the player who is playing. Lets you select the pieces on your turn. "All" would be for local multiplayer.
    /// </summary>
    public static Enums.Colours PlayerColour { get; set; } = Enums.Colours.All;


    /// <summary>
    /// Indicates if a game is being played against the AI.
    /// </summary>
    static bool singlePlay;

    /// <summary>
    /// The colour of the player whose turn it is to play.
    /// </summary>
    static Enums.Colours playerInTurn;

    /// <summary>
    /// List of all the white pieces on the board.
    /// </summary>
    public static List<GameObject> PiecesWhite { get; set; } = new List<GameObject>();

    /// <summary>
    /// List of all the black pieces on the board.
    /// </summary>
    public static List<GameObject> PiecesBlack { get; set; } = new List<GameObject>();

    /// <summary>
    /// The king of the white player.
    /// </summary>
    static GameObject whiteKing = null;

    /// <summary>
    /// The king of the black player.
    /// </summary>
    static GameObject blackKing = null;

    /// <summary>
    /// Activate the selected color and start a game against the AI.
    /// </summary>
    /// <param name="data">The loaded game, if it exists. Null to start the game from the beginning.</param>
    public static void SelectColor(Enums.Colours colour, SaveData data)
    {
        // We clean the scene for safety.

        CleanScene();

        // We define the selected color.

        PlayerColour = colour;

        // We indicate that the game will be against the AI.

        singlePlay = true;

        // We call the starting method of the game according to whether or not there is a loaded game.

        if (data == null)
        {
            StartNewGame();
        }

        else
        {
            singlePlay = true;

            StartLoadedGame(data);
        }
    }

    /// <summary>
    /// Start a game from the beginning.
    /// </summary>
    public static void StartNewGame()
    {
        // We place all the pieces on the board in their initial position.

        InitialSpawn();

        // We remove the history of saved positions from a possible previous game.

        savedPositions.Clear();
        savedPositions.Add(new PositionRecord(PiecesWhite, PiecesBlack));

        // We define the main variables of the game to their default value.

        stalemate = false;
        movements = 0;
        playerInTurn = Enums.Colours.White;
        WhiteKingInCheck = false;
        BlackKingInCheck = false;
        IsPlaying = true;

        // We activate the message on the interface to indicate that it is White's turn (they play first).
        // In addition, we activate the save button in the pause menu.

        Interface.interfaceClass.SetWaitingMessage(Enums.Colours.White);
        Interface.interfaceClass.EnableOnlineSave();

        // If the AI is active and he plays White, he starts moving.

        if (singlePlay && PlayerColour == Enums.Colours.Black)
        {
            Interface.interfaceClass.EnableButtonPause(false);
            TimeEvents.timeEvents.StartWaitForAI();
        }

        // We deactivate the blocking of the squares in order to move the pieces.

        EnableSelection();
    }

    /// <summary>
    /// Starts a game from an loaded file.
    /// </summary>
    /// <param name="data">The loaded file.</param>
    public static void StartLoadedGame(SaveData data)
    {
        // We start by converting all the data within the SaveData class so that the variables of this class can be initialized.

        playerInTurn = data.playerInTurn;
        enPassantPawnPosition = new Vector2(data.enPassantDoublePositionX, data.enPassantDoublePositionY);
        EnPassantPosition = new Vector2(data.enPassantPositionX, data.enPassantPositionY);
        EnPassantActive = enPassantPawnPosition != Vector2.zero;
        movements = data.movements;

        savedPositions.Clear();

        // To interpret the variables of the saved positions, we take into account the number of saved positions.
        // As all the saved positions have the same number of pieces, we can easily retrieve the list of positions that was saved at the time.
        // We divide the total positions by the number of moves and get several complete boards.
        // Since the colors and the piece type are stored in the same order, we assign the values.

        int numberOfPieces = data.numberOfPieces;
        List<Vector2> tempPositions = new List<Vector2>();
        List<Pieces.Piece> tempPieces = new List<Pieces.Piece>();
        List<Pieces.Colour> tempColours = new List<Pieces.Colour>();

        for (int i = 0; i < data.savedPositionsX.Length; i++)
        {
            tempPositions.Add(new Vector2(data.savedPositionsX[i], data.savedPositionsY[i]));
            tempPieces.Add(data.savedPieces[i]);
            tempColours.Add(data.savedColours[i]);

            numberOfPieces--;

            if (numberOfPieces == 0)
            {
                savedPositions.Add(new PositionRecord(tempPositions.ToArray(), tempPieces.ToArray(), tempColours.ToArray()));

                tempPositions.Clear();
                tempPieces.Clear();
                tempColours.Clear();

                numberOfPieces = data.numberOfPieces;
            }
        }

        // From the saved values of the pieces on the board, we instantiate the pieces from the prefabs in the "Resources" folder.

        for (int i = 0; i < data.whitePositionsX.Length; i++)
        {
            string path = "";

            switch (data.whitePieces[i])
            {
                case Pieces.Piece.Bishop:
                    path = "Pieces/bishopW";
                    break;
                case Pieces.Piece.King:
                    path = "Pieces/kingW";
                    break;
                case Pieces.Piece.Knight:
                    path = "Pieces/knightW";
                    break;
                case Pieces.Piece.Pawn:
                    path = "Pieces/pawnW";
                    break;
                case Pieces.Piece.Queen:
                    path = "Pieces/queenW";
                    break;
                case Pieces.Piece.Rook:
                    path = "Pieces/rookW";
                    break;
            }

            GameObject piece = Object.Instantiate(Resources.Load<GameObject>(path), new Vector2(data.whitePositionsX[i], data.whitePositionsY[i]), Quaternion.identity);

            // We must take into account if the piece has been previously moved.

            piece.GetComponent<PiecesMovement>().FirstMove = data.whiteFirstMove[i];

            // We add the piece to the pieces list.

            PiecesWhite.Add(piece);

            if (data.whitePieces[i] == Pieces.Piece.King)
            {
                whiteKing = PiecesWhite[i];
            }
        }

        // We repeat the same process with the black pieces.

        for (int i = 0; i < data.blackPositionsX.Length; i++)
        {
            string path = "";

            switch (data.blackPieces[i])
            {
                case Pieces.Piece.Bishop:
                    path = "Pieces/bishopB";
                    break;
                case Pieces.Piece.King:
                    path = "Pieces/kingB";
                    break;
                case Pieces.Piece.Knight:
                    path = "Pieces/knightB";
                    break;
                case Pieces.Piece.Pawn:
                    path = "Pieces/pawnB";
                    break;
                case Pieces.Piece.Queen:
                    path = "Pieces/queenB";
                    break;
                case Pieces.Piece.Rook:
                    path = "Pieces/rookB";
                    break;
            }

            GameObject piece = Object.Instantiate(Resources.Load<GameObject>(path), new Vector2(data.blackPositionsX[i], data.blackPositionsY[i]), Quaternion.identity);

            piece.GetComponent<PiecesMovement>().FirstMove = data.blackFirstMove[i];

            PiecesBlack.Add(piece);

            if (data.blackPieces[i] == Pieces.Piece.King)
            {
                blackKing = PiecesBlack[i];
            }
        }

        // We initialize the game variables and check if there is a king in check in the initial position.

        stalemate = false;
        IsPlaying = true;

        CheckVerification();

        // We activate the message to indicate which player should play.

        Interface.interfaceClass.SetWaitingMessage(playerInTurn);

        // If none of the above options have been met, the game continues.

        ResetValues();

        // We activate the AI if is its turn.

        if (singlePlay)
        {
            Interface.interfaceClass.EnableButtonPause(false);
            TimeEvents.timeEvents.StartWaitForAI();
        }

        EnableSelection();
    }

    /// <summary>
    /// Eliminate all the pieces on the screen, as well as deactivate the variables that indicate that it is being played or that the AI is active.
    /// This method is useful for clearing the board before starting a new game.
    /// </summary>
    public static void CleanScene()
    {
        foreach (GameObject piece in PiecesWhite)
        {
            Object.Destroy(piece);
        }

        foreach (GameObject piece in PiecesBlack)
        {
            Object.Destroy(piece);
        }

        DeselectPosition();

        PiecesWhite.Clear();
        PiecesBlack.Clear();

        checkPositionsWhite.Clear();
        checkPositionsBlack.Clear();

        PlayerColour = Enums.Colours.All;

        IsPlaying = false;
        singlePlay = false;
    }

    /// <summary>
    /// Place all the pieces on the board in an orderly manner, following the rules of chess.
    /// </summary>
    static void InitialSpawn()
    {
        // We instantiate all the white pieces from the prefabs in the "Resources" folder. Likewise, we indicate which piece is the king.

        whiteKing = Object.Instantiate(Resources.Load<GameObject>("Pieces/kingW"), new Vector2(5, 1), Quaternion.identity);
        PiecesWhite.Add(whiteKing);

        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/rookW"), new Vector2(1, 1), Quaternion.identity));
        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/rookW"), new Vector2(8, 1), Quaternion.identity));

        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/knightW"), new Vector2(2, 1), Quaternion.identity));
        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/knightW"), new Vector2(7, 1), Quaternion.identity));

        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/bishopW"), new Vector2(3, 1), Quaternion.identity));
        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/bishopW"), new Vector2(6, 1), Quaternion.identity));

        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/queenW"), new Vector2(4, 1), Quaternion.identity));

        for (int i = 1; i <= 8; i++)
        {
            PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/pawnW"), new Vector2(i, 2), Quaternion.identity));
        }

        // We do the same with the black pieces.

        blackKing = Object.Instantiate(Resources.Load<GameObject>("Pieces/kingB"), new Vector2(5, 8), Quaternion.identity);
        PiecesBlack.Add(blackKing);

        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/rookB"), new Vector2(1, 8), Quaternion.identity));
        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/rookB"), new Vector2(8, 8), Quaternion.identity));

        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/knightB"), new Vector2(2, 8), Quaternion.identity));
        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/knightB"), new Vector2(7, 8), Quaternion.identity));

        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/bishopB"), new Vector2(3, 8), Quaternion.identity));
        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/bishopB"), new Vector2(6, 8), Quaternion.identity));

        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/queenB"), new Vector2(4, 8), Quaternion.identity));

        for (int i = 1; i <= 8; i++)
        {
            PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/pawnB"), new Vector2(i, 7), Quaternion.identity));
        }
    }

    #endregion

    #region Movement

    /// <summary>
    /// Indicates if a game is currently playing.
    /// </summary>
    public static bool IsPlaying { get; set; }

    /// <summary>
    /// The piece selected to move on the turn.
    /// </summary>
    public static GameObject ActivePiece { get; set; } = null;

    /// <summary>
    /// The position of the selected piece.
    /// </summary>
    public static Vector2 ActivePiecePosition => ActivePiece.transform.position;

    /// <summary>
    /// Moves the selected piece to the indicated position.
    /// </summary>
    /// <param name="position">The position to which the piece will move.</param>
    public static void MovePiece(Vector2 position)
    {
        // We return all squares to their original color.

        ResetColour();

        // We increase the number of movements made to control if we have exceeded the allowed limit.

        movements++;

        // Depending on the type of piece that has been moved, there may be some peculiarities.

        MovementPeculiarities(position);

        // We move the piece to its new position.

        ActivePiece.transform.position = position;

        // We check if there is a piece of the opposite color in the square. If so, we will have captured it.

        Pieces.Colour colour = ActivePiece.GetComponent<PiecesMovement>().PieceColour;

        if (colour == Pieces.Colour.White)
        {
            if (CheckSquareBlack(position))
            {
                // The procedure that we will follow is to remove it from the corresponding list and then destroy it.

                GameObject capturedPiece = GetPieceBlackInPosition(position);

                PiecesBlack.Remove(capturedPiece);

                Object.Destroy(capturedPiece);

                // When a capture has taken place, we restart the movements.

                movements = 0;

                // Since there is one less piece, the new positions will not coincide with the saved positions, we delete them to save memory.

                savedPositions.Clear();
            }
        }

        else
        {
            if (CheckSquareWhite(position))
            {
                GameObject capturedPiece = GetPieceWhiteInPosition(position);

                PiecesWhite.Remove(capturedPiece);

                Object.Destroy(capturedPiece);

                movements = 0;

                savedPositions.Clear();
            }
        }

        // In the "Movement peculiarities" method, if there was a pawn at the opposite end of the board,
        // this variable is activated to start the promotion process.

        if (activePromotion)
        {
            //We activate the message on the screen based on the color of the player who is promoting.

            if (ActivePiece.GetComponent<PiecesMovement>().PieceColour == Pieces.Colour.White)
            {
                if (singlePlay && PlayerColour == Enums.Colours.Black)
                {
                    PieceSelectedToPromotion(Enums.PromotablePieces.Queen, Pieces.Colour.White);

                    return;
                }

                else
                {
                    DisableSelection();

                    Interface.interfaceClass.ActivatePromotionWhite(CheckTurn());
                }
            }

            else
            {
                if (singlePlay && PlayerColour == Enums.Colours.White)
                {
                    PieceSelectedToPromotion(Enums.PromotablePieces.Queen, Pieces.Colour.Black);

                    return;
                }

                else
                {
                    DisableSelection();

                    Interface.interfaceClass.ActivatePromotionBlack(CheckTurn());
                }
            }

            // After activating the message on the screen, we interrupt the execution of the method so that the player can choose the piece.

            return;
        }

        // We save the new position to see if it repeats in the future.

        savedPositions.Add(new PositionRecord(PiecesWhite, PiecesBlack));

        //if (movements == 100)
        //{
        //    return;
        //}

        // The next player's turn begins.

        NextTurn();
    }

    /// <summary>
    /// Check if the piece to be moved has any peculiarities.
    /// </summary>
    /// <param name="position">The position to which the piece will move.</param>
    static void MovementPeculiarities(Vector2 position)
    {
        // We identify the type of piece.

        Pieces.Piece kindOfPiece = ActivePiece.GetComponent<PiecesMovement>().PieceType;

        switch (kindOfPiece)
        {
            // In the case of pawns, it is checked whether they are in an En Passant position or if they are in a promotional position.
            // In addition, it is necessary to indicate that they have already been moved and reset the movements.

            case Pieces.Piece.Pawn:
                CheckEnPassant(position);
                ActivateEnPassant(position);
                VerifyPromotion(position);
                ActivePiece.GetComponent<PiecesMovement>().FirstMove = true;
                movements = 0;
                break;

            // In the case of towers, we indicate that they have already been moved so that castling can no longer be made.

            case Pieces.Piece.Rook:
                ActivePiece.GetComponent<PiecesMovement>().FirstMove = true;
                break;

            // If the one who moves is the king, we check if he can castling in that position.
            // We also indicate that it has already been moved so that castling is no longer available later.

            case Pieces.Piece.King:
                CheckCastling(position);
                ActivePiece.GetComponent<PiecesMovement>().FirstMove = true;
                break;
        }
    }

    /// <summary>
    /// Block certain movements of the white pieces so that they cannot checkmate themselves.
    /// </summary>
    /// <param name="unblock">True if we disable the lock, false if we enable it.</param>
    static void BlockMovementsWhite(bool unblock)
    {
        // At the start of White's turn, we block White's movements and unlock Black's.

        if (unblock)
        {
            foreach (GameObject piece in PiecesWhite)
            {
                piece.GetComponent<PiecesMovement>().DisableLock();
            }
        }

        else
        {
            foreach (GameObject piece in PiecesBlack)
            {
                piece.GetComponent<PiecesMovement>().ActivateForbiddenPositions();
            }
        }
    }

    /// <summary>
    /// Block certain movements of the black pieces so that they cannot checkmate themselves.
    /// </summary>
    /// <param name="unblock">True if we disable the lock, false if we enable it.</param>
    static void BlockMovementsBlack(bool unblock)
    {
        // At the start of Black's turn, we block Black's movements and unlock White's.

        if (unblock)
        {
            foreach (GameObject piece in PiecesBlack)
            {
                piece.GetComponent<PiecesMovement>().DisableLock();
            }
        }

        else
        {
            foreach (GameObject piece in PiecesWhite)
            {
                piece.GetComponent<PiecesMovement>().ActivateForbiddenPositions();
            }
        }
    }

    #endregion

    #region Castling

    /// <summary>
    /// The rook piece when a long castling is made.
    /// </summary>
    static GameObject castlingLeftRook = null;

    /// <summary>
    /// The rook piece when a short castling is made.
    /// </summary>
    static GameObject castlingRightRook = null;

    /// <summary>
    /// The position to which the king must move to make long castling.
    /// </summary>
    static Vector2 castlingLeftDestination;

    /// <summary>
    /// The position to which the king must move to make short castling.
    /// </summary>
    static Vector2 castlingRightDestination;

    /// <summary>
    /// The position to which the rook will move to make long castling.
    /// </summary>
    static Vector2 castlingLeftPosition;

    /// <summary>
    /// The position to which the rook will move to make short castling.
    /// </summary>
    static Vector2 castlingRightPosition;

    /// <summary>
    /// Check if the move made by the king allows a castling.
    /// </summary>
    /// <param name="position">The position to which the king is going to move.</param>
    static void CheckCastling(Vector2 position)
    {
        // If a long castling is in progress, we move the rook to the correct position.

        if (castlingLeftDestination == position)
        {
            castlingLeftRook.transform.position = castlingLeftPosition;
        }

        // We do the same if there is a short castling.

        else if (castlingRightDestination == position)
        {
            castlingRightRook.transform.position = castlingRightPosition;
        }
    }

    /// <summary>
    /// It lets you know if a long castling is in progress.
    /// </summary>
    /// <param name="colour">The colour of the moving piece.</param>
    /// <returns>If there has been a long castling move.</returns>
    public static bool CastlingLeft(Pieces.Colour colour)
    {
        if (colour == Pieces.Colour.White && CheckSquareWhite(new Vector2(1, 1)))
        {
            // If the white piece is moving, we check that the piece in A1 is the rook and that it has never moved.

            GameObject capturedPiece = GetPieceWhiteInPosition(new Vector2(1, 1));

            if (capturedPiece.GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Rook &&
                !capturedPiece.GetComponent<PiecesMovement>().FirstMove)
            {
                // If the positions the king will pass through are not empty or threatened, there is no castling.

                if (!CheckSquareEmpty(new Vector2(2, 1)))
                {
                    return false;
                }

                if (VerifyBlackCheckPosition(new Vector2(3, 1)) || !CheckSquareEmpty(new Vector2(3, 1)))
                {
                    return false;
                }

                if (VerifyBlackCheckPosition(new Vector2(4, 1)) || !CheckSquareEmpty(new Vector2(4, 1)))
                {
                    return false;
                }

                if (VerifyBlackCheckPosition(new Vector2(5, 1)))
                {
                    return false;
                }

                // If the above conditions are not met, there is castling. We assign the corresponding variables.

                else
                {
                    castlingLeftRook = capturedPiece;
                    castlingLeftPosition = new Vector2(4, 1);
                    castlingLeftDestination = new Vector2(3, 1);

                    return true;
                }
            }

            else
            {
                return false;
            }
        }

        if (colour == Pieces.Colour.Black && CheckSquareBlack(new Vector2(1, 8)))
        {
            // If the black piece is moving, we check that the piece in A8 is the rook and that it has never moved.

            GameObject capturedPiece = GetPieceBlackInPosition(new Vector2(1, 8));

            if (capturedPiece.GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Rook &&
                !capturedPiece.GetComponent<PiecesMovement>().FirstMove)
            {
                // If the positions the king will pass through are not empty or threatened, there is no castling.

                if (CheckSquareBlack(new Vector2(2, 8)) || CheckSquareWhite(new Vector2(2, 8)))
                {
                    return false;
                }

                if (VerifyWhiteCheckPosition(new Vector2(3, 8)) || CheckSquareBlack(new Vector2(3, 8)) || CheckSquareWhite(new Vector2(3, 8)))
                {
                    return false;
                }

                if (VerifyWhiteCheckPosition(new Vector2(4, 8)) || CheckSquareBlack(new Vector2(4, 8)) || CheckSquareWhite(new Vector2(4, 8)))
                {
                    return false;
                }

                if (VerifyWhiteCheckPosition(new Vector2(5, 8)))
                {
                    return false;
                }

                // If the above conditions are not met, there is castling. We assign the corresponding variables.

                else
                {
                    castlingLeftRook = capturedPiece;
                    castlingLeftPosition = new Vector2(4, 8);
                    castlingLeftDestination = new Vector2(3, 8);

                    return true;
                }
            }

            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
    }

    /// <summary>
    /// It lets you know if a short castling is in progress.
    /// </summary>
    /// <param name="colour">The colour of the moving piece.</param>
    /// <returns>If there has been a short castling move.</returns>
    public static bool CastlingRight(Pieces.Colour colour)
    {
        if (colour == Pieces.Colour.White && CheckSquareWhite(new Vector2(8, 1)))
        {
            // If the white piece is moving, we check that the piece in H8 is the rook and that it has never moved.

            GameObject capturedPiece = GetPieceWhiteInPosition(new Vector2(8, 1));

            if (capturedPiece.GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Rook &&
                !capturedPiece.GetComponent<PiecesMovement>().FirstMove)
            {
                // If the positions the king will pass through are not empty or threatened, there is no castling.

                if (VerifyBlackCheckPosition(new Vector2(7, 1)) || CheckSquareWhite(new Vector2(7, 1)) || CheckSquareBlack(new Vector2(7, 1)))
                {
                    return false;
                }

                if (VerifyBlackCheckPosition(new Vector2(6, 1)) || CheckSquareWhite(new Vector2(6, 1)) || CheckSquareBlack(new Vector2(6, 1)))
                {
                    return false;
                }

                if (VerifyBlackCheckPosition(new Vector2(5, 1)))
                {
                    return false;
                }

                // If the above conditions are not met, there is castling. We assign the corresponding variables.

                else
                {
                    castlingRightRook = capturedPiece;
                    castlingRightPosition = new Vector2(6, 1);
                    castlingRightDestination = new Vector2(7, 1);

                    return true;
                }
            }

            else
            {
                return false;
            }
        }

        if (colour == Pieces.Colour.Black && CheckSquareBlack(new Vector2(8, 8)))
        {
            // If the black piece is moving, we check that the piece in H8 is the rook and that it has never moved.

            GameObject capturedPiece = GetPieceBlackInPosition(new Vector2(8, 8));

            if (capturedPiece.GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Rook &&
                !capturedPiece.GetComponent<PiecesMovement>().FirstMove)
            {
                // If the positions the king will pass through are not empty or threatened, there is no castling.

                if (VerifyWhiteCheckPosition(new Vector2(7, 8)) || CheckSquareBlack(new Vector2(7, 8)) || CheckSquareWhite(new Vector2(7, 8)))
                {
                    return false;
                }

                if (VerifyWhiteCheckPosition(new Vector2(6, 8)) || CheckSquareBlack(new Vector2(6, 8)) || CheckSquareWhite(new Vector2(6, 8)))
                {
                    return false;
                }

                if (VerifyWhiteCheckPosition(new Vector2(5, 8)))
                {
                    return false;
                }

                // If the above conditions are not met, there is castling. We assign the corresponding variables.

                else
                {
                    castlingRightRook = capturedPiece;
                    castlingRightPosition = new Vector2(6, 8);
                    castlingRightDestination = new Vector2(7, 8);

                    return true;
                }
            }

            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
    }

    #endregion

    #region En Passant

    /// <summary>
    /// The position of the pawn that can be captured En Passant.
    /// </summary>
    static Vector2 enPassantPawnPosition;

    /// <summary>
    /// The position to which a pawn must move to make an En Passant capture of the opposing pawn.
    /// </summary>
    public static Vector2 EnPassantPosition { get; private set; }


    /// <summary>
    /// The pawn that has advanced two squares (has activated the En Passant capture).
    /// </summary>
    static GameObject enPassantPiece;

    /// <summary>
    /// Indicates whether En Passant capture is active (a pawn has advanced two squares).
    /// </summary>
    public static bool EnPassantActive { get; private set; }

    /// <summary>
    /// Activate En Passant capture chance (if applicable) after pawn move.
    /// </summary>
    /// <param name="position">The final position of the pawn after moving.</param>
    public static void ActivateEnPassant(Vector2 position)
    {
        // We give value to all variables according to the position of the pawn.

        if (ActivePiece.GetComponent<PiecesMovement>().FirstMove)
        {
            return;
        }

        if (position.y == 4)
        {
            EnPassantActive = true;
            enPassantPawnPosition = position;
            EnPassantPosition = new Vector2(enPassantPawnPosition.x, enPassantPawnPosition.y - 1);
            enPassantPiece = ActivePiece;
        }

        else if (position.y == 5)
        {
            EnPassantActive = true;
            enPassantPawnPosition = position;
            EnPassantPosition = new Vector2(enPassantPawnPosition.x, enPassantPawnPosition.y + 1);
            enPassantPiece = ActivePiece;
        }

        else
        {
            EnPassantActive = false;
            enPassantPawnPosition = Vector2.zero;
            EnPassantPosition = Vector2.zero;
            enPassantPiece = null;
        }
    }

    /// <summary>
    /// Reset all the variables related to the capture En Passant.
    /// </summary>
    static void DeactivateEnPassant()
    {
        EnPassantActive = false;

        enPassantPawnPosition = Vector2.zero;
        EnPassantPosition = Vector2.zero;
        enPassantPiece = null;
    }

    /// <summary>
    /// He checks if after the movement of a pawn he has captured another pawn En Passant.
    /// </summary>
    /// <param name="position">The position to which the pawn has moved.</param>
    static void CheckEnPassant(Vector2 position)
    {
        // If in the previous move a pawn did not activate the capture En Passant, there is nothing to do.

        if (!EnPassantActive)
        {
            return;
        }

        // If the En Passant capture was activated and has been used this turn, we capture the corresponding piece.

        if (position == EnPassantPosition)
        {
            if (enPassantPiece.GetComponent<PiecesMovement>().PieceColour == Pieces.Colour.White)
            {
                PiecesWhite.Remove(enPassantPiece);
            }

            else if (enPassantPiece.GetComponent<PiecesMovement>().PieceColour == Pieces.Colour.Black)
            {
                PiecesBlack.Remove(enPassantPiece);
            }

            Object.Destroy(enPassantPiece);
            enPassantPiece = null;

            DeactivateEnPassant();
        }
    }

    #endregion

    #region Pawn Promotion

    /// <summary>
    /// Indicates whether a pawn has been promoted this turn.
    /// </summary>
    static bool activePromotion = false;

    /// <summary>
    /// Check if a pawn has promoted after moving and activate the necessary variables.
    /// </summary>
    /// <param name="position">The position to which the pawn has moved.</param>
    static void VerifyPromotion(Vector2 position)
    {
        if (ActivePiece.GetComponent<PiecesMovement>().PieceColour == Pieces.Colour.White && position.y == 8)
        {
            activePromotion = true;
            savedPositions.Clear();
        }

        else if (ActivePiece.GetComponent<PiecesMovement>().PieceColour == Pieces.Colour.Black && position.y == 1)
        {
            activePromotion = true;
            savedPositions.Clear();
        }
    }

    /// <summary>
    /// It substitutes the pawn that it has promoted with the chosen piece.
    /// </summary>
    /// <param name="piece">The new piece chosen.</param>
    /// <param name="colour">The colour of the piece chosen.</param>
    public static void PieceSelectedToPromotion(Enums.PromotablePieces piece, Pieces.Colour colour)
    {
        Vector2 position = ActivePiece.transform.position;
        GameObject newPiece = null;

        if (colour == Pieces.Colour.White)
        {
            PiecesWhite.Remove(ActivePiece);
            Object.Destroy(ActivePiece);

            switch (piece)
            {
                case Enums.PromotablePieces.Rook:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/rookW"), position, Quaternion.identity);
                    break;
                case Enums.PromotablePieces.Knight:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/knightW"), position, Quaternion.identity);
                    break;
                case Enums.PromotablePieces.Bishop:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/bishopW"), position, Quaternion.identity);
                    break;
                case Enums.PromotablePieces.Queen:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/queenW"), position, Quaternion.identity);
                    break;
            }

            PiecesWhite.Add(newPiece);
            ActivePiece = newPiece;
        }

        else
        {
            PiecesBlack.Remove(ActivePiece);
            Object.Destroy(ActivePiece);

            switch (piece)
            {
                case Enums.PromotablePieces.Rook:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/rookB"), position, Quaternion.identity);
                    break;
                case Enums.PromotablePieces.Knight:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/knightB"), position, Quaternion.identity);
                    break;
                case Enums.PromotablePieces.Bishop:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/bishopB"), position, Quaternion.identity);
                    break;
                case Enums.PromotablePieces.Queen:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/queenB"), position, Quaternion.identity);
                    break;
            }

            PiecesBlack.Add(newPiece);
            ActivePiece = newPiece;
        }

        Interface.interfaceClass.DisablePromotions();

        activePromotion = false;

        savedPositions.Add(new PositionRecord(PiecesWhite, PiecesBlack));

        EnableSelection();

        NextTurn();
    }

    #endregion

    #region Check

    /// <summary>
    /// List of all positions where white pieces can check.
    /// </summary>
    static List<Vector2> checkPositionsWhite = new List<Vector2>();

    /// <summary>
    /// List of all positions where black pieces can check.
    /// </summary>
    static List<Vector2> checkPositionsBlack = new List<Vector2>();

    /// <summary>
    /// Indicates whether the white king is in check.
    /// </summary>
    public static bool WhiteKingInCheck { get; private set; }

    /// <summary>
    /// Indicates whether the black king is in check.
    /// </summary>
    public static bool BlackKingInCheck { get; private set; }

    /// <summary>
    /// The position of the white king on the board.
    /// </summary>
    public static Vector2 WhiteKingPosition => whiteKing.transform.position;

    /// <summary>
    /// The position of the black king on the board.
    /// </summary>
    public static Vector2 BlackKingPosition => blackKing.transform.position;

    /// <summary>
    /// Positions that can block the check of the white pieces.
    /// </summary>
    static List<Vector2> menacingWhitePositions = new List<Vector2>();

    /// <summary>
    /// Positions that can block the check of the black pieces.
    /// </summary>
    static List<Vector2> menacingBlackPositions = new List<Vector2>();

    /// <summary>
    /// Update the list of positions where the white pieces can capture.
    /// </summary>
    static void SetCheckPositionsWhite()
    {
        checkPositionsWhite.Clear();

        List<Vector2> tempList = new List<Vector2>();

        foreach (GameObject piece in PiecesWhite)
        {
            tempList = tempList.Concat(piece.GetComponent<PiecesMovement>().GetPositionsInCheck()).ToList();
        }

        checkPositionsWhite = tempList.Distinct().ToList();
    }

    /// <summary>
    /// Update the list of positions where the black pieces can capture.
    /// </summary>
    static void SetCheckPositionsBlack()
    {
        checkPositionsBlack.Clear();

        List<Vector2> tempList = new List<Vector2>();

        foreach (GameObject piece in PiecesBlack)
        {
            tempList = tempList.Concat(piece.GetComponent<PiecesMovement>().GetPositionsInCheck()).ToList();
        }

        checkPositionsBlack = tempList.Distinct().ToList();
    }

    /// <summary>
    /// Update the threatening position list for the black player.
    /// </summary>
    static void SetMenacingPositionsWhite()
    {
        menacingWhitePositions.Clear();

        List<Vector2> tempList = new List<Vector2>();

        foreach (GameObject piece in PiecesWhite)
        {
            tempList = tempList.Concat(piece.GetComponent<PiecesMovement>().GetMenacingPositions()).ToList();
        }

        menacingWhitePositions = tempList.Distinct().ToList();
    }

    /// <summary>
    /// Update the threatening position list for the white player.
    /// </summary>
    static void SetMenacingPositionsBlack()
    {
        menacingBlackPositions.Clear();

        List<Vector2> tempList = new List<Vector2>();

        foreach (GameObject piece in PiecesBlack)
        {
            tempList = tempList.Concat(piece.GetComponent<PiecesMovement>().GetMenacingPositions()).ToList();
        }

        menacingBlackPositions = tempList.Distinct().ToList();
    }

    /// <summary>
    /// We check if the turn starts with the king in check.
    /// </summary>
    public static void CheckVerification()
    {
        // We update both lists and block illegal movements.

        SetCheckPositionsWhite();
        SetMenacingPositionsWhite();
        BlockMovementsBlack(false);
        BlockMovementsWhite(true);

        SetCheckPositionsBlack();
        SetMenacingPositionsBlack();
        BlockMovementsWhite(false);
        BlockMovementsBlack(true);


        // We make sure that the check is for the player in turn.

        if (playerInTurn == Enums.Colours.Black)
        {            
            for (int i = 0; i < checkPositionsWhite.Count; i++)
            {
                // If the king is in any of the check positions, we issue a warning on the interface.

                if ((Vector2)blackKing.transform.position == checkPositionsWhite[i])
                {
                    BlackKingInCheck = true;
                    WhiteKingInCheck = false;

                    Interface.interfaceClass.ActivatePanelCheck(Enums.Colours.Black);

                    return;
                }

                // If he is not in check, we deactivate the panel (it may have been activated in the previous turn).

                else
                {
                    BlackKingInCheck = false;
                    WhiteKingInCheck = false;

                    Interface.interfaceClass.DeactivatePanelCheck();
                }
            }
        }

        else
        {
            for (int i = 0; i < checkPositionsBlack.Count; i++)
            {
                // If the king is in any of the check positions, we issue a warning on the interface.

                if ((Vector2)whiteKing.transform.position == checkPositionsBlack[i])
                {
                    WhiteKingInCheck = true;
                    BlackKingInCheck = false;

                    Interface.interfaceClass.ActivatePanelCheck(Enums.Colours.White);

                    return;
                }

                // If he is not in check, we deactivate the panel (it may have been activated in the previous turn).

                else
                {
                    WhiteKingInCheck = false;
                    BlackKingInCheck = false;

                    Interface.interfaceClass.DeactivatePanelCheck();
                }
            }
        }
    }

    /// <summary>
    /// Check if a specific position is in check by the white pieces.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns></returns>
    public static bool VerifyWhiteCheckPosition(Vector2 position)
    {
        for (int i = 0; i < checkPositionsWhite.Count; i++)
        {
            if (position == checkPositionsWhite[i])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check if a specific position is in check by the black pieces.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns></returns>
    public static bool VerifyBlackCheckPosition(Vector2 position)
    {
        for (int i = 0; i < checkPositionsBlack.Count; i++)
        {
            if (position == checkPositionsBlack[i])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check if a position is a threat from the white pieces.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns></returns>
    public static bool VerifyWhiteMenacedPosition(Vector2 position)
    {
        for (int i = 0; i < menacingWhitePositions.Count; i++)
        {
            if (position == menacingWhitePositions[i])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check if a position is a threat from the black pieces.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns></returns>
    public static bool VerifyBlackMenacedPosition(Vector2 position)
    {
        for (int i = 0; i < menacingBlackPositions.Count; i++)
        {
            if (position == menacingBlackPositions[i])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Locate the white piece in a specific position.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns></returns>
    public static GameObject GetPieceWhiteInPosition(Vector2 position)
    {
        for (int i = 0; i < PiecesWhite.Count; i++)
        {
            if ((Vector2)PiecesWhite[i].transform.position == position)
            {
                return PiecesWhite[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Locate the black piece in a specific position.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns></returns>
    public static GameObject GetPieceBlackInPosition(Vector2 position)
    {
        for (int i = 0; i < PiecesBlack.Count; i++)
        {
            if ((Vector2)PiecesBlack[i].transform.position == position)
            {
                return PiecesBlack[i];
            }
        }

        return null;
    }

    #endregion

    #region Checkmate

    /// <summary>
    /// Check if the white pieces have made a checkmate.
    /// </summary>
    /// <returns>If the white pieces have won.</returns>
    static bool CheckmateWhiteVerification()
    {
        // We calculate all possible legal moves for Black.

        List<Vector2> tempList = new List<Vector2>();

        foreach (GameObject piece in PiecesBlack)
        {
            tempList = tempList.Concat(piece.GetComponent<PiecesMovement>().SearchGreenPositions()).ToList();
        }

        tempList.Distinct().ToList();

        // We clean the list by removing the squares occupied by black pieces.

        for (int i = 0; i < tempList.Count; i++)
        {
            if (CheckSquareBlack(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        // If there is no possible move, there are two possible options.

        if (tempList.Count == 0)
        {
            // If the black king is in check, White wins.

            if (BlackKingInCheck)
            {
                return true;
            }

            // If not, the game ends in a draw. We activate the necessary variable.

            else
            {
                stalemate = true;

                return false;
            }
        }

        return false;
    }

    /// <summary>
    /// Check if the black pieces have made a checkmate.
    /// </summary>
    /// <returns>If the black pieces have won.</returns>
    static bool CheckmateBlackVerification()
    {
        // We calculate all possible legal moves for White.

        List<Vector2> tempList = new List<Vector2>();

        foreach (GameObject piece in PiecesWhite)
        {
            tempList = tempList.Concat(piece.GetComponent<PiecesMovement>().SearchGreenPositions()).ToList();
        }

        tempList.Distinct().ToList();

        // We clean the list by removing the squares occupied by white pieces.

        for (int i = 0; i < tempList.Count; i++)
        {
            if (CheckSquareWhite(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        // If there is no possible move, there are two possible options.

        if (tempList.Count == 0)
        {
            // If the white king is in check, Black wins.

            if (WhiteKingInCheck)
            {
                return true;
            }

            // If not, the game ends in a draw. We activate the necessary variable.

            else
            {
                stalemate = true;

                return false;
            }
        }

        return false;
    }

    #endregion

    #region Draw

    /// <summary>
    /// It indicates, after certain checks, if the shift is going to end in a draw by stalemate.
    /// </summary>
    static bool stalemate = false;

    /// <summary>
    /// Number of movements made without capturing pieces or pawn movements.
    /// </summary>
    static int movements = 0;

    /// <summary>
    /// History of saved positions to know if they are being repeated.
    /// </summary>
    static readonly List<PositionRecord> savedPositions = new List<PositionRecord>();

    /// <summary>
    /// Throw a message on the screen to indicate that the game ends in a draw.
    /// </summary>
    /// <param name="drawType">The reason the game ends in a draw.</param>
    static void FinishWithDraw(Enums.DrawModes drawType)
    {
        Interface.interfaceClass.ActivateDrawMessage(drawType);

        // We deactivate the possibility of choosing the squares of the board.

        DisableSelection();

        // We delete the game from the autosave.

        DeleteAutoSave();
    }

    /// <summary>
    /// Indicates if the game has ended in a draw because it is impossible to finish with the current pieces on the board.
    /// </summary>
    /// <returns>True if there are not enough pieces to finish the game.</returns>
    static bool DrawByImpossibility()
    {
        // We do different counts to see if it is impossible to win.

        if (PiecesWhite.Count == 1 && PiecesWhite.Count == 1)
        {
            return true;
        }

        if (PiecesWhite.Count == 2 && PiecesBlack.Count == 1 && PiecesWhite[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Knight)
        {
            return true;
        }

        if (PiecesBlack.Count == 2 && PiecesWhite.Count == 1 && PiecesBlack[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Knight)
        {
            return true;
        }

        if (PiecesWhite.Count == 2 && PiecesBlack.Count == 1 && PiecesWhite[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Bishop)
        {
            return true;
        }

        if (PiecesBlack.Count == 2 && PiecesWhite.Count == 1 && PiecesBlack[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Bishop)
        {
            return true;
        }

        if (PiecesWhite.Count == 3 && PiecesBlack.Count == 1 && PiecesWhite[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Knight
            && PiecesWhite[2].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Knight)
        {
            return true;
        }

        if (PiecesBlack.Count == 3 && PiecesWhite.Count == 1 && PiecesBlack[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Knight
            && PiecesBlack[2].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Knight)
        {
            return true;
        }

        if (PiecesWhite.Count == 2 && PiecesBlack.Count == 2 && PiecesWhite[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Bishop
            && PiecesBlack[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Bishop
            && (PiecesWhite[1].transform.position.x + PiecesWhite[1].transform.position.y) % 2 == (PiecesBlack[1].transform.position.x + PiecesBlack[1].transform.position.y) % 2)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Indicates whether the current position of the pieces on the board has been repeated three times.
    /// </summary>
    /// <returns></returns>
    static bool VerifyRepeatedPositions()
    {
        for (int i = 0; i < savedPositions.Count - 1; i++)
        {
            int repetitions = 0;

            for (int j = i + 1; j < savedPositions.Count; j++)
            {
                if (savedPositions[i].Equals(savedPositions[j]))
                {
                    repetitions++;

                    if (repetitions == 2)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    #endregion

    #region Turns

    /// <summary>
    /// The next player's turn begins, checking first for check, checkmate, or a draw.
    /// </summary>
    static void NextTurn()
    {
        // First we indicate that it is the other player's turn and we launch the message on the screen to indicate it.

        if (playerInTurn == Enums.Colours.White)
        {
            playerInTurn = Enums.Colours.Black;

            Interface.interfaceClass.SetWaitingMessage(Enums.Colours.Black);
        }

        else if (playerInTurn == Enums.Colours.Black)
        {
            playerInTurn = Enums.Colours.White;

            Interface.interfaceClass.SetWaitingMessage(Enums.Colours.White);
        }

        // After this we check if the game has ended by checkmate.

        CheckVerification();

        if (playerInTurn == Enums.Colours.White)
        {
            if (CheckmateWhiteVerification())
            {
                Interface.interfaceClass.ActivateCheckmateMessage(Enums.Colours.White);
                ResetColour();
                DisableSelection();
                DeleteAutoSave();

                return;
            }

            if (CheckmateBlackVerification())
            {
                Interface.interfaceClass.ActivateCheckmateMessage(Enums.Colours.Black);
                ResetColour();
                DisableSelection();
                DeleteAutoSave();

                return;
            }
        }

        else if (playerInTurn == Enums.Colours.Black)
        {
            if (CheckmateBlackVerification())
            {
                Interface.interfaceClass.ActivateCheckmateMessage(Enums.Colours.Black);
                ResetColour();
                DisableSelection();
                DeleteAutoSave();

                return;
            }

            if (CheckmateWhiteVerification())
            {
                Interface.interfaceClass.ActivateCheckmateMessage(Enums.Colours.White);
                ResetColour();
                DisableSelection();
                DeleteAutoSave();

                return;
            }
        }

        // If there is no checkmate, we check that there have not been 75 moves (150 turns) without piece capture or pawn moves.

        if (movements == 150)
        {
            FinishWithDraw(Enums.DrawModes.Move75);

            return;
        }

        // If the same position on the board has been repeated three times, the game is over.

        if (savedPositions.Count > 5 && VerifyRepeatedPositions())
        {
            FinishWithDraw(Enums.DrawModes.ThreefoldRepetition);

            return;
        }

        // We also check if the variable "stalemate" is true. In that case, the game also ends.

        if (stalemate)
        {
            FinishWithDraw(Enums.DrawModes.Stalemate);

            return;
        }

        // We also verify that you can win with the pieces on the board.

        if (PiecesWhite.Count <= 3 && PiecesBlack.Count <= 3 && DrawByImpossibility())
        {
            FinishWithDraw(Enums.DrawModes.Impossibility);

            return;
        }

        // If none of the above options have been met, the game continues.

        ResetValues();

        // We activate the AI if is its turn.

        if (singlePlay)
        {
            Interface.interfaceClass.EnableButtonPause(false);
            TimeEvents.timeEvents.StartWaitForAI();
        }
    }

    /// <summary>
    /// Indicates if it is the player's turn.
    /// </summary>
    /// <returns>If it is the player's turn.</returns>
    public static bool CheckTurn()
    {
        if (PlayerColour == Enums.Colours.All || PlayerColour == playerInTurn)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    /// <summary>
    /// Choose the piece located in a specific position for further movement.
    /// </summary>
    /// <param name="position">The position in which the piece to choose is.</param>
    /// <returns>True if there is a piece in position and it belongs to the player in turn.</returns>
    public static bool SelectPiece(Vector2 position)
    {
        if (playerInTurn == Enums.Colours.White)
        {
            GameObject piece = GetPieceWhiteInPosition(position);

            if (piece != null)
            {
                ActivePiece = piece;

                // All the squares that correspond to legal moves of the piece turn green.

                ChangeColour(ActivePiece.transform.position, ActivePiece.GetComponent<PiecesMovement>().SearchGreenPositions());

                return true;
            }
        }

        else if (playerInTurn == Enums.Colours.Black)
        {
            GameObject piece = GetPieceBlackInPosition(position);

            if (piece != null)
            {
                ActivePiece = piece;

                // All the squares that correspond to legal moves of the piece turn green.

                ChangeColour(ActivePiece.transform.position, ActivePiece.GetComponent<PiecesMovement>().SearchGreenPositions());

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Deselect the piece that was selected and reset the color of the squares.
    /// </summary>
    public static void DeselectPosition()
    {
        ActivePiece = null;

        ResetColour();
    }

    /// <summary>
    /// Reset the necessary values between shift and shift.
    /// </summary>
    public static void ResetValues()
    {
        if (enPassantPawnPosition != Vector2.zero && ActivePiece != null)
        {
            if (enPassantPawnPosition != (Vector2)ActivePiece.transform.position)
            {
                DeactivateEnPassant();
            }
        }

        ActivePiece = null;

        castlingLeftRook = null;
        castlingRightRook = null;
        castlingLeftDestination = Vector2.zero;
        castlingRightDestination = Vector2.zero;
        castlingLeftPosition = Vector2.zero;
        castlingRightPosition = Vector2.zero;

        AutoSave();
    }

    #endregion

    #region Square Colours

    /// <summary>
    /// Tell all the squares to change color through the delegate (if applicable).
    /// </summary>
    /// <param name="piecePosition">The position of the selected piece (turns yellow).</param>
    /// <param name="greenPositions">The positions to which the selected piece can be legally moved (turn green).</param>
    public static void ChangeColour(Vector2 piecePosition, List<Vector2> greenPositions)
    {
        ResetColour();

        if (CheckTurn())
        {
            UpdateColour(piecePosition, greenPositions);
        }
    }

    /// <summary>
    /// Return all pieces to their original color through the delegate.
    /// </summary>
    public static void ResetColour()
    {
        OriginalColour();
    }

    #endregion

    #region Examine Squares

    /// <summary>
    /// Checks if a selected square is empty.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns></returns>
    public static bool CheckSquareEmpty(Vector2 position)
    {
        for (int i = 0; i < PiecesWhite.Count; i++)
        {
            if ((Vector2)PiecesWhite[i].transform.position == position && PiecesWhite[i].activeSelf)
            {
                return false;
            }
        }

        for (int i = 0; i < PiecesBlack.Count; i++)
        {
            if ((Vector2)PiecesBlack[i].transform.position == position && PiecesBlack[i].activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if a selected square is occupied by a white piece.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns></returns>
    public static bool CheckSquareWhite(Vector2 position)
    {
        for (int i = 0; i < PiecesWhite.Count; i++)
        {
            if ((Vector2)PiecesWhite[i].transform.position == position && PiecesWhite[i].activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if a selected square is occupied by a black piece.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns></returns>
    public static bool CheckSquareBlack(Vector2 position)
    {
        for (int i = 0; i < PiecesBlack.Count; i++)
        {
            if ((Vector2)PiecesBlack[i].transform.position == position && PiecesBlack[i].activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region Save

    /// <summary>
    /// Save the game in the autosave slot.
    /// </summary>
    static void AutoSave()
    {
        SaveDataRaw data = new SaveDataRaw
        {
            playerInTurn = playerInTurn,
            enPassantDoublePosition = enPassantPawnPosition,
            enPassantPosition = EnPassantPosition,
            movements = movements,
            savedPositions = savedPositions,
            piecesWhite = PiecesWhite,
            piecesBlack = PiecesBlack
        };

        SaveManager.SaveGame(0, data);
    }

    /// <summary>
    /// Delete the game in the autosave slot.
    /// </summary>
    static void DeleteAutoSave()
    {
        SaveManager.DeleteAutoSave();

        Interface.interfaceClass.UpdateSaveDates();
    }

    /// <summary>
    /// Save the current game.
    /// </summary>
    /// <param name="saveSlot">The slot in which we want to save.</param>
    public static void SaveGame(int saveSlot)
    {
        SaveDataRaw data = new SaveDataRaw
        {
            playerInTurn = playerInTurn,
            enPassantDoublePosition = enPassantPawnPosition,
            enPassantPosition = EnPassantPosition,
            movements = movements,
            savedPositions = savedPositions,
            piecesWhite = PiecesWhite,
            piecesBlack = PiecesBlack
        };

        SaveManager.SaveGame(saveSlot, data);
    }

    #endregion

    #region Pause

    /// <summary>
    /// It made the pieces invisible and locks their selection during the pause.
    /// </summary>
    /// <param name="activePause">True if pause is being activated, false if it is being deactivated.</param>
    public static void PauseGame(bool activePause)
    {
        if (activePause)
        {
            foreach (GameObject piece in PiecesWhite)
            {
                piece.GetComponent<SpriteRenderer>().enabled = false;
            }

            foreach (GameObject piece in PiecesBlack)
            {
                piece.GetComponent<SpriteRenderer>().enabled = false;
            }

            DeselectPosition();
            DisableSelection();
        }

        else
        {
            foreach (GameObject piece in PiecesWhite)
            {
                piece.GetComponent<SpriteRenderer>().enabled = true;
            }

            foreach (GameObject piece in PiecesBlack)
            {
                piece.GetComponent<SpriteRenderer>().enabled = true;
            }

            EnableSelection();
        }
    }

    #endregion

    #region Artificial Intelligence

    /// <summary>
    /// We start the movement of the AI piece.
    /// </summary>
    public static void MoveAIPiece()
    {
        if (PlayerColour == Enums.Colours.White && playerInTurn == Enums.Colours.Black)
        {
            AIMovePosition bestMove = MiniMax.BestMovementBlack();
            ActivePiece = bestMove.piece;

            MovePiece(bestMove.position);
        }

        else if (PlayerColour == Enums.Colours.Black && playerInTurn == Enums.Colours.White)
        {
            AIMovePosition bestMove = MiniMax.BestMovementWhite();
            ActivePiece = bestMove.piece;

            MovePiece(bestMove.position);
        }
    }

    #endregion
}