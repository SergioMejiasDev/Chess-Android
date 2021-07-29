using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The different data related to a specific game that will be saved in a serialized file.
/// </summary>
[Serializable]
public class SaveData
{
    /// <summary>
    /// String where the date and time of the save is recorded.
    /// </summary>
    public readonly string saveDate;


    /// <summary>
    /// The player whose turn it is to play at the time of saving.
    /// </summary>
    public readonly Enums.Colours playerInTurn;

    /// <summary>
    /// The X value of the position of the pawn that is in the En Passant position.
    /// </summary>
    public readonly int enPassantDoublePositionX;

    /// <summary>
    /// The Y value of the position of the pawn that is in the En Passant position.
    /// </summary>
    public readonly int enPassantDoublePositionY;

    /// <summary>
    /// The X value of the lethal position for the pawn in En Passant position.
    /// </summary>
    public readonly int enPassantPositionX;

    /// <summary>
    /// The Y value of the lethal position for the pawn in En Passant position.
    /// </summary>
    public readonly int enPassantPositionY;

    /// <summary>
    /// The number of moves made without moving pawns or capturing any pieces.
    /// </summary>
    public readonly int movements;


    /// <summary>
    /// Number of past positions to be saved.
    /// </summary>
    public readonly int positionsSaved;

    /// <summary>
    /// The number of pieces of all stored positions.
    /// </summary>
    public int numberOfPieces;

    /// <summary>
    /// The X position of all stored positions.
    /// </summary>
    public int[] savedPositionsX;

    /// <summary>
    /// The Y position of all stored positions.
    /// </summary>
    public int[] savedPositionsY;

    /// <summary>
    /// All part types from saved positions.
    /// </summary>
    public Pieces.Piece[] savedPieces;

    /// <summary>
    /// All colours from saved positions.
    /// </summary>
    public Pieces.Colour[] savedColours;


    /// <summary>
    /// All the positions in X of the white pieces that are on the board.
    /// </summary>
    public int[] whitePositionsX;

    /// <summary>
    /// All the positions in Y of the white pieces that are on the board.
    /// </summary>
    public int[] whitePositionsY;

    /// <summary>
    /// All the positions in Y of the black pieces that are on the board.
    /// </summary>
    public int[] blackPositionsX;

    /// <summary>
    /// All the positions in Y of the black pieces that are on the board.
    /// </summary>
    public int[] blackPositionsY;


    /// <summary>
    /// The types of the white pieces on the board.
    /// </summary>
    public Pieces.Piece[] whitePieces;

    /// <summary>
    /// The types of the black pieces on the board.
    /// </summary>
    public Pieces.Piece[] blackPieces;

    /// <summary>
    /// List of Booleans that indicate whether the white pieces on the board have moved for the first time.
    /// </summary>
    public bool[] whiteFirstMove;

    /// <summary>
    /// List of Booleans that indicate whether the black pieces on the board have moved for the first time.
    /// </summary>
    public bool[] blackFirstMove;


    /// <summary>
    /// Constructor of the class from the raw data so that it can be serialized.
    /// </summary>
    /// <param name="data">Raw data that cannot be serialized.</param>
    public SaveData(SaveDataRaw data)
    {
        // We obtain the date and time data.

        saveDate = SetDate();

        // We save various data of the game, such as the movements made or the En Passant position if it exists.

        playerInTurn = data.playerInTurn;
        enPassantDoublePositionX = (int)data.enPassantDoublePosition.x;
        enPassantDoublePositionY = (int)data.enPassantDoublePosition.y;
        enPassantPositionX = (int)data.enPassantPosition.x;
        enPassantPositionY = (int)data.enPassantPosition.y;
        movements = data.movements;

        // We save the data of past positions.

        positionsSaved = data.savedPositions.Count;
        SetPositionRecord(data.savedPositions);

        // Finally, we save the current board data.

        SetPositions(data.piecesWhite, data.piecesBlack);
        SetPieces(data.piecesWhite, data.piecesBlack);
        SetFirstMove(data.piecesWhite, data.piecesBlack);
    }

    /// <summary>
    /// Calculate the date and time of the save.
    /// </summary>
    /// <returns>String in the format "DD-MM-YYYY  HH:MM:SS".</returns>
    string SetDate()
    {
        DateTime time = DateTime.Now;

        return time.ToString("dd-MM-yyyy  HH:mm:ss");
    }

    /// <summary>
    /// Convert raw data from saved past positions to data that can be serialized.
    /// </summary>
    /// <param name="savedPositions">List with saved positions.</param>
    void SetPositionRecord(List<PositionRecord> savedPositions)
    {
        // First of all we take note of the number of positions saved for when it is time to load the data.

        numberOfPieces = savedPositions[0].positions.Count;

        // We create different lists to save the positions (one for X and one for Y), the types of pieces and the colors.

        List<int> tempListPositionsX = new List<int>();
        List<int> tempListPositionsY = new List<int>();
        List<Pieces.Piece> tempListPieces = new List<Pieces.Piece>();
        List<Pieces.Colour> tempListColours = new List<Pieces.Colour>();

        for (int i = 0; i < savedPositions.Count; i++)
        {
            tempListPositionsX = tempListPositionsX.Concat(savedPositions[i].GetPositionsX()).ToList();
            tempListPositionsY = tempListPositionsY.Concat(savedPositions[i].GetPositionsY()).ToList();
            tempListPieces = tempListPieces.Concat(savedPositions[i].pieces).ToList();
            tempListColours = tempListColours.Concat(savedPositions[i].colours).ToList();
        }

        // We convert all the lists into arrays to be able to serialize them.

        savedPositionsX = tempListPositionsX.ToArray();
        savedPositionsY = tempListPositionsY.ToArray();
        savedPieces = tempListPieces.ToArray();
        savedColours = tempListColours.ToArray();
    }

    /// <summary>
    /// Converts raw positions from pieces on the board to data that can be serialized.
    /// </summary>
    /// <param name="piecesWhite">List with the white pieces on the board.</param>
    /// <param name="piecesBlack">List with the black pieces on the board.</param>
    void SetPositions(List<GameObject> piecesWhite, List<GameObject> piecesBlack)
    {
        // We create two lists to save the X and Y positions of the pieces, finally saving them as an array.

        List<int> tempListX = new List<int>();
        List<int> tempListY = new List<int>();

        for (int i = 0; i < piecesWhite.Count; i++)
        {
            tempListX.Add((int)piecesWhite[i].transform.position.x);
            tempListY.Add((int)piecesWhite[i].transform.position.y);
        }

        whitePositionsX = tempListX.ToArray();
        whitePositionsY = tempListY.ToArray();

        // We reuse the variables and do the same with the black pieces.

        tempListX.Clear();
        tempListY.Clear();

        for (int i = 0; i < piecesBlack.Count; i++)
        {
            tempListX.Add((int)piecesBlack[i].transform.position.x);
            tempListY.Add((int)piecesBlack[i].transform.position.y);
        }

        blackPositionsX = tempListX.ToArray();
        blackPositionsY = tempListY.ToArray();
    }

    /// <summary>
    /// Converts raw types from pieces on the board to data that can be serialized.
    /// </summary>
    /// <param name="piecesWhite">List with the white pieces on the board.</param>
    /// <param name="piecesBlack">List with the black pieces on the board.</param>
    void SetPieces(List<GameObject> piecesWhite, List<GameObject> piecesBlack)
    {
        // We create a temporary list to store all types of white pieces on the board.

        List<Pieces.Piece> tempList = new List<Pieces.Piece>();

        for (int i = 0; i < piecesWhite.Count; i++)
        {
            tempList.Add(piecesWhite[i].GetComponent<PiecesMovement>().PieceType);
        }

        whitePieces = tempList.ToArray();

        // We reuse the variable to do the same with black.

        tempList.Clear();

        for (int i = 0; i < piecesBlack.Count; i++)
        {
            tempList.Add(piecesBlack[i].GetComponent<PiecesMovement>().PieceType);

            blackPieces = tempList.ToArray();
        }
    }

    /// <summary>
    /// Convert raw data with first move information to data that can be serialized.
    /// </summary>
    /// <param name="piecesWhite">List with the white pieces on the board.</param>
    /// <param name="piecesBlack">List with the black pieces on the board.</param>
    void SetFirstMove(List<GameObject> piecesWhite, List<GameObject> piecesBlack)
    {
        // We create a temporary list to store all first-move values of white pieces on the board.

        List<bool> tempList = new List<bool>();

        for (int i = 0; i < piecesWhite.Count; i++)
        {
            tempList.Add(piecesWhite[i].GetComponent<PiecesMovement>().FirstMove);
        }

        whiteFirstMove = tempList.ToArray();

        // We reuse the variable to do the same with black.

        tempList.Clear();

        for (int i = 0; i < piecesBlack.Count; i++)
        {
            tempList.Add(piecesBlack[i].GetComponent<PiecesMovement>().FirstMove);

            blackFirstMove = tempList.ToArray();
        }
    }
}