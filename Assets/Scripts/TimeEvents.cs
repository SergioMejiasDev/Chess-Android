using System.Collections;
using UnityEngine;

/// <summary>
/// Activate all temporary events through coroutines. Static class that inherits from Monobehaviour.
/// </summary>
public class TimeEvents : MonoBehaviour
{
    /// <summary>
    /// Singleton of the class.
    /// </summary>
    public static TimeEvents timeEvents;

    void Awake()
    {
        timeEvents = this;
    }

    /// <summary>
    /// We introduce a three second wait before the AI moves its piece. From here the coroutine is called from another class that does not inherit from Monobehaviour.
    /// </summary>
    public void StartWaitForAI()
    {
        StartCoroutine(WaitForAI());
    }

    /// <summary>
    /// Coroutine that starts the AI movement after 0.25 seconds.
    /// </summary>
    /// <returns>The method "MovePieceAI" from GameManager is called after 0.25 seconds.</returns>
    IEnumerator WaitForAI()
    {
        yield return new WaitForSeconds(0.25f);

        Chess.MoveAIPiece();
        Interface.interfaceClass.EnableButtonPause(true);
    }
}