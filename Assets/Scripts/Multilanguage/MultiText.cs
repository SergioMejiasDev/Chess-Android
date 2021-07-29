using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Toggles the language of the texts according to the language selected in the settings.
/// </summary>
[RequireComponent(typeof(Text))]
public class MultiText : MonoBehaviour
{
    /// <summary>
    /// The text that can be translated.
    /// </summary>
    Text text = null;

    /// <summary>
    /// The file that contains the differentiated translations of the text.
    /// </summary>
    [SerializeField] TranslateText textAsset = null;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        // When the text is activated, the corresponding language is activated.

        UpdateText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Updates the language of the text on the screen.
    /// </summary>
    /// <param name="language">The language into which the text will be translated.</param>
    void UpdateText(Options.Language language)
    {
        text.text = textAsset.GetText(language);
    }
}