/// <summary>
/// It contains the settings data that can be saved in a binary file.
/// </summary>
[System.Serializable]
public class SettingsData
{
    /// <summary>
    /// The resolution in which the application is displayed.
    /// </summary>
    public Options.Resolution resolution;

    /// <summary>
    /// The language in which the game interface is displayed.
    /// </summary>
    public Options.Language language;

    /// <summary>
    /// The Photon server to which the game will connect for online games.
    /// </summary>
    public Options.Server server;
}