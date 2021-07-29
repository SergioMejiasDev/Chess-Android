using UnityEngine;

/// <summary>
/// It contains the different settings that can be modified within the application.
/// </summary>
public static class Options
{
    /// <summary>
    /// List of the different resolutions in which the application can be used.
    /// </summary>
    public enum Resolution {
        /// <summary>
        /// Full screen, corrected resolution of 16:9.
        /// </summary>
        Fullscreen,
        /// <summary>
        /// Window mode, with a resolution of 1280x720.
        /// </summary>
        Windowed720,
        /// <summary>
        /// Window mode, with a resolution of 854x480.
        /// </summary>
        Windowed480
    }

    /// <summary>
    /// List of languages available within the application.
    /// </summary>
    public enum Language { 
        /// <summary>
        /// English.
        /// </summary>
        EN,
        /// <summary>
        /// Spanish.
        /// </summary>
        ES,
        /// <summary>
        /// Catalan.
        /// </summary>
        CA,
        /// <summary>
        /// Italian.
        /// </summary>
        IT };

    /// <summary>
    /// List of servers offered by Photon for online games.
    /// </summary>
    public enum Server { 
        /// <summary>
        /// Asia (Singapore).
        /// </summary>
        Asia,
        /// <summary>
        /// Australia (Melbourne).
        /// </summary>
        Australia,
        /// <summary>
        /// Canada, East (Montreal).
        /// </summary>
        CanadaEast,
        /// <summary>
        /// Europe (Amsterdam).
        /// </summary>
        Europe,
        /// <summary>
        /// India (Chennai).
        /// </summary>
        India,
        /// <summary>
        /// Japan (Tokyo).
        /// </summary>
        Japan,
        /// <summary>
        /// Russia, East (Khabarovsk).
        /// </summary>
        RussiaEast,
        /// <summary>
        /// Russia, West (Moscow).
        /// </summary>
        RussiaWest,
        /// <summary>
        /// South Africa (Johannesburg).
        /// </summary>
        SouthAfrica,
        /// <summary>
        /// South America (Sao Paulo).
        /// </summary>
        SouthAmerica,
        /// <summary>
        /// South Korea (Seoul).
        /// </summary>
        SouthKorea,
        /// <summary>
        /// USA, East (Washington D.C.).
        /// </summary>
        USAEast,
        /// <summary>
        /// Usa, West (San José).
        /// </summary>
        USAWest }

    #region Properties

    /// <summary>
    /// The active resolution at the moment.
    /// </summary>
    public static Resolution ActiveResolution { get; set; }

    /// <summary>
    /// The active language at the moment.
    /// </summary>
    public static Language ActiveLanguage { get; set; }

    /// <summary>
    /// The currently selected Photon server.
    /// </summary>
    public static Server ActiveServer { get; set; }

    #endregion

    /// <summary>
    /// Saves the settings data in a binary file.
    /// </summary>
    public static void SaveOptions()
    {
        SettingsData data = new SettingsData
        {
            resolution = ActiveResolution,
            language = ActiveLanguage,
            server = ActiveServer
        };

        SaveManager.SaveSettings(data);
    }

    /// <summary>
    /// Loads the settings data from a previously created binary file.
    /// </summary>
    public static void LoadOptions()
    {
        SettingsData data = SaveManager.LoadSettings();

        ActiveResolution = data.resolution;
        ActiveLanguage = data.language;
        ActiveServer = data.server;
    }

    /// <summary>
    /// Create a settings file with the default data.
    /// </summary>
    public static void DefaultValues()
    {
        // The default resolution is an 854x480 window.

        ActiveResolution = Resolution.Windowed480;

        // Depending on the language of the device, the same language is set by default within those existing in the application.

        if (Application.systemLanguage == SystemLanguage.Spanish ||
            Application.systemLanguage == SystemLanguage.Basque)
        {
            ActiveLanguage = Language.ES;
        }

        else if (Application.systemLanguage == SystemLanguage.Catalan)
        {
            ActiveLanguage = Language.CA;
        }

        else if (Application.systemLanguage == SystemLanguage.Italian)
        {
            ActiveLanguage = Language.IT;
        }

        // If the device language is not among the above, English is used as the default language.

        else
        {
            ActiveLanguage = Language.EN;
        }

        // By default, the server in Europe is chosen.

        ActiveServer = Server.Europe;

        // We save the data.

        SaveOptions();
    }
}