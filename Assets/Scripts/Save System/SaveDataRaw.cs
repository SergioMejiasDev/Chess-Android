using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Raw data that we want to save and have not gone through a conversion process to be serialized.
/// </summary>
public class SaveDataRaw
{
    /// <summary>
    /// The player whose turn it is to play at the time of saving.
    /// </summary>
    public Enums.Colours playerInTurn;

    /// <summary>
    /// The position of the pawn that is in the En Passant position.
    /// </summary>
    public Vector2 enPassantDoublePosition;

    /// <summary>
    /// The lethal position for the pawn in En Passant position.
    /// </summary>
    public Vector2 enPassantPosition;

    /// <summary>
    /// The number of moves made without moving pawns or capturing any pieces.
    /// </summary>
    public int movements;

    /// <summary>
    /// List of all saved past positions.
    /// </summary>
    public List<PositionRecord> savedPositions;

    /// <summary>
    /// List of all the white pieces on the board.
    /// </summary>
    public List<GameObject> piecesWhite;

    /// <summary>
    /// List of all the black pieces on the board.
    /// </summary>
    public List<GameObject> piecesBlack;
}
