/// <summary>
/// A collection of public enumerations for use in outer classes.
/// </summary>
public static class Enums
{
    /// <summary>
    /// The color of the player who can move the pieces on this device.
    /// </summary>
    public enum Colours {
        /// <summary>
        /// The player plays with the black pieces.
        /// </summary>
        Black,
        /// <summary>
        /// The player plays with the white pieces.
        /// </summary>
        White,
        /// <summary>
        /// All pieces play on this device (local multiplayer).
        /// </summary>
        All
    };

    /// <summary>
    /// The pieces that can be promoted when a pawn reaches the end of the board.
    /// </summary>
    public enum PromotablePieces {
        /// <summary>
        /// The pawn promotes in a rook.
        /// </summary>
        Rook,
        /// <summary>
        /// The pawn promotes in a knight.
        /// </summary>
        Knight,
        /// <summary>
        /// The pawn promotes in a bishop.
        /// </summary>
        Bishop,
        /// <summary>
        /// The pawn promotes in a queen.
        /// </summary>
        Queen
    }

    /// <summary>
    /// The different possibilities for which the game can end in a draw.
    /// </summary>
    public enum DrawModes {
        /// <summary>
        /// The game ends in a draw due to a stalemate.
        /// </summary>
        Stalemate,
        /// <summary>
        /// The game ends in a draw because it is impossible to checkmate with the current pieces on the board.
        /// </summary>
        Impossibility,
        /// <summary>
        /// The game ends in a draw because 75 moves have been made without pawn movements or piece captures.
        /// </summary>
        Move75,
        /// <summary>
        /// The game ends in a draw because the same position on the board has been repeated three times.
        /// </summary>
        ThreefoldRepetition
    }
}