using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It contains the constants and methods necessary to allow the movement of the knights.
/// </summary>
public class Knight
{
    /// <summary>
    /// The position of the piece.
    /// </summary>
    readonly Vector2 position;
    /// <summary>
    /// The color of the piece.
    /// </summary>
    readonly Pieces.Colour colour;

    public Knight(Vector2 position, Pieces.Colour colour)
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
    /// The value of the piece at the current position.
    /// </summary>
    public int PositionValue => (colour == Pieces.Colour.White) ? GetPositionValueWhite() : GetPositionValueBlack();

    /// <summary>
    /// List of movements that a white piece can perform. This list of movements has not been filtered yet to eliminate illegal movements.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMovePositionsWhite()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x + 1, position.y + 2),
            new Vector2(position.x + 2, position.y + 1),
            new Vector2(position.x + 2, position.y - 1),
            new Vector2(position.x + 1, position.y - 2),
            new Vector2(position.x - 1, position.y - 2),
            new Vector2(position.x - 2, position.y - 1),
            new Vector2(position.x - 2, position.y + 1),
            new Vector2(position.x - 1, position.y + 2)
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
    /// List of movements that a black piece can perform. This list of movements has not been filtered yet to eliminate illegal movements.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMovePositionsBlack()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x + 1, position.y + 2),
            new Vector2(position.x + 2, position.y + 1),
            new Vector2(position.x + 2, position.y - 1),
            new Vector2(position.x + 1, position.y - 2),
            new Vector2(position.x - 1, position.y - 2),
            new Vector2(position.x - 2, position.y - 1),
            new Vector2(position.x - 2, position.y + 1),
            new Vector2(position.x - 1, position.y + 2)
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
    /// List of positions where the white piece can capture another.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetPositionsInCheckWhite()
    {
        List<Vector2> tempList = GetMovePositionsWhite();

        return tempList;
    }

    /// <summary>
    /// List of positions where the black piece can capture another.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetPositionsInCheckBlack()
    {
        List<Vector2> tempList = GetMovePositionsBlack();

        return tempList;
    }

    /// <summary>
    /// List of positions where the white piece is a threat to the black player.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMenacingPositionsWhite()
    {
        List<Vector2> tempList = GetMovePositionsWhite();

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
        List<Vector2> tempList = GetMovePositionsBlack();

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
            { 50, 40, 30, 30, 30, 30, 40, 50 },
            { 40, 20, 00, -05, -05, 00, 20, 40 },
            { 30, -05, -10, -15, -15, -10, -05, 30 },
            { 30, 00, -15, -20, -20, -15, 00, 30 },
            { 30, -05, -15, -20, -20, -15, -05, 30 },
            { 30, 00, -10, -15, -15, -10, 00, 30 },
            { 40, 20, 00, 00, 00, 00, 20, 40 },
            { 50, 40, 30, 30, -30, 30, 40, 50 }
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
            { -50, -40, -30, -30, -30, -30, -40, -50 },
            { -40, -20, 00, 00, 00, 00, -20, -40 },
            { -30, 00, 10, 15, 15, 10, 00, -30 },
            { -30, 05, 15, 20, 20, 15, 05, -30 },
            { -30, 00, 15, 20, 20, 15, 00, -30 },
            { -30, 05, 10, 15, 15, 10, 05, -30 },
            { -40, -20, 00, 05, 05, 00, -20, -40 },
            { -50, -40, -30, -30, -30, -30, -40, -50 }
        };

        return 30 + blackValue[(int)position.y - 1, (int)position.x - 1];
    }
}