using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It contains the constants and methods necessary to allow the king to move.
/// </summary>
public class King
{
    /// <summary>
    /// The position of the piece.
    /// </summary>
    readonly Vector2 position;
    /// <summary>
    /// Indicates if the part has been moved for the first time.
    /// </summary>
    readonly bool firstMove;
    /// <summary>
    /// The color of the piece.
    /// </summary>
    readonly Pieces.Colour colour;

    public King(Vector2 position, bool firstMove, Pieces.Colour colour)
    {
        this.position = position;
        this.firstMove = firstMove;
        this.colour = colour;
    }

    /// <summary>
    /// List of movements that the piece can perform before eliminating the blocked movements.
    /// </summary>
    public List<Vector2> MovePositions
    {
        get
        {
            if (colour == Pieces.Colour.White)
            {
                List<Vector2> tempList = GetMovePositionsWhite();

                for (int i = 0; i < tempList.Count; i++)
                {
                    if (Chess.VerifyBlackCheckPosition(tempList[i]))
                    {
                        tempList.Remove(tempList[i]);

                        i--;
                    }
                }

                return tempList;
            }

            else
            {
                List<Vector2> tempList = GetMovePositionsBlack();

                for (int i = 0; i < tempList.Count; i++)
                {
                    if (Chess.VerifyWhiteCheckPosition(tempList[i]))
                    {
                        tempList.Remove(tempList[i]);

                        i--;
                    }
                }

                return tempList;
            }
        }
    }

    /// <summary>
    /// List of positions where the piece can capture another.
    /// </summary>
    public List<Vector2> PositionsInCheck => (colour == Pieces.Colour.White) ? GetPositionsInCheckWhite() : GetPositionsInCheckBlack();

    /// <summary>
    /// List of positions where the piece is a threat to the opposing king.
    /// </summary>
    public List<Vector2> MenacingPositions => (colour == Pieces.Colour.White) ? GetMenacingPositionsWhite() : GetMenacingPositionsBlack();

    /// <summary>
    /// The value of the piece at the current position.
    /// </summary>
    public int PositionValue
    {
        get
        {
            bool whiteQueen = false;
            bool blackQueen = false;
            int whitePieces = 0;
            int blackPieces = 0;

            for (int i = 0; i < Chess.PiecesWhite.Count; i++)
            {
                if (Chess.PiecesWhite[i].activeSelf)
                {
                    whitePieces++;

                    if (Chess.PiecesWhite[i].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Queen)
                    {
                        whiteQueen = true;
                    }
                }
            }

            for (int i = 0; i < Chess.PiecesBlack.Count; i++)
            {
                if (Chess.PiecesBlack[i].activeSelf)
                {
                    blackPieces++;

                    if (Chess.PiecesBlack[i].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Queen)
                    {
                        blackQueen = true;
                    }
                }
            }

            if (colour == Pieces.Colour.White)
            {
                if ((!whiteQueen & !blackQueen) || (whiteQueen && whitePieces <= 3))
                {
                    return GetPositionValueWhiteEnd();
                }

                return GetPositionValueWhite();
            }

            else
            {
                if ((!whiteQueen & !blackQueen) || (blackQueen && blackPieces <= 3))
                {
                    return GetPositionValueBlackEnd();
                }

                return GetPositionValueBlack();
            }
        }
    }

    /// <summary>
    /// List of movements that a white piece can perform. This list of movements has not been filtered yet to eliminate illegal movements.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMovePositionsWhite()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x + 1, position.y + 1),
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x + 1, position.y - 1),
            new Vector2(position.x, position.y - 1),
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x - 1, position.y + 1)
        };

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].x < 1 || tempList[i].x > 8 || tempList[i].y < 1 || tempList[i].y > 8 || Chess.CheckSquareWhite(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        if (!firstMove)
        {
            if (Chess.CastlingLeft(colour))
            {
                tempList.Add(new Vector2(3, 1));
            }

            if (Chess.CastlingRight(colour))
            {
                tempList.Add(new Vector2(7, 1));
            }
        }

        for (int i = 0; i < tempList.Count; i++)
        {
            if (Chess.VerifyBlackCheckPosition(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        return tempList;
    }

    /// <summary>
    /// List of movements that a black piece can perform. This list of movements has not been filtered yet to eliminate illegal movements.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMovePositionsBlack()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x + 1, position.y + 1),
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x + 1, position.y - 1),
            new Vector2(position.x, position.y - 1),
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x - 1, position.y + 1)
        };

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].x < 1 || tempList[i].x > 8 || tempList[i].y < 1 || tempList[i].y > 8 || Chess.CheckSquareBlack(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        if (!firstMove)
        {
            if (Chess.CastlingLeft(colour))
            {
                tempList.Add(new Vector2(3, 8));
            }

            if (Chess.CastlingRight(colour))
            {
                tempList.Add(new Vector2(7, 8));
            }
        }

        for (int i = 0; i < tempList.Count; i++)
        {
            if (Chess.VerifyWhiteCheckPosition(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        return tempList;
    }

    /// <summary>
    /// List of positions where the white piece can capture another.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetPositionsInCheckWhite()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x + 1, position.y + 1),
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x + 1, position.y - 1),
            new Vector2(position.x, position.y - 1),
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x - 1, position.y + 1)
        };

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].x < 1 || tempList[i].x > 8 || tempList[i].y < 1 || tempList[i].y > 8 || Chess.CheckSquareWhite(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        return tempList;
    }

    /// <summary>
    /// List of positions where the black piece can capture another.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetPositionsInCheckBlack()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x + 1, position.y + 1),
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x + 1, position.y - 1),
            new Vector2(position.x, position.y - 1),
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x - 1, position.y + 1)
        };

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].x < 1 || tempList[i].x > 8 || tempList[i].y < 1 || tempList[i].y > 8 || Chess.CheckSquareBlack(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        return tempList;
    }

    /// <summary>
    /// List of positions where the white piece is a threat to the black player.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMenacingPositionsWhite()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x + 1, position.y + 1),
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x + 1, position.y - 1),
            new Vector2(position.x, position.y - 1),
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x - 1, position.y + 1)
        };

        for (int i = 0; i < tempList.Count; i++)
        {
            if (Chess.BlackKingPosition == tempList[i])
            {
                return new List<Vector2> { position };
            }
        }

        return new List<Vector2>();
    }

    /// <summary>
    /// List of positions where the black piece is a threat to the white player.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMenacingPositionsBlack()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x + 1, position.y + 1),
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x + 1, position.y - 1),
            new Vector2(position.x, position.y - 1),
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x - 1, position.y + 1)
        };

        for (int i = 0; i < tempList.Count; i++)
        {
            if (Chess.WhiteKingPosition == tempList[i])
            {
                return new List<Vector2> { position };
            }
        }

        return new List<Vector2>();
    }

    /// <summary>
    /// The value of the white piece at the current position.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueWhite()
    {
        int[,] whiteValue =
        {
            { -20, -30, -10, 00, 00, -10, -30, -20 },
            { -20, -20, 00, 00, 00, 00, -20, -20 },
            { 10, 20, 20, 20, 20, 20, 20, 10 },
            { 20, 30, 30, 40, 40, 30, 30, 20 },
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 30, 40, 40, 50, 50, 40, 40, 30 }
        };

        return -900 + whiteValue[(int)position.y - 1, (int)position.x - 1];
    }

    /// <summary>
    /// The value of the black piece at the current position.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueBlack()
    {
        int[,] blackValue =
        {
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 20, 30, 30, 40, 40, 30, 30, 20 },
            { 10, 20, 20, 20, 20, 20, 20, 10 },
            { -20, -20, 00, 00, 00, 00, -20, -20 },
            { -20, -30, -10, 00, 00, -10, -30, -20 }
        };

        return 900 + blackValue[(int)position.y - 1, (int)position.x - 1];
    }

    /// <summary>
    /// An alternative value for the white king in the current position when the game is more advanced.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueWhiteEnd()
    {
        int[,] whiteValueEnd =
        {
            { -50, -30, -30, -30, -30, -30, -30, -50 },
            { -30, -30, 00, 00, 00, 00, -30, -30 },
            { -30, -10, 20, 30, 30, 20, -10, -30 },
            { -30, -10, 30, 40, 40, 30, -10, -30 },
            { -30, -10, 30, 40, 40, 30, -10, -30 },
            { -30, -10, 20, 30, 30, 20, -10, -30 },
            { -30, -20, -10, 00, 00, -10, -20, -30 },
            { -50, -40, -30, -20, -20, -30, -40, -50 }
        };

        return -900 + whiteValueEnd[(int)position.y - 1, (int)position.x - 1];
    }

    /// <summary>
    /// An alternative value for the black king in the current position when the game is more advanced.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueBlackEnd()
    {
        int[,] blackValueEnd =
        {
            { 50, 40, 30, 20, 20, 30, 40, 50 },
            { 30, 20, 10, 00, 00, 10, 20, 30 },
            { 30, 10, -20, -30, -30, -20, 10, 30 },
            { 30, 10, -30, -40, -40, -30, 10, 30 },
            { 30, 10, -30, -40, -40, -30, 10, 30 },
            { 30, 10, -20, -30, -30, -20, 10, 30 },
            { 30, 30, 00, 00, 00, 00, 30, 30 },
            { 50, 30, 30, 30, 30, 30, 30, 50 }
        };

        return 900 + blackValueEnd[(int)position.y - 1, (int)position.x - 1];
    }
}