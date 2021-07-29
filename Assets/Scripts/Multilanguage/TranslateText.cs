using UnityEngine;

/// <summary>
/// Contains the different translations of a text.
/// These are read through the MultiText class to manage the translations on the screen.
/// </summary>
[CreateAssetMenu]
public class TranslateText : ScriptableObject
{
    [TextArea(5, 10)] [SerializeField] string english = null;
    [TextArea(5, 10)] [SerializeField] string spanish = null;
    [TextArea(5, 10)] [SerializeField] string catalan = null;
    [TextArea(5, 10)] [SerializeField] string italian = null;

    /// <summary>
    /// Choose the correct version of the text for the selected language.
    /// </summary>
    /// <param name="language">The language in which we want the text.</param>
    /// <returns>The text in the selected language.</returns>
    public string GetText(Options.Language language)
    {
        switch (language)
        {
            case Options.Language.EN:
                return english;
            case Options.Language.ES:
                return spanish;
            case Options.Language.CA:
                return catalan;
            case Options.Language.IT:
                return italian;
            default:
                return english;
        }
    }
}