using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// It allows to select the different squares of the game.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BoardSquare : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// Indicates if the box is selected at this time.
    /// </summary>
    bool isSelected = false;

    /// <summary>
    /// Indicates if the box can be selected at this time.
    /// </summary>
    bool selectable = true;

    /// <summary>
    /// Lock the box so that it cannot be selected (end of game or main menu).
    /// </summary>
    bool locked = true;

    /// <summary>
    /// The starting color of the box (white or black).
    /// </summary>
    Color initialColour;

    /// <summary>
    /// The Sprite Renderer linked to the GameObject.
    /// </summary>
    SpriteRenderer sr = null;

    private void Awake()
    {
        // We initialize the Sprite Renderer and save the initial colour of the box so as not to lose it when making changes.

        sr = GetComponent<SpriteRenderer>();
        initialColour = sr.color;
    }

    private void OnEnable()
    {
        // We subscribe to the different delegates that will allow us to make changes in all the squares from the Game Manager.

        Chess.UpdateColour += UpdateColour;
        Chess.OriginalColour += ResetColour;
        Chess.EnableSelection += UnlockSquare;
        Chess.DisableSelection += LockSquare;
    }

    /// <summary>
    /// It is activated when we click on a square.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // If the square is locked or it is not the player's turn, we prevent it from being selected.

        if (locked || !Chess.CheckTurn())
        {
            return;
        }

        // If it can be selected and there is a piece on it, we select it.

        if (selectable && Chess.SelectPiece(transform.position))
        {
            selectable = false;
            isSelected = true;
        }

        // If the square was selected when clicking, we deselect it.

        else if (isSelected)
        {
            Chess.DeselectPosition();
        }

        // If the square is green (the selected piece can move to this direction) we carry out the movement.

        else if (sr.color == Color.green)
        {
            // If we are not playing an online game, we move the selected piece to this square.

            if (!NetworkManager.manager.IsConnected)
            {
                Chess.MovePiece(transform.position);
            }

            // If the game is online, we do the same but sending the data to the server.

            else
            {
                NetworkManager.manager.MovePiece(Chess.ActivePiecePosition, transform.position);
            }
        }
    }

    /// <summary>
    /// Lock the square so that it cannot be selected.
    /// </summary>
    void LockSquare()
    {
        locked = true;
    }

    /// <summary>
    /// Unlock the square so that it can be selected.
    /// </summary>
    void UnlockSquare()
    {
        locked = false;
    }

    /// <summary>
    /// The square returns to its original colour, allowing its selection if the conditions are met.
    /// </summary>
    void ResetColour()
    {
        sr.color = initialColour;
        isSelected = false;
        selectable = true;
    }

    /// <summary>
    /// Updates the colour of the square based on the received parameters.
    /// </summary>
    /// <param name="piecePosition">Position that has been selected (there is a piece on it).</param>
    /// <param name="greenPositions">Position to which the selected piece can be moved.</param>
    void UpdateColour(Vector2 piecePosition, List<Vector2> greenPositions)
    {
        // If the box is selected (we want to move the piece over it), it turns yellow.

        if (piecePosition == (Vector2)transform.position)
        {
            sr.color = Color.yellow;

            return;
        }

        // If the player is allowed to move the selected piece to this square, it turns green.

        for (int i = 0; i < greenPositions.Count; i++)
        {
            if (greenPositions[i] == (Vector2)transform.position)
            {
                sr.color = Color.green;

                return;
            }
        }
    }
}