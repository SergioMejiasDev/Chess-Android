using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It contains the different variables that the different types of pieces can contain.
/// </summary>
[CreateAssetMenu]
public class Pieces : ScriptableObject
{
    /// <summary>
    /// The possible colors that a piece can have.
    /// </summary>
    public enum Colour {
        /// <summary>
        /// Black piece.
        /// </summary>
        Black,
        /// <summary>
        /// White piece.
        /// </summary>
        White
    };

    /// <summary>
    /// The different types of pieces on the board.
    /// </summary>
    public enum Piece {
        /// <summary>
        /// A bishop piece.
        /// </summary>
        Bishop,
        /// <summary>
        /// The king piece.
        /// </summary>
        King,
        /// <summary>
        /// A knight piece.
        /// </summary>
        Knight,
        /// <summary>
        /// A pawn piece.
        /// </summary>
        Pawn,
        /// <summary>
        /// The queen piece.
        /// </summary>
        Queen,
        /// <summary>
        /// A rook piece.
        /// </summary>
        Rook};

    /// <summary>
    /// The different axes in which a piece can move.
    /// </summary>
    public enum Directions {
        /// <summary>
        /// Upward movements.
        /// </summary>
        Top,
        /// <summary>
        /// Movements to the right.
        /// </summary>
        Right,
        /// <summary>
        /// Downward movements.
        /// </summary>
        Bottom,
        /// <summary>
        /// Movements to the left.
        /// </summary>
        Left,
        /// <summary>
        /// Movements in the up-right diagonal.
        /// </summary>
        TopRight,
        /// <summary>
        /// Movements in the up-left diagonal.
        /// </summary>
        TopLeft,
        /// <summary>
        /// Movements in the lower-right diagonal.
        /// </summary>
        BottomRight,
        /// <summary>
        /// Movements in the lower-left diagonal.
        /// </summary>
        BottomLeft
    };

    /// <summary>
    /// The colour of the piece.
    /// </summary>
    [SerializeField] Colour colour;

    /// <summary>
    /// The type of piece.
    /// </summary>
    [SerializeField] Piece piece;

    /// <summary>
    /// The color of the piece.
    /// </summary>
    /// <returns></returns>
    public Colour GetColour()
    {
        return colour;
    }

    /// <summary>
    /// The type of piece.
    /// </summary>
    /// <returns></returns>
    public Piece GetPiece()
    {
        return piece;
    }

    /// <summary>
    /// Gets the list of legal moves that the piece can make from its current position.
    /// </summary>
    /// <param name="position">The position of the piece.</param>
    /// <param name="firstMove">If the piece has been previously moved.</param>
    /// <returns></returns>
    public List<Vector2> GetMovePositions(Vector2 position, bool firstMove)
    {
        if (piece == Piece.Pawn)
        {
            Pawn constructor = new Pawn(position, firstMove, colour);

            return constructor.MovePositions;
        }

        else if (piece == Piece.Rook)
        {
            Rook constructor = new Rook(position, colour);

            return constructor.MovePositions;
        }

        else if (piece == Piece.Bishop)
        {
            Bishop constructor = new Bishop(position, colour);

            return constructor.MovePositions;
        }

        else if (piece == Piece.Queen)
        {
            Queen constructor = new Queen(position, colour);

            return constructor.MovePositions;
        }

        else if (piece == Piece.Knight)
        {
            Knight constructor = new Knight(position, colour);

            return constructor.MovePositions;
        }

        else
        {
            King constructor = new King(position, firstMove, colour);

            return constructor.MovePositions;
        }
    }

    /// <summary>
    /// Calculates the list of positions where the piece can catch from its current position.
    /// </summary>
    /// <param name="position">The position of the piece.</param>
    /// <param name="firstMove">If the piece has been previously moved.</param>
    /// <returns></returns>
    public List<Vector2> GetPositionsInCheck(Vector2 position, bool firstMove)
    {
        if (piece == Piece.Pawn)
        {
            Pawn constructor = new Pawn(position, firstMove, colour);

            return constructor.PositionsInCheck;
        }

        else if (piece == Piece.Rook)
        {
            Rook constructor = new Rook(position, colour);

            return constructor.PositionsInCheck;
        }

        else if (piece == Piece.Bishop)
        {
            Bishop constructor = new Bishop(position, colour);

            return constructor.PositionsInCheck;
        }

        else if (piece == Piece.Queen)
        {
            Queen constructor = new Queen(position, colour);

            return constructor.PositionsInCheck;
        }

        else if (piece == Piece.Knight)
        {
            Knight constructor = new Knight(position, colour);

            return constructor.PositionsInCheck;
        }

        else
        {
            King constructor = new King(position, firstMove, colour);

            return constructor.PositionsInCheck;
        }
    }

    /// <summary>
    /// Calculate a list of threatening positions in case of check.
    /// It is used primarily to block movements that produce a check and allow those that can nullify the check.
    /// </summary>
    /// <param name="position">The position of the piece.</param>
    /// <param name="firstMove">If the piece has been previously moved.</param>
    /// <returns></returns>
    public List<Vector2> GetMenacingPositions(Vector2 position, bool firstMove)
    {
        if (piece == Piece.Pawn)
        {
            Pawn constructor = new Pawn(position, firstMove, colour);

            return constructor.MenacingPositions;
        }

        else if (piece == Piece.Rook)
        {
            Rook constructor = new Rook(position, colour);

            return constructor.MenacingPositions;
        }

        else if (piece == Piece.Bishop)
        {
            Bishop constructor = new Bishop(position, colour);

            return constructor.MenacingPositions;
        }

        else if (piece == Piece.Queen)
        {
            Queen constructor = new Queen(position, colour);

            return constructor.MenacingPositions;
        }

        else if (piece == Piece.Knight)
        {
            Knight constructor = new Knight(position, colour);

            return constructor.MenacingPositions;
        }

        else
        {
            King constructor = new King(position, firstMove, colour);

            return constructor.MenacingPositions;
        }
    }

    /// <summary>
    /// It blocks all the movements that can cause a situation of check for the own player.
    /// </summary>
    /// <param name="position">The position of the piece.</param>
    public void ActivateForbiddenPosition(Vector2 position)
    {
        if (piece == Piece.Pawn)
        {
            return;
        }

        else if (piece == Piece.Rook)
        {
            Rook constructor = new Rook(position, colour);

            constructor.ActivateForbiddenPositions();
        }

        else if (piece == Piece.Bishop)
        {
            Bishop constructor = new Bishop(position, colour);

            constructor.ActivateForbiddenPositions();
        }

        else if (piece == Piece.Queen)
        {
            Queen constructor = new Queen(position, colour);

            constructor.ActivateForbiddenPositions();
        }

        else if (piece == Piece.Knight)
        {
            return;
        }

        else
        {
            return;
        }
    }

    /// <summary>
    /// Finds the relative value of the piece in its current position. This value is used by the AI to choose its best move.
    /// </summary>
    /// <param name="position">The position of the piece.</param>
    /// <param name="firstMove">If the piece has been previously moved.</param>
    /// <returns></returns>
    public int GetValue(Vector2 position, bool firstMove)
    {
        if (piece == Piece.Pawn)
        {
            Pawn constructor = new Pawn(position, firstMove, colour);

            return constructor.PositionValue;
        }

        else if (piece == Piece.Rook)
        {
            Rook constructor = new Rook(position, colour);

            return constructor.PositionValue;
        }

        else if (piece == Piece.Bishop)
        {
            Bishop constructor = new Bishop(position, colour);

            return constructor.PositionValue;
        }

        else if (piece == Piece.Queen)
        {
            Queen constructor = new Queen(position, colour);

            return constructor.PositionValue;
        }

        else if (piece == Piece.Knight)
        {
            Knight constructor = new Knight(position, colour);

            return constructor.PositionValue;
        }

        else
        {
            King constructor = new King(position, firstMove, colour);

            return constructor.PositionValue;
        }
    }
}