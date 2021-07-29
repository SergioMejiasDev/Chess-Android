using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Set of methods responsible for the functions of saving or loading the game.
/// </summary>
public static class SaveManager
{
    #region Game Data

    /// <summary>
    /// Save the game data to a binary file.
    /// </summary>
    /// <param name="saveSlot">The save slot where we want to save the game (1, 2, 3; 0 is reserved for automatic saving).</param>
    /// <param name="dataRaw">The raw data of the game that we want to save.</param>
    public static void SaveGame(int saveSlot, SaveDataRaw dataRaw)
    {
        // We create a serializable file from the raw data.

        SaveData data = new SaveData(dataRaw);

        // We serialize the data and create a binary file in the directory indicated in "path".

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/AutoSave.sav";

        switch (saveSlot)
        {
            case 1:
                path = Application.persistentDataPath + "/Save1.sav";
                break;
            case 2:
                path = Application.persistentDataPath + "/Save2.sav";
                break;
            case 3:
                path = Application.persistentDataPath + "/Save3.sav";
                break;
        }

        FileStream fileStream = new FileStream(path, FileMode.Create);

        formatter.Serialize(fileStream, data);

        fileStream.Close();

        // We update the load-save menus with the date and time data of the new save game.

        Interface.interfaceClass.UpdateSaveDates();
    }

    /// <summary>
    /// Delete the save game in the autosave slot from the device.
    /// </summary>
    public static void DeleteAutoSave()
    {
        string path = Application.persistentDataPath + "/AutoSave.sav";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// We load the data from a binary file on the device.
    /// </summary>
    /// <param name="saveSlot">The save slot where we want to save the game (1, 2, 3; 0 is reserved for automatic saving).</param>
    /// <returns>A SaveData class with all the saved data of the game.</returns>
    public static SaveData LoadGame(int saveSlot)
    {
        // According to the chosen save slot, we save the file location in the variable "path".

        string path = Application.persistentDataPath + "/AutoSave.sav";

        switch (saveSlot)
        {
            case 1:
                path = Application.persistentDataPath + "/Save1.sav";
                break;
            case 2:
                path = Application.persistentDataPath + "/Save2.sav";
                break;
            case 3:
                path = Application.persistentDataPath + "/Save3.sav";
                break;
        }

        // If a file exists at that location, we convert it to a readable file to extract the data.

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }

        return null;
    }

    #endregion

    #region Settings

    /// <summary>
    /// Save the game settings to a binary file.
    /// </summary>
    /// <param name="data">Class with the data we want to save.</param>
    public static void SaveSettings(SettingsData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Settings.sav";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        formatter.Serialize(fileStream, data);

        fileStream.Close();
    }

    /// <summary>
    /// Load previously saved game settings.
    /// </summary>
    /// <returns>Class with the game configuration data.</returns>
    public static SettingsData LoadSettings()
    {
        SettingsData data;

        string path = Application.persistentDataPath + "/Settings.sav";

        // If there is a saved file, we load it.

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();
        }

        // If the file doesn't exist, we create one using the default values.

        else
        {
            Options.DefaultValues();

            return LoadSettings();
        }

        return data;
    }

    /// <summary>
    /// We obtain the data with the information of the date and time of saving.
    /// </summary>
    /// <returns>Array with the four strings with the information of the date and time of saving.</returns>
    public static string[] GetDates()
    {
        string[] dates = new string[4];

        SaveData data;

        string path = Application.persistentDataPath + "/AutoSave.sav";

        // If the saved file exists, we record the data in the array.

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            dates[0] = data.saveDate;
        }

        // If there is no data saved in a slot, we save the value "0" as distinctive data.

        else
        {
            dates[0] = "0";
        }

        // We repeat with all the save slots.

        path = Application.persistentDataPath + "/Save1.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            dates[1] = data.saveDate;
        }

        else
        {
            dates[1] = "0";
        }

        path = Application.persistentDataPath + "/Save2.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            dates[2] = data.saveDate;
        }

        else
        {
            dates[2] = "0";
        }

        path = Application.persistentDataPath + "/Save3.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            dates[3] = data.saveDate;
        }

        else
        {
            dates[3] = "0";
        }

        return dates;
    }

    #endregion

    #region Photon Serialization

    /// <summary>
    /// Serialize the data so that it can be transferred through Photon's servers.
    /// </summary>
    /// <param name="data">Serializes the data as a byte array so that it can be transferred through Photon's servers.</param>
    /// <returns>The data in the form of an array of bytes.</returns>
    public static byte[] Serialize(object data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();

        formatter.Serialize(stream, data);

        return stream.ToArray();
    }

    /// <summary>
    /// Deserializes the data as a byte array to make it human-readable data.
    /// </summary>
    /// <param name="byteData">The data as a byte array</param>
    /// <returns></returns>
    public static SaveData Deserialize(byte[] byteData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream(byteData);

        return formatter.Deserialize(stream) as SaveData;
    }

    #endregion
}