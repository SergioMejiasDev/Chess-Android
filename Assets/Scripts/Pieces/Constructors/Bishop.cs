using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It contains the constants and methods necessary to allow bishop movement.
/// </summary>
public class Bishop
{
    /// <summary>
    /// The position of the piece.
    /// </summary>
    readonly Vector2 position;
    /// <summary>
    /// The color of the piece.
    /// </summary>
    readonly Pieces.Colour colour;

    public Bishop(Vector2 position, Pieces.Colour colour)
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
    public List<Vector2> PositionsInCheck => (colour == Pieces.Colour.White) ? GetPositionsInCheckWhite() : GetPositionsInCheckBlack();

    /// <summary>
    /// List of positions where the piece is a threat to the opposing king.
    /// </summary>
    public List<Vector2> MenacingPositions => (colour == Pieces.Colour.White) ? GetMenacingPositionsWhite() : GetMenacingPositionsBlack();

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

        int loops = 0;

        // Up-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a white piece in position or it is off the board, we do not keep looking in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)) || position.y + loops > 8)
            {
                break;
            }

            // If the square is empty, we add it to the move list and keep looking in this direction.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // If there is a black piece in the square, we add the position to the list and stop looking in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }
        }

        loops = 0;

        // Down-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a white piece in position or it is off the board, we do not keep looking in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)) || position.y - loops < 1)
            {
                break;
            }

            // If the square is empty, we add it to the move list and keep looking in this direction.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // If there is a black piece in the square, we add the position to the list and stop looking in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }
        }

        loops = 0;

        // Up-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a white piece in position or it is off the board, we do not keep looking in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)) || position.y + loops > 8)
            {
                break;
            }

            // If the square is empty, we add it to the move list and keep looking in this direction.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // If there is a black piece in the square, we add the position to the list and stop looking in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }
        }

        loops = 0;

        // Down-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a white piece in position or it is off the board, we do not keep looking in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)) || position.y - loops < 1)
            {
                break;
            }

            // If the square is empty, we add it to the move list and keep looking in this direction.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // If there is a black piece in the square, we add the position to the list and stop looking in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

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

        int loops = 0;

        // Up-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a black piece in position or it is off the board, we do not keep looking in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)) || position.y + loops > 8)
            {
                break;
            }

            // If the square is empty, we add it to the move list and keep looking in this direction.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // If there is a white piece in the square, we add the position to the list and stop looking in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }
        }

        loops = 0;

        // Down-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a black piece in position or it is off the board, we do not keep looking in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)) || position.y - loops < 1)
            {
                break;
            }

            // If the square is empty, we add it to the move list and keep looking in this direction.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // If there is a white piece in the square, we add the position to the list and stop looking in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }
        }

        loops = 0;

        // Up-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a black piece in position or it is off the board, we do not keep looking in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)) || position.y + loops > 8)
            {
                break;
            }

            // If the square is empty, we add it to the move list and keep looking in this direction.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // If there is a white piece in the square, we add the position to the list and stop looking in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }
        }

        loops = 0;

        // Down-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a black piece in position or it is off the board, we do not keep looking in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)) || position.y - loops < 1)
            {
                break;
            }

            // If the square is empty, we add it to the move list and keep looking in this direction.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // If there is a white piece in the square, we add the position to the list and stop looking in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

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

        int loops = 0;

        // Up-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a white piece in the square, we add it to the list and look no further in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }

            // If the square is empty, we add it to the list and keep looking in the same direction.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // If there is a black piece in the square, we add it to the list and look no further in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }
        }

        loops = 0;

        // Down-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a white piece in the square, we add it to the list and look no further in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }

            // If the square is empty, we add it to the list and keep looking in the same direction.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // If there is a black piece in the square, we add it to the list and look no further in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }
        }

        loops = 0;

        // Up-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a white piece in the square, we add it to the list and look no further in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }

            // If the square is empty, we add it to the list and keep looking in the same direction.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // If there is a black piece in the square, we add it to the list and look no further in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }
        }

        loops = 0;

        // Down-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a white piece in the square, we add it to the list and look no further in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }

            // If the square is empty, we add it to the list and keep looking in the same direction.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // If there is a black piece in the square, we add it to the list and look no further in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

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

        int loops = 0;

        // Up-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a black piece in the square, we add it to the list and look no further in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }

            // If the square is empty, we add it to the list and keep looking in the same direction.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // If there is a white piece in the square, we add it to the list and look no further in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }
        }

        loops = 0;

        // Down-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a black piece in the square, we add it to the list and look no further in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }

            // If the square is empty, we add it to the list and keep looking in the same direction.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // If there is a white piece in the square, we add it to the list and look no further in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }
        }

        loops = 0;

        // Up-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a black piece in the square, we add it to the list and look no further in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }

            // If the square is empty, we add it to the list and keep looking in the same direction.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // If there is a white piece in the square, we add it to the list and look no further in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }
        }

        loops = 0;

        // Down-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a black piece in the square, we add it to the list and look no further in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }

            // If the square is empty, we add it to the list and keep looking in the same direction.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // If there is a white piece in the square, we add it to the list and look no further in this direction.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

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

        int loops = 0;

        // Up-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a white piece, we do not keep looking in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // If the square is empty, we add it to the list and keep looking in this direction.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // If the opposing king is in the square, we add the position of our piece to the list.

            else
            {
                if (Chess.BlackKingPosition == new Vector2(i, position.y + loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        loops = 0;
        tempList.Clear();

        // Down-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a white piece, we do not keep looking in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // If the square is empty, we add it to the list and keep looking in this direction.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // If the opposing king is in the square, we add the position of our piece to the list.

            else
            {
                if (Chess.BlackKingPosition == new Vector2(i, position.y - loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        loops = 0;
        tempList.Clear();

        // Up-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a white piece, we do not keep looking in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // If the square is empty, we add it to the list and keep looking in this direction.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // If the opposing king is in the square, we add the position of our piece to the list.

            else
            {
                if (Chess.BlackKingPosition == new Vector2(i, position.y + loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        loops = 0;
        tempList.Clear();

        // Down-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a white piece, we do not keep looking in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // If the square is empty, we add it to the list and keep looking in this direction.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // If the opposing king is in the square, we add the position of our piece to the list.

            else
            {
                if (Chess.BlackKingPosition == new Vector2(i, position.y - loops))
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

        int loops = 0;

        // Up-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a black piece, we do not keep looking in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // If the square is empty, we add it to the list and keep looking in this direction.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // If the opposing king is in the square, we add the position of our piece to the list.

            else
            {
                if (Chess.WhiteKingPosition == new Vector2(i, position.y + loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        tempList.Clear();
        loops = 0;

        // Down-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a black piece, we do not keep looking in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // If the square is empty, we add it to the list and keep looking in this direction.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // If the opposing king is in the square, we add the position of our piece to the list.

            else
            {
                if (Chess.WhiteKingPosition == new Vector2(i, position.y - loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        tempList.Clear();
        loops = 0;

        // Up-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a black piece, we do not keep looking in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // If the square is empty, we add it to the list and keep looking in this direction.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // If the opposing king is in the square, we add the position of our piece to the list.

            else
            {
                if (Chess.WhiteKingPosition == new Vector2(i, position.y + loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        tempList.Clear();
        loops = 0;

        // Down-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a black piece, we do not keep looking in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // If the square is empty, we add it to the list and keep looking in this direction.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // If the opposing king is in the square, we add the position of our piece to the list.

            else
            {
                if (Chess.WhiteKingPosition == new Vector2(i, position.y - loops))
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
        int loops = 0;

        // Up-right diadonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a white piece in the square, we look no further in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                break;
            }

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                // If there is a black piece and it is not the king, we keep looking in this direction. We can search a maximum of two pieces.
                // It is necessary to block the movements of the king or the other piece to prevent them from causing a check.

                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceBlackInPosition(new Vector2(i, position.y + loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopRight);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y + loops) == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopLeft);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomRight);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        loops = 0;
        firstJump = false;
        selectedPiece = null;

        // Down-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a white piece in the square, we look no further in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // If there is a black piece and it is not the king, we keep looking in this direction. We can search a maximum of two pieces.
            // It is necessary to block the movements of the king or the other piece to prevent them from causing a check.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceBlackInPosition(new Vector2(i, position.y - loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomRight);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y - loops) == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopRight);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomLeft);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        loops = 0;
        firstJump = false;
        selectedPiece = null;

        // Up-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a white piece in the square, we look no further in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // If there is a black piece and it is not the king, we keep looking in this direction. We can search a maximum of two pieces.
            // It is necessary to block the movements of the king or the other piece to prevent them from causing a check.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceBlackInPosition(new Vector2(i, position.y + loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopLeft);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y + loops) == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopRight);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomLeft);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        loops = 0;
        firstJump = false;
        selectedPiece = null;

        // Down-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a white piece in the square, we look no further in this direction.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // If there is a black piece and it is not the king, we keep looking in this direction. We can search a maximum of two pieces.
            // It is necessary to block the movements of the king or the other piece to prevent them from causing a check.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceBlackInPosition(new Vector2(i, position.y - loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomLeft);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y - loops) == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopLeft);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomRight);
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
        int loops = 0;

        // Up-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a black piece in the square, we look no further in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // If there is a black piece and it is not the king, we keep looking in this direction. We can search a maximum of two pieces.
            // It is necessary to block the movements of the king or the other piece to prevent them from causing a check.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceWhiteInPosition(new Vector2(i, position.y + loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopRight);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y + loops) == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopLeft);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomRight);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        loops = 0;
        firstJump = false;
        selectedPiece = null;

        // Down-right diagonal.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // If there is a black piece in the square, we look no further in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // If there is a black piece and it is not the king, we keep looking in this direction. We can search a maximum of two pieces.
            // It is necessary to block the movements of the king or the other piece to prevent them from causing a check.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceWhiteInPosition(new Vector2(i, position.y - loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomRight);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y - loops) == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopRight);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomLeft);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        loops = 0;
        firstJump = false;
        selectedPiece = null;

        // Up-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a black piece in the square, we look no further in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // If there is a black piece and it is not the king, we keep looking in this direction. We can search a maximum of two pieces.
            // It is necessary to block the movements of the king or the other piece to prevent them from causing a check.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceWhiteInPosition(new Vector2(i, position.y + loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopLeft);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y + loops) == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopRight);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomLeft);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        loops = 0;
        firstJump = false;
        selectedPiece = null;

        // Down-left diagonal.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // If there is a black piece in the square, we look no further in this direction.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // If there is a black piece and it is not the king, we keep looking in this direction. We can search a maximum of two pieces.
            // It is necessary to block the movements of the king or the other piece to prevent them from causing a check.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceWhiteInPosition(new Vector2(i, position.y - loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomLeft);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y - loops) == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopLeft);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomRight);
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
            { 20, 10, 10, 10, 10, 10, 10, 20 },
            { 10, -05, 00, 00, 00, 00, -05, 10 },
            { 10, -10, -10, -10, -10, -10, -10, 10 },
            { 10, 00, -10, -10, -10, -10, 00, 10 },
            { 10, -05, -05, -10, -10, -05, -05, 10 },
            { 10, 00, -05, -10, -10, -05, 00, 10 },
            { 10, 00, 00, 00, 00, 00, 00, 10 },
            { 20, 10, 10, 10, 10, 10, 10, 20 }
        };

        return -30 + whiteValue[(int)position.y - 1, (int)position.x - 1];
    }

    /// <summary>
    /// The value of the black piece at the current position.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueBlack()
    {
        int[,] blackValue =
        {
            { -20, -10, -10, -10, -10, -10, -10, -20 },
            { -10, 00, 00, 00, 00, 00, 00, -10 },
            { -10, 00, 05, 10, 10, 05, 00, -10 },
            { -10, 05, 05, 10, 10, 05, 05, -10 },
            { -10, 00, 10, 10, 10, 10, 00, -10 },
            { -10, 10, 10, 10, 10, 10, 10, -10 },
            { -10, 05, 00, 00, 00, 00, 05, -10 },
            { -20, -10, -10, -10, -10, -10, -10, -20 }
        };

        return 30 + blackValue[(int)position.y - 1, (int)position.x - 1];
    }
}