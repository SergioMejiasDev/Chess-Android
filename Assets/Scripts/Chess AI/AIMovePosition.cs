using UnityEngine;

/// <summary>
/// It contains the data of the part to be moved, and to which position it will be moved. It is used by the game's AI to choose a move.
/// </summary>
public class AIMovePosition
{
    /// <summary>
    /// The piece that is going to move.
    /// </summary>
    public readonly GameObject piece;

    /// <summary>
    /// The position to which the piece is going to move.
    /// </summary>
    public readonly Vector2 position;

    public AIMovePosition(GameObject piece, Vector2 position)
    {
        this.piece = piece;
        this.position = position;
    }
}