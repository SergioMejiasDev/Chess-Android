using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour that will be added to each piece and that contains the necessary methods for their movement.
/// </summary>
public class PiecesMovement : MonoBehaviour
{
    /// <summary>
    /// The color of the piece.
    /// </summary>
    public Pieces.Colour PieceColour { get; private set; }

    /// <summary>
    /// The type of piece.
    /// </summary>
    public Pieces.Piece PieceType { get; private set; }

    /// <summary>
    /// Indicates if the piece has been moved for the first time.
    /// </summary>
    public bool FirstMove { get; set; } = false;

    /// <summary>
    /// Asset with the different variables and movements of the piece.
    /// </summary>
    [SerializeField] Pieces variables;

    /// <summary>
    /// Upward movements.
    /// </summary>
    bool lockTop = false;

    /// <summary>
    /// Movements to the right.
    /// </summary>
    bool lockRight = false;

    /// <summary>
    /// Downward movements.
    /// </summary>
    bool lockBottom = false;

    /// <summary>
    /// Movements to the left.
    /// </summary>
    bool lockLeft = false;

    /// <summary>
    /// Movements in the up-right diagonal.
    /// </summary>
    bool lockTopRight = false;

    /// <summary>
    /// Movements in the up-left diagonal.
    /// </summary>
    bool lockTopLeft = false;

    /// <summary>
    /// Movements in the lower-right diagonal.
    /// </summary>
    bool lockBottomRight = false;

    /// <summary>
    /// Movements in the lower-left diagonal.
    /// </summary>
    bool lockBottomLeft = false;

    void OnEnable()
    {
        PieceColour = variables.GetColour();
        PieceType = variables.GetPiece();
    }

    /// <summary>
    /// The value of the piece in its current position.
    /// </summary>
    public int Value
    {
        get
        {
            if (!gameObject.activeSelf)
            {
                return -variables.GetValue(transform.position, FirstMove);
            }

            return variables.GetValue(transform.position, FirstMove);
        }
    }

    /// <summary>
    /// Locks the piece movements in the specified direction.
    /// </summary>
    /// <param name="direction">The direction we want to block.</param>
    public void EnableLock(Pieces.Directions direction)
    {
        switch (direction)
        {
            case Pieces.Directions.Top:
                lockTop = true;
                break;
            case Pieces.Directions.Right:
                lockRight = true;
                break;
            case Pieces.Directions.Bottom:
                lockBottom = true;
                break;
            case Pieces.Directions.Left:
                lockLeft = true;
                break;
            case Pieces.Directions.TopRight:
                lockTopRight = true;
                break;
            case Pieces.Directions.TopLeft:
                lockTopLeft = true;
                break;
            case Pieces.Directions.BottomRight:
                lockBottomRight = true;
                break;
            case Pieces.Directions.BottomLeft:
                lockBottomLeft = true;
                break;
        }
    }

    /// <summary>
    /// Unlock all the moves that were locked.
    /// </summary>
    public void DisableLock()
    {
        lockTop = false;
        lockRight = false;
        lockBottom = false;
        lockLeft = false;
        lockTopLeft = false;
        lockTopRight = false;
        lockBottomLeft = false;
        lockBottomRight = false;
    }

    /// <summary>
    /// Calculate all the legal moves that a piece can make.
    /// </summary>
    /// <returns>The list with all possible moves.</returns>
    public List<Vector2> SearchGreenPositions()
    {
        // If the piece is deactivated (it happens in games against the AI), there is no legal move.
        // We return an empty list.

        if (!gameObject.activeSelf)
        {
            return new List<Vector2>();
        }

        // We calculate a list with all possible movements from the specific class of the piece.

        List<Vector2> tempList = variables.GetMovePositions(transform.position, FirstMove);

        // From the list we get as a result, we remove all blocked positions.

        for (int i = 0; i < tempList.Count; i++)
        {
            if (lockTop && tempList[i].y > transform.position.y && tempList[i].x == transform.position.x)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockRight && tempList[i].x > transform.position.x && tempList[i].y == transform.position.y)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockBottom && tempList[i].y < transform.position.y && tempList[i].x == transform.position.x)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockLeft && tempList[i].x < transform.position.x && tempList[i].y == transform.position.y)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockTopRight && tempList[i].x > transform.position.x && tempList[i].y > transform.position.y)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockTopLeft && tempList[i].x < transform.position.x && tempList[i].y > transform.position.y)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockBottomRight && tempList[i].x > transform.position.x && tempList[i].y < transform.position.y)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockBottomLeft && tempList[i].x < transform.position.x && tempList[i].y < transform.position.y)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }
        }

        return tempList;
    }

    /// <summary>
    /// Calculate all the positions where the piece can capture.
    /// </summary>
    /// <returns>The list of positions where the piece can capture.</returns>
    public List<Vector2> GetPositionsInCheck()
    {
        // If the piece is deactivated (it happens in games against the AI), there is no legal move.
        // We return an empty list.

        if (!gameObject.activeSelf)
        {
            return new List<Vector2>();
        }

        return variables.GetPositionsInCheck(transform.position, FirstMove);
    }

    /// <summary>
    /// Calculate the positions where the piece is lethal to the opposing king.
    /// </summary>
    /// <returns>The list of positions where the piece is lethal to the opposing king.</returns>
    public List<Vector2> GetMenacingPositions()
    {
        // If the piece is deactivated (it happens in games against the AI), there is no legal move.
        // We return an empty list.

        if (!gameObject.activeSelf)
        {
            return new List<Vector2>();
        }

        return variables.GetMenacingPositions(transform.position, FirstMove);
    }

    /// <summary>
    /// Block all the movements of the piece that could cause a check to our king.
    /// </summary>
    public void ActivateForbiddenPositions()
    {
        // If the piece is deactivated (it happens in games against the AI), there is no legal move.

        if (!gameObject.activeSelf)
        {
            return;
        }

        variables.ActivateForbiddenPosition(transform.position);
    }
}