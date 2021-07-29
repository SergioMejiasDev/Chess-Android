using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The state of the board at a specific moment of the game. It is stored to verify the rule of threefold repetition.
/// </summary>
public class PositionRecord
{
    /// <summary>
    /// The positions of all the pieces on the board.
    /// </summary>
    public readonly List<Vector2> positions;

    /// <summary>
    /// All types of pieces on the board.
    /// </summary>
    public readonly List<Pieces.Piece> pieces;

    /// <summary>
    /// All the colors of the pieces on the board.
    /// </summary>
    public readonly List<Pieces.Colour> colours;

    /// <summary>
    /// Constructor that is created when a position is to be saved.
    /// </summary>
    /// <param name="whitePieces">The white pieces on the board.</param>
    /// <param name="blackPieces">The black pieces on the board.</param>
    public PositionRecord(List<GameObject> whitePieces, List<GameObject> blackPieces)
    {
        // We initialize three lists to save the data of the pieces (position, type of piece and color).

        List<Vector2> tempPositions = new List<Vector2>();
        List<Pieces.Piece> tempPieces = new List<Pieces.Piece>();
        List<Pieces.Colour> tempColours = new List<Pieces.Colour>();

        // We store the values in the list consecutively for the two colors.

        for (int i = 0; i < whitePieces.Count; i++)
        {
            tempPositions.Add(whitePieces[i].transform.position);
            tempPieces.Add(whitePieces[i].GetComponent<PiecesMovement>().PieceType);
            tempColours.Add(Pieces.Colour.White);
        }

        for (int i = 0; i < blackPieces.Count; i++)
        {
            tempPositions.Add(blackPieces[i].transform.position);
            tempPieces.Add(blackPieces[i].GetComponent<PiecesMovement>().PieceType);
            tempColours.Add(Pieces.Colour.Black);
        }

        // We store the values in the global variables.

        positions = tempPositions;
        pieces = tempPieces;
        colours = tempColours;
    }

    /// <summary>
    /// Constructor that is created when we want to rescue saved positions from a saved game.
    /// </summary>
    /// <param name="savedPositions">The positions of all the pieces on the board.</param>
    /// <param name="savedPieces">The types of all the pieces on the board.</param>
    /// <param name="savedColours">The colours of all the pieces on the board.</param>
    public PositionRecord(Vector2[] savedPositions, Pieces.Piece[] savedPieces, Pieces.Colour[] savedColours)
    {
        // We store the values (arrays) in the global variables (lists).

        positions = savedPositions.ToList();
        pieces = savedPieces.ToList();
        colours = savedColours.ToList();
    }

    /// <summary>
    /// Method to obtain a list of all the X values of the position vectors.
    /// </summary>
    /// <returns>A list of integers with all positions in X (we cannot serialize the vectors to save the game).</returns>
    public List<int> GetPositionsX()
    {
        List<int> tempList = new List<int>();

        for (int i = 0; i < positions.Count; i++)
        {
            tempList.Add((int)positions[i].x);
        }

        return tempList;
    }

    /// <summary>
    /// Method to obtain a list of all the Y values of the position vectors.
    /// </summary>
    /// <returns>A list of integers with all positions in Y (we cannot serialize the vectors to save the game).</returns>
    public List<int> GetPositionsY()
    {
        List<int> tempList = new List<int>();

        for (int i = 0; i < positions.Count; i++)
        {
            tempList.Add((int)positions[i].y);
        }

        return tempList;
    }

    /// <summary>
    /// Method to know if two saved positions are exactly the same.
    /// </summary>
    /// <param name="other">The position with which we want to compare the data in this class.</param>
    /// <returns>True if the two positions are equal, false if they are not.</returns>
    public bool Equals(PositionRecord other)
    {
        // If they are not the same size they are not the same, we save the checks.

        if (positions.Count != other.positions.Count)
        {
            return false;
        }

        // We check that all the values are equal (due to the way the board was generated, if they are the same pieces, they are always saved in the same order).

        for (int i = 0; i < positions.Count; i++)
        {
            if (positions[i] != other.positions[i] || pieces[i] != other.pieces[i] || colours[i] != other.colours[i])
            {
                // As soon as there is the smallest difference, the positions are not equal.

                return false;
            }
        }

        return true;
    }
}