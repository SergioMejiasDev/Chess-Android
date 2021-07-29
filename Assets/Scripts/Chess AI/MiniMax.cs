using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It contains the functions related to the MiniMax algorithm used by the AI.
/// </summary>
public static class MiniMax
{
    /// <summary>
    /// Calculate the most suitable move for the white pieces following the MiniMax algorithm.
    /// </summary>
    /// <returns>The most optimal piece and move for the white player.</returns>
    public static AIMovePosition BestMovementWhite()
    {
        int value = 0;
        AIMovePosition selectedMove = null;

        for (int i = 0; i < Chess.PiecesWhite.Count; i++)
        {
            // For all the white pieces we calculate all possible legal moves.

            List<Vector2> greenPositions = Chess.PiecesWhite[i].GetComponent<PiecesMovement>().SearchGreenPositions();

            // If there are no possible moves for this piece, we move on to the next one.

            if (greenPositions.Count == 0)
            {
                continue;
            }

            for (int j = 0; j < greenPositions.Count; j++)
            {
                // We temporarily save the piece variables (position and if it has been moved) to later retrieve them.

                Vector2 startPosition = Chess.PiecesWhite[i].transform.position;
                bool hasMoved = Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove;

                // We move the piece to one of the possible positions.

                Chess.PiecesWhite[i].transform.position = greenPositions[j];
                Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove = true;

                int currentValue = BoardValueWhite(greenPositions[j]);

                if (currentValue > value && value != 0)
                {
                    Chess.PiecesWhite[i].transform.position = startPosition;
                    Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;

                    continue;
                }

                // From this new position, we get the best value for the black pieces.

                int valueTemp = BestValueBlack(4, currentValue);

                // White's pieces seek to minimize the value, and Black to maximize it,
                // so we are going to choose the lowest value that Black can get.

                // If the list of possible moves is empty, we add this one.
                // Also, if the value obtained is the same as the one we already had, we also add the movement to the list.

                if (selectedMove == null || valueTemp <= value)
                {
                    value = valueTemp;
                    selectedMove = new AIMovePosition(Chess.PiecesWhite[i], greenPositions[j]);
                }

                // We return the piece to its original position.

                Chess.PiecesWhite[i].transform.position = startPosition;
                Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;

                if (value == -100000)
                {
                    return selectedMove;
                }
            }
        }

        // We are left with a random movement of the ones we have on the list.

        return selectedMove;
    }

    /// <summary>
    /// Calculate the most suitable move for the black pieces following the MiniMax algorithm.
    /// </summary>
    /// <returns>The most optimal piece and move for the black player.</returns>
    public static AIMovePosition BestMovementBlack()
    {
        int value = 0;
        AIMovePosition selectedMove = null;

        for (int i = 0; i < Chess.PiecesBlack.Count; i++)
        {
            // For all the white pieces we calculate all possible legal moves.

            List<Vector2> greenPositions = Chess.PiecesBlack[i].GetComponent<PiecesMovement>().SearchGreenPositions();

            // If there are no possible moves for this piece, we move on to the next one.

            if (greenPositions.Count == 0)
            {
                continue;
            }

            for (int j = 0; j < greenPositions.Count; j++)
            {
                // We temporarily save the piece variables (position and if it has been moved) to later retrieve them.

                Vector2 startPosition = Chess.PiecesBlack[i].transform.position;
                bool hasMoved = Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove;

                // We move the piece to one of the possible positions.

                Chess.PiecesBlack[i].transform.position = greenPositions[j];
                Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove = true;

                int currentValue = BoardValueBlack(greenPositions[j]);

                if (currentValue < value && value != 0)
                {
                    Chess.PiecesBlack[i].transform.position = startPosition;
                    Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;

                    continue;
                }

                // From this new position, we get the best value for the white pieces.

                int valueTemp = BestValueWhite(4, currentValue);

                // White's pieces seek to minimize the value, and Black to maximize it,
                // so we are going to choose the highest value that White can get.

                // If the list of possible moves is empty, we add this one.
                // Also, if the value obtained is the same as the one we already had, we also add the movement to the list.

                if (selectedMove == null || valueTemp >= value)
                {
                    value = valueTemp;
                    selectedMove = new AIMovePosition(Chess.PiecesBlack[i], greenPositions[j]);
                }

                // We return the piece to its original position.

                Chess.PiecesBlack[i].transform.position = startPosition;
                Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;

                if (value == 100000)
                {
                    return selectedMove;
                }
            }
        }

        // We are left with a random movement of the ones we have on the list.

        return selectedMove;
    }

    /// <summary>
    /// Calculate the best possible value for the white pieces in a specific situation.
    /// </summary>
    /// <param name="depth">The depth to which we want to explore the results of the algorithm.
    /// A greater depth implies better movements, but also a greater consumption of resources.</param>
    /// <returns>The best possible value for the current board.</returns>
    static int BestValueWhite(int depth, int previousValue)
    {
        // The first thing we do is check that there are no black and white pieces in the same square.
        // This would mean that in the previous move this white piece has been captured by a black one.
        // In that case we deactivate the captured white pieces so that they are not detectable.

        List<GameObject> capturedPieces = new List<GameObject>();

        foreach (GameObject piece in Chess.PiecesWhite)
        {
            if (Chess.CheckSquareBlack(piece.transform.position))
            {
                if (Chess.BlackKingPosition == (Vector2)piece.transform.position)
                {
                    foreach (GameObject capturedPiece in capturedPieces)
                    {
                        capturedPiece.SetActive(true);
                    }

                    return -100000;
                }

                capturedPieces.Add(piece);

                piece.SetActive(false);
            }
        }

        // We start by taking an extreme initial value so that any existing result is better than this.

        int value = 100000;

        for (int i = 0; i < Chess.PiecesWhite.Count; i++)
        {
            // For each piece we update the list of positions in check and legal moves.

            Chess.CheckVerification();

            List<Vector2> greenPositions = Chess.PiecesWhite[i].GetComponent<PiecesMovement>().SearchGreenPositions();

            // If the piece cannot make any movement, we skip it and move on to the next one.

            if (greenPositions.Count == 0)
            {
                continue;
            }

            for (int j = 0; j < greenPositions.Count; j++)
            {
                // For each piece, we save the initial values to later retrieve them.

                Vector2 startPosition = Chess.PiecesWhite[i].transform.position;
                bool hasMoved = Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove;

                // We move the piece to its new position and obtain the value of the board.

                Chess.PiecesWhite[i].transform.position = greenPositions[j];
                Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove = true;

                int currentValue = BoardValueWhite(greenPositions[j]);

                if (currentValue > previousValue)
                {
                    Chess.PiecesWhite[i].transform.position = startPosition;
                    Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;

                    continue;
                }

                int valueTemp;

                // If we are at the deepest level of depth (0), this will be the value we return.

                if (depth == 0)
                {
                    valueTemp = currentValue;
                }

                // If we are not at the greatest depth level, we "go down" one level and calculate the same function for the opposite color.

                else
                {
                    depth--;

                    valueTemp = BestValueBlack(depth, currentValue);
                }

                // Since we want to maximize White's value, we want the lowest value.

                if (valueTemp < value)
                {
                    value = valueTemp;
                }

                // After this, we return the pieces to its original values.

                Chess.PiecesWhite[i].transform.position = startPosition;
                Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;
            }
        }

        // We return the pieces to the original values before making the checks.

        foreach (GameObject piece in capturedPieces)
        {
            piece.SetActive(true);
        }

        return value;
    }

    /// <summary>
    /// Calculate the best possible value for the black pieces in a specific situation.
    /// </summary>
    /// <param name="depth">The depth to which we want to explore the results of the algorithm.
    /// A greater depth implies better movements, but also a greater consumption of resources.</param>
    /// <returns>The best possible value for the current board.</returns>
    static int BestValueBlack(int depth, int previousValue)
    {
        // The first thing we do is check that there are no black and white pieces in the same square.
        // This would mean that in the previous move this white piece has been captured by a black one.
        // In that case we deactivate the captured white pieces so that they are not detectable.

        List<GameObject> capturedPieces = new List<GameObject>();

        foreach (GameObject piece in Chess.PiecesBlack)
        {
            if (Chess.CheckSquareWhite(piece.transform.position))
            {
                if (Chess.WhiteKingPosition == (Vector2)piece.transform.position)
                {
                    foreach (GameObject capturedPiece in capturedPieces)
                    {
                        capturedPiece.SetActive(true);
                    }

                    return 100000;
                }

                capturedPieces.Add(piece);

                piece.SetActive(false);
            }
        }

        // We start by taking an extreme initial value so that any existing result is better than this.

        int value = -100000;

        for (int i = 0; i < Chess.PiecesBlack.Count; i++)
        {
            // For each piece we update the list of positions in check and legal moves.

            Chess.CheckVerification();

            List<Vector2> greenPositions = Chess.PiecesBlack[i].GetComponent<PiecesMovement>().SearchGreenPositions();

            // If the piece cannot make any movement, we skip it and move on to the next one.

            if (greenPositions.Count == 0)
            {
                continue;
            }

            for (int j = 0; j < greenPositions.Count; j++)
            {
                // For each piece, we save the initial values to later retrieve them.

                Vector2 startPosition = Chess.PiecesBlack[i].transform.position;
                bool hasMoved = Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove;

                // We move the piece to its new position and obtain the value of the board.

                Chess.PiecesBlack[i].transform.position = greenPositions[j];
                Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove = true;

                int currentValue = BoardValueBlack(greenPositions[j]);

                if (currentValue < previousValue)
                {
                    Chess.PiecesBlack[i].transform.position = startPosition;
                    Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;

                    continue;
                }

                int valueTemp;

                // If we are at the deepest level of depth (0), this will be the value we return.

                if (depth == 0)
                {
                    valueTemp = currentValue;
                }

                // If we are not at the greatest depth level, we "go down" one level and calculate the same function for the opposite color.

                else
                {
                    depth--;

                    valueTemp = BestValueWhite(depth, currentValue);
                }

                // Since we want to maximize Black's value, we want the highest value.

                if (valueTemp > value)
                {
                    value = valueTemp;
                }

                // After this, we return the pieces to its original values.

                Chess.PiecesBlack[i].transform.position = startPosition;
                Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;
            }
        }

        // We return the pieces to the original values before making the checks.

        foreach (GameObject piece in Chess.PiecesBlack)
        {
            piece.SetActive(true);
        }

        return value;
    }

    /// <summary>
    /// Find the current value of the board for the white pieces.
    /// </summary>
    /// <param name="position">The position of the last movement, where we capture the piece of the opposite color if it exists.</param>
    /// <returns>The integer value of the board.</returns>
    static int BoardValueWhite(Vector2 position)
    {
        int value = 0;

        // The first thing we do is eliminate the piece of the opposite color that exists in our square.
        // This means that we have captured it with the previous movement.

        GameObject pieceInPosition = Chess.GetPieceBlackInPosition(position);

        if (pieceInPosition != null)
        {
            pieceInPosition.SetActive(false);
        }

        // The next step is to add the value of each piece on the board for the two colors.

        for (int i = 0; i < Chess.PiecesWhite.Count; i++)
        {
            value += Chess.PiecesWhite[i].GetComponent<PiecesMovement>().Value;
        }

        for (int i = 0; i < Chess.PiecesBlack.Count; i++)
        {
            value += Chess.PiecesBlack[i].GetComponent<PiecesMovement>().Value;
        }

        // Finally, if in the first step we have eliminated any piece, we restore it again.

        if (pieceInPosition != null)
        {
            pieceInPosition.SetActive(true);
        }

        return value;
    }

    /// <summary>
    /// Find the current value of the board for the black pieces.
    /// </summary>
    /// <param name="position">The position of the last movement, where we capture the piece of the opposite color if it exists.</param>
    /// <returns>The integer value of the board.</returns>
    static int BoardValueBlack(Vector2 position)
    {
        int value = 0;

        // The first thing we do is eliminate the piece of the opposite color that exists in our square.
        // This means that we have captured it with the previous movement.

        GameObject pieceInPosition = Chess.GetPieceWhiteInPosition(position);

        if (pieceInPosition != null)
        {
            pieceInPosition.SetActive(false);
        }

        // The next step is to add the value of each piece on the board for the two colors.

        for (int i = 0; i < Chess.PiecesWhite.Count; i++)
        {
            value += Chess.PiecesWhite[i].GetComponent<PiecesMovement>().Value;
        }

        for (int i = 0; i < Chess.PiecesBlack.Count; i++)
        {
            value += Chess.PiecesBlack[i].GetComponent<PiecesMovement>().Value;
        }

        // Finally, if in the first step we have eliminated any piece, we restore it again.

        if (pieceInPosition != null)
        {
            pieceInPosition.SetActive(true);
        }

        return value;
    }
}