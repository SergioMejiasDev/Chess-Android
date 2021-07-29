using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It contains the constants and methods necessary to allow the movement of the rooks.
/// </summary>
public class Rook
{
    /// <summary>
    /// The position of the piece.
    /// </summary>
    readonly Vector2 position;
    /// <summary>
    /// The color of the piece.
    /// </summary>
    readonly Pieces.Colour colour;

    public Rook(Vector2 position, Pieces.Colour colour)
    {
        this.position = position;
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
                // If white plays and the white king is in check, we eliminate the positions that cannot avoid checkmate.

                if (Chess.WhiteKingInCheck)
                {
                    List<Vector2> tempList = GetMovePositionsWhite();

                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (!Chess.VerifyBlackMenacedPosition(tempList[i]))
                        {
                            tempList.Remove(tempList[i]);

                            i--;
                        }
                    }

                    return tempList;
                }

                // If this is not the case, we return the original movement list.

                return GetMovePositionsWhite();
            }

            else
            {
                // If black plays and the black king is in check, we eliminate the positions that cannot avoid checkmate.

                if (Chess.BlackKingInCheck)
                {
                    List<Vector2> tempList = GetMovePositionsBlack();

                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (!Chess.VerifyWhiteMenacedPosition(tempList[i]))
                        {
                            tempList.Remove(tempList[i]);

                            i--;
                        }
                    }

                    return tempList;
                }

                // If this is not the case, we return the original movement list.

                return GetMovePositionsBlack();
            }
        }
    }

    /// <summary>
    /// List of positions where the piece can capture another.
    /// </summary>
    public List<Vector2> PositionsInCheck
    {
        get
        {
            return (colour == Pieces.Colour.White) ? GetPositionsInCheckWhite() : GetPositionsInCheckBlack();
        }
    }

    /// <summary>
    /// List of positions where the piece is a threat to the opposing king.
    /// </summary>
    public List<Vector2> MenacingPositions
    {
        get
        {
            return (colour == Pieces.Colour.White) ? GetMenacingPositionsWhite() : GetMenacingPositionsBlack();
        }
    }

    /// <summary>
    /// Block the movements of the nearby pieces that could cause a checkmate of this piece.
    /// </summary>
    public void ActivateForbiddenPositions()
    {
        if (colour == Pieces.Colour.White)
        {
            SetForbiddenPositionsWhite();
        }

        else
        {
            SetForbiddenPositionsBlack();
        }
    }

    /// <summary>
    /// The value of the piece at the current position.
    /// </summary>
    public int PositionValue => (colour == Pieces.Colour.White) ? GetPositionValueWhite() : GetPositionValueBlack();

    /// <summary>
    /// List of movements that a white piece can perform. This list of movements has not been filtered yet to eliminate illegal movements.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMovePositionsWhite()
    {
        List<Vector2> tempList = new List<Vector2>();

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                break;
            }

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));
            }

            else
            {
                tempList.Add(new Vector2(i, position.y));

                break;
            }
        }

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                break;
            }

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));
            }

            else
            {
                tempList.Add(new Vector2(i, position.y));

                break;
            }
        }

        for (int i = (int)position.y + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                break;
            }

            if (!Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));
            }

            else
            {
                tempList.Add(new Vector2(position.x, i));

                break;
            }
        }

        for (int i = (int)position.y - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                break;
            }

            if (!Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));
            }

            else
            {
                tempList.Add(new Vector2(position.x, i));

                break;
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
        List<Vector2> tempList = new List<Vector2>();

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                break;
            }

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));
            }

            else
            {
                tempList.Add(new Vector2(i, position.y));

                break;
            }
        }

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                break;
            }

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));
            }

            else
            {
                tempList.Add(new Vector2(i, position.y));

                break;
            }
        }

        for (int i = (int)position.y + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                break;
            }

            if (!Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));
            }

            else
            {
                tempList.Add(new Vector2(position.x, i));

                break;
            }
        }

        for (int i = (int)position.y - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                break;
            }

            if (!Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));
            }

            else
            {
                tempList.Add(new Vector2(position.x, i));

                break;
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
        List<Vector2> tempList = new List<Vector2>();

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));

                break;
            }

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));
            }

            else
            {
                tempList.Add(new Vector2(i, position.y));

                break;
            }
        }

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));

                break;
            }

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));
            }

            else
            {
                tempList.Add(new Vector2(i, position.y));

                break;
            }
        }

        for (int i = (int)position.y + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));

                break;
            }

            if (!Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));
            }

            else
            {
                tempList.Add(new Vector2(position.x, i));

                break;
            }
        }

        for (int i = (int)position.y - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));

                break;
            }

            if (!Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));
            }

            else
            {
                tempList.Add(new Vector2(position.x, i));

                break;
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
        List<Vector2> tempList = new List<Vector2>();

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));

                break;
            }

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));
            }

            else
            {
                tempList.Add(new Vector2(i, position.y));

                break;
            }
        }

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));

                break;
            }

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));
            }

            else
            {
                tempList.Add(new Vector2(i, position.y));

                break;
            }
        }

        for (int i = (int)position.y + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));

                break;
            }

            if (!Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));
            }

            else
            {
                tempList.Add(new Vector2(position.x, i));

                break;
            }
        }

        for (int i = (int)position.y - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));

                break;
            }

            if (!Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));
            }

            else
            {
                tempList.Add(new Vector2(position.x, i));

                break;
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
        List<Vector2> tempList = new List<Vector2>();

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                break;
            }

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));
            }

            else
            {
                if (Chess.BlackKingPosition == new Vector2(i, position.y))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        tempList.Clear();

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                break;
            }

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));
            }

            else
            {
                if (Chess.BlackKingPosition == new Vector2(i, position.y))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        tempList.Clear();

        for (int i = (int)position.y + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                break;
            }

            if (!Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));
            }

            else
            {
                if (Chess.BlackKingPosition == new Vector2(position.x, i))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        tempList.Clear();

        for (int i = (int)position.y - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                break;
            }

            if (!Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));
            }

            else
            {
                if (Chess.BlackKingPosition == new Vector2(position.x, i))
                {
                    tempList.Add(position);

                    return tempList;
                }
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
        List<Vector2> tempList = new List<Vector2>();

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                break;
            }

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));
            }

            else
            {
                if (Chess.WhiteKingPosition == new Vector2(i, position.y))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        tempList.Clear();

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                break;
            }

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                tempList.Add(new Vector2(i, position.y));
            }

            else
            {
                if (Chess.WhiteKingPosition == new Vector2(i, position.y))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        tempList.Clear();

        for (int i = (int)position.y + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                break;
            }

            if (!Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));
            }

            else
            {
                if (Chess.WhiteKingPosition == new Vector2(position.x, i))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        tempList.Clear();

        for (int i = (int)position.y - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                break;
            }

            if (!Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                tempList.Add(new Vector2(position.x, i));
            }

            else
            {
                if (Chess.WhiteKingPosition == new Vector2(position.x, i))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        return new List<Vector2>();
    }

    /// <summary>
    /// Block the movements of the nearby black pieces that could cause this piece to checkmate.
    /// </summary>
    void SetForbiddenPositionsWhite()
    {
        bool firstJump = false;
        GameObject selectedPiece = null;

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                break;
            }

            if (Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceBlackInPosition(new Vector2(i, position.y));

                    if ((Vector2)selectedPiece.transform.position == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Right);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y) == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Top);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Bottom);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        firstJump = false;
        selectedPiece = null;

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                break;
            }

            if (Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceBlackInPosition(new Vector2(i, position.y));

                    if ((Vector2)selectedPiece.transform.position == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Left);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y) == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Top);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Bottom);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        firstJump = false;
        selectedPiece = null;

        for (int i = (int)position.y + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                break;
            }

            if (Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceBlackInPosition(new Vector2(position.x, i));

                    if ((Vector2)selectedPiece.transform.position == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Top);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(position.x, i) == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Left);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Right);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        firstJump = false;
        selectedPiece = null;

        for (int i = (int)position.y - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                break;
            }

            if (Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceBlackInPosition(new Vector2(position.x, i));

                    if ((Vector2)selectedPiece.transform.position == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Bottom);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(position.x, i) == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Left);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Right);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Block the movements of the nearby white pieces that could cause this piece to checkmate.
    /// </summary>
    void SetForbiddenPositionsBlack()
    {
        bool firstJump = false;
        GameObject selectedPiece = null;

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                break;
            }

            if (Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceWhiteInPosition(new Vector2(i, position.y));

                    if ((Vector2)selectedPiece.transform.position == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Right);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y) == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Top);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Bottom);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        firstJump = false;
        selectedPiece = null;

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareBlack(new Vector2(i, position.y)))
            {
                break;
            }

            if (Chess.CheckSquareWhite(new Vector2(i, position.y)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceWhiteInPosition(new Vector2(i, position.y));

                    if ((Vector2)selectedPiece.transform.position == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Left);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y) == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Top);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Bottom);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        firstJump = false;
        selectedPiece = null;

        for (int i = (int)position.y + 1; i <= 8; i++)
        {
            if (Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                break;
            }

            if (Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceWhiteInPosition(new Vector2(position.x, i));

                    if ((Vector2)selectedPiece.transform.position == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Top);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(position.x, i) == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Left);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Right);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        firstJump = false;
        selectedPiece = null;

        for (int i = (int)position.y - 1; i >= 1; i--)
        {
            if (Chess.CheckSquareBlack(new Vector2(position.x, i)))
            {
                break;
            }

            if (Chess.CheckSquareWhite(new Vector2(position.x, i)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceWhiteInPosition(new Vector2(position.x, i));

                    if ((Vector2)selectedPiece.transform.position == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Bottom);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(position.x, i) == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Left);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.Right);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// The value of the white piece at the current position.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueWhite()
    {
        int[,] whiteValue =
        {
            { 00, 00, 00, -05, -05, 00, 00, 00 },
            { 05, 00, 00, 00, 00, 00, 00, 05 },
            { 05, 00, 00, 00, 00, 00, 00, 05 },
            { 05, 00, 00, 00, 00, 00, 00, 05 },
            { 05, 00, 00, 00, 00, 00, 00, 05 },
            { 05, 00, 00, 00, 00, 00, 00, 05 },
            { -05, -10, -10, -10, -10, -10, -10, -05 },
            { 00, 00, 00, 00, 00, 00, 00, 00 }
        };

        return -50 + whiteValue[(int)position.y - 1, (int)position.x - 1];
    }

    /// <summary>
    /// The value of the black piece at the current position.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueBlack()
    {
        int[,] blackValue =
        {
            { 00, 00, 00, 00, 00, 00, 00, 00 },
            { 05, 10, 10, 10, 10, 10, 10, 05 },
            { -05, 00, 00, 00, 00, 00, 00, -05 },
            { -05, 00, 00, 00, 00, 00, 00, -05 },
            { -05, 00, 00, 00, 00, 00, 00, -05 },
            { -05, 00, 00, 00, 00, 00, 00, -05 },
            { -05, 00, 00, 00, 00, 00, 00, -05 },
            { 00, 00, 00, 05, 05, 00, 00, 00 }
        };

        return 50 + blackValue[(int)position.y - 1, (int)position.x - 1];
    }
}