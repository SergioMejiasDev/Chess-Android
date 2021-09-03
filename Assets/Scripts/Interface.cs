using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains the necessary methods to manage the game interface.
/// </summary>
public class Interface : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Singleton of the class.
    /// </summary>
    public static Interface interfaceClass;

    /// <summary>
    /// Panel that contains all the subpanels of the main menu.
    /// </summary>
    [Header("Main Menu")]
    [SerializeField] GameObject mainMenu = null;
    /// <summary>
    /// All sub-panels of the main menu.
    /// </summary>
    [SerializeField] GameObject[] panelsMenu = null;
    /// <summary>
    /// Message for indicates that there was an error in the connection.
    /// </summary>
    [SerializeField] Text textErrorConnection = null;
    /// <summary>
    /// Text where the server to which we are connected is indicated.
    /// </summary>
    [SerializeField] Text serverText = null;

    /// <summary>
    /// Text with the save date of slot 0.
    /// </summary>
    [Header("Panel Load")]
    [SerializeField] Text textLoad0 = null;
    /// <summary>
    /// Text with the save date of slot 1.
    /// </summary>
    [SerializeField] Text textLoad1 = null;
    /// <summary>
    /// Text with the save date of slot 2.
    /// </summary>
    [SerializeField] Text textLoad2 = null;
    /// <summary>
    /// Text with the save date of slot 3.
    /// </summary>
    [SerializeField] Text textLoad3 = null;
    /// <summary>
    /// Button to load the game in slot 0.
    /// </summary>
    [SerializeField] Button buttonLoad0 = null;
    /// <summary>
    /// Button to load the game in slot 1.
    /// </summary>
    [SerializeField] Button buttonLoad1 = null;
    /// <summary>
    /// Button to load the game in slot 2.
    /// </summary>
    [SerializeField] Button buttonLoad2 = null;
    /// <summary>
    /// Button to load the game in slot 3.
    /// </summary>
    [SerializeField] Button buttonLoad3 = null;

    /// <summary>
    /// Pause button in the corner of the screen shaped like a gear.
    /// </summary>
    [Header("Panel Pause")]
    [SerializeField] GameObject buttonPauseObject = null;
    /// <summary>
    /// Panel containing all the sub-panels of the pause menu.
    /// </summary>
    [SerializeField] GameObject panelPause = null;
    /// <summary>
    /// All sub-panels of the pause menu.
    /// </summary>
    [SerializeField] GameObject[] panelsPause = null;
    /// <summary>
    /// Text with the save date of slot 1.
    /// </summary>
    [SerializeField] Text textSave1 = null;
    /// <summary>
    /// Text with the save date of slot 2.
    /// </summary>
    [SerializeField] Text textSave2 = null;
    /// <summary>
    /// Text with the save date of slot 3.
    /// </summary>
    [SerializeField] Text textSave3 = null;
    /// <summary>
    /// Button to save the game in slot 1.
    /// </summary>
    [SerializeField] Button buttonSave1 = null;
    /// <summary>
    /// Button to save the game in slot 2.
    /// </summary>
    [SerializeField] Button buttonSave2 = null;
    /// <summary>
    /// Button to save the game in slot 3.
    /// </summary>
    [SerializeField] Button buttonSave3 = null;
    /// <summary>
    /// Button to confirm that you want to save game in case of overwriting.
    /// </summary>
    [SerializeField] Button buttonConfirmSave = null;
    /// <summary>
    /// Save button in pause menu.
    /// </summary>
    [SerializeField] GameObject buttonSave = null;
    /// <summary>
    /// Reset button in pause menu.
    /// </summary>
    [SerializeField] GameObject buttonRestart = null;
    /// <summary>
    /// Button to confirm the restart of the game.
    /// </summary>
    [SerializeField] Button buttonConfirmRestart = null;

    /// <summary>
    /// Brown panel displayed on the right of the screen during the game.
    /// </summary>
    [Header("Panel Game")]
    [SerializeField] GameObject panelGame = null;
    /// <summary>
    /// All sub-panels within the brown panel during gameplay.
    /// </summary>
    [SerializeField] GameObject[] panelsGame = null;
    /// <summary>
    /// The button to play again after finishing the game.S
    /// </summary>
    [SerializeField] Button buttonRestartEndGame = null;
    /// <summary>
    /// Button to start playing against the AI using the color white.
    /// </summary>
    [SerializeField] Button buttonColourWhite = null;
    /// <summary>
    /// Button to start playing against the AI using the color black.
    /// </summary>
    [SerializeField] Button buttonColourBlack = null;

    /// <summary>
    /// Panel where the main notifications of the game appear.
    /// </summary>
    [Header("Panel Notifications")]
    [SerializeField] GameObject panelNotifications = null;
    /// <summary>
    /// Text within the notification panel.
    /// </summary>
    [SerializeField] Text notificationsText = null;
    /// <summary>
    /// Color circle that appears inside the notification panel.
    /// </summary>
    [SerializeField] Image notificationsImage = null;

    /// <summary>
    /// Panel to indicate that a player is in check.
    /// </summary>
    [Header("Panel Check")]
    [SerializeField] GameObject panelCheck = null;
    /// <summary>
    /// Text indicating that a player is in check.
    /// </summary>
    [SerializeField] Text checkText = null;

    /// <summary>
    /// Text with the name of the room that indicates that we are waiting for another player to join.
    /// </summary>
    [Header("Panel Waiting Player 2")]
    [SerializeField] Text textWaitingRoomName = null;

    /// <summary>
    /// Text where the name of the room is written to join it.
    /// </summary>
    [Header("Panel Keyboard")]
    [SerializeField] Text textRoomName = null;

    /// <summary>
    /// Text indicating that a pawn has been promoted.
    /// </summary>
    [Header("Panel Promotion")]
    [SerializeField] Text promotionText = null;
    /// <summary>
    /// Panel to choose the white piece in which a pawn promotes.
    /// </summary>
    [SerializeField] GameObject panelPiecesWhite = null;
    /// <summary>
    /// Panel to choose the black piece in which a pawn promotes.
    /// </summary>
    [SerializeField] GameObject panelPiecesBlack = null;

    #endregion

    public void Awake()
    {
        interfaceClass = this;

        Options.LoadOptions();

        LetterBoxer.AddLetterBoxing();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        UpdateSaveDates();
    }

    #region Main Menu

    /// <summary>
    /// Opens the main menu together with the indicated sub-panel.
    /// </summary>
    /// <param name="panel">The sub-panel to open.</param>
    public void OpenPanelMenu(GameObject panel)
    {
        for (int i = 0; i < panelsMenu.Length; i++)
        {
            panelsMenu[i].SetActive(false);
        }

        panel.SetActive(true);
        mainMenu.SetActive(true);
        panelPause.SetActive(false);
        panelGame.SetActive(false);
    }

    /// <summary>
    /// Opens the main menu together with the indicated sub-panel.
    /// </summary>
    /// <param name="panel">The index of the sub-panel to open.</param>
    public void OpenPanelMenu(int panel)
    {
        for (int i = 0; i < panelsMenu.Length; i++)
        {
            panelsMenu[i].SetActive(false);
        }

        panelsMenu[panel].SetActive(true);
        mainMenu.SetActive(true);
        panelPause.SetActive(false);
        panelGame.SetActive(false);
    }

    /// <summary>
    /// Opens the connection error notification, displaying a message to indicate the reason for the error.
    /// </summary>
    /// <param name="cause">The cause of the error that occurred.</param>
    public void OpenErrorPanel(Photon.Realtime.DisconnectCause cause)
    {
        switch (cause)
        {
            case Photon.Realtime.DisconnectCause.MaxCcuReached:
                textErrorConnection.text = ServerFull();
                break;
            case Photon.Realtime.DisconnectCause.DnsExceptionOnConnect:
                textErrorConnection.text = NoInternetConnection();
                break;
            case Photon.Realtime.DisconnectCause.InvalidRegion:
                textErrorConnection.text = InvalidRegion();
                break;
            default:
                textErrorConnection.text = GenericServerError();
                break;
        }

        Chess.CleanScene();

        OpenPanelMenu(3);
    }

    /// <summary>
    /// Start a new game by choosing the white pieces and playing against the AI.
    /// </summary>
    public void NewGameWhite()
    {
        buttonPauseObject.SetActive(true);
        panelCheck.SetActive(false);
        OpenPanelGame(0);

        buttonRestartEndGame.onClick.AddListener(delegate { OpenPanelColours(); });
        buttonConfirmRestart.onClick.AddListener(delegate { OpenPanelColours(); });

        Chess.SelectColor(Enums.Colours.White, null);
    }

    /// <summary>
    /// Start a new game by choosing the black pieces and playing against the AI.
    /// </summary>
    public void NewGameBlack()
    {
        buttonPauseObject.SetActive(true);
        panelCheck.SetActive(false);
        OpenPanelGame(0);

        buttonRestartEndGame.onClick.AddListener(delegate { OpenPanelColours(); });
        buttonConfirmRestart.onClick.AddListener(delegate { OpenPanelColours(); });

        Chess.SelectColor(Enums.Colours.Black, null);
    }

    /// <summary>
    /// Start a loaded game by choosing the white pieces and playing against the AI.
    /// </summary>
    /// <param name="saveSlot">The save slot to be loaded.</param>
    public void LoadGameWhite(int saveSlot)
    {
        buttonPauseObject.SetActive(true);
        panelCheck.SetActive(false);
        OpenPanelGame(0);

        buttonRestartEndGame.onClick.AddListener(delegate { OpenPanelColours(); });
        buttonConfirmRestart.onClick.AddListener(delegate { OpenPanelColours(); });

        Chess.SelectColor(Enums.Colours.White, SaveManager.LoadGame(saveSlot));
    }

    /// <summary>
    /// Start a loaded game by choosing the black pieces and playing against the AI.
    /// </summary>
    /// <param name="saveSlot">The save slot to be loaded.</param>
    public void LoadGameBlack(int saveSlot)
    {
        buttonPauseObject.SetActive(true);
        panelCheck.SetActive(false);
        OpenPanelGame(0);

        buttonRestartEndGame.onClick.AddListener(delegate { OpenPanelColours(); });
        buttonConfirmRestart.onClick.AddListener(delegate { OpenPanelColours(); });

        Chess.SelectColor(Enums.Colours.Black, SaveManager.LoadGame(saveSlot));
    }

    /// <summary>
    /// Start a local multiplayer game.
    /// </summary>
    public void NewGame()
    {
        panelCheck.SetActive(false);
        OpenPanelGame(0);

        buttonRestartEndGame.onClick.AddListener(delegate { NewGame(); });
        buttonConfirmRestart.onClick.AddListener(delegate { NewGame(); });

        buttonPauseObject.SetActive(true);

        Chess.CleanScene();
        Chess.StartNewGame();
    }

    /// <summary>
    /// Start a loaded local multiplayer game.
    /// </summary>
    /// <param name="saveSlot">The save slot to be loaded.</param>
    public void LoadGame(int saveSlot)
    {
        panelCheck.SetActive(false);
        OpenPanelGame(0);

        buttonRestartEndGame.onClick.AddListener(delegate { NewGame(); });
        buttonConfirmRestart.onClick.AddListener(delegate { NewGame(); });

        buttonPauseObject.SetActive(true);

        SaveData data = SaveManager.LoadGame(saveSlot);

        Chess.CleanScene();
        Chess.StartLoadedGame(data);
    }

    /// <summary>
    /// Update the text where the server to which we are connected is indicated.
    /// </summary>
    public void UpdateServerName()
    {
        string server = "";

        switch (Options.ActiveServer)
        {
            case Options.Server.Asia:
                server = "Asia";
                break;
            case Options.Server.Australia:
                server = "Australia";
                break;
            case Options.Server.CanadaEast:
                server = "Canada East";
                break;
            case Options.Server.Europe:
                server = "Europe";
                break;
            case Options.Server.India:
                server = "India";
                break;
            case Options.Server.Japan:
                server = "Japan";
                break;
            case Options.Server.RussiaEast:
                server = "Russia East";
                break;
            case Options.Server.RussiaWest:
                server = "Russia West";
                break;
            case Options.Server.SouthAfrica:
                server = "South Africa";
                break;
            case Options.Server.SouthAmerica:
                server = "South America";
                break;
            case Options.Server.SouthKorea:
                server = "South Korea";
                break;
            case Options.Server.Turkey:
                server = "Turkey";
                break;
            case Options.Server.USAEast:
                server = "USA East";
                break;
            case Options.Server.USAWest:
                server = "USA West";
                break;
        }

        serverText.text = "Server " + server;
    }

    /// <summary>
    /// Activate the save button during online games.
    /// At the start of the game, the button is disabled to prevent the player from being able to save the game before it starts.
    /// </summary>
    public void EnableOnlineSave()
    {
        buttonSave.SetActive(true);
    }

    /// <summary>
    /// Save the current game in the selected slot.
    /// </summary>
    /// <param name="saveSlot">The slot where the game is to be saved.</param>
    public void SaveGame(int saveSlot)
    {
        Chess.SaveGame(saveSlot);

        UpdateSaveDates();
        OpenPanelPause(1);
    }

    /// <summary>
    /// Open an external link.
    /// </summary>
    /// <param name="link">The link that we want to open within the lists in the method.</param>
    public void OpenLink(int link)
    {
        if (link == 0)
        {
            Application.OpenURL("https://play.google.com/store/apps/developer?id=Sergio+Mejias");
        }

        else
        {
            Application.OpenURL("https://github.com/SergioMejiasDev/Chess-Android");
        }
    }

    /// <summary>
    /// Change the screen resolution to the selected one.
    /// </summary>
    /// <param name="resolution">The index of the resolution that we want to set.</param>
    public void ChangeResolution(int resolution)
    {
        switch (resolution)
        {
            case 0:
                Options.ActiveResolution = Options.Resolution.Fullscreen;
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.ExclusiveFullScreen);
                break;

            case 1:
                Options.ActiveResolution = Options.Resolution.Windowed720;
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                break;

            case 2:
                Options.ActiveResolution = Options.Resolution.Windowed480;
                Screen.SetResolution(854, 480, FullScreenMode.Windowed);
                break;
        }

        Options.SaveOptions();
    }

    /// <summary>
    /// Change the language of the application to the selected one.
    /// </summary>
    /// <param name="language">The index of the language that we want to set.</param>
    public void ChangeLanguage(int language)
    {
        switch (language)
        {
            case 0:
                Options.ActiveLanguage = Options.Language.EN;
                break;

            case 1:
                Options.ActiveLanguage = Options.Language.ES;
                break;

            case 2:
                Options.ActiveLanguage = Options.Language.CA;
                break;

            case 3:
                Options.ActiveLanguage = Options.Language.IT;
                break;
        }

        Options.SaveOptions();
        UpdateSaveDates();

        OpenPanelMenu(10);
    }

    /// <summary>
    /// Change the Photon server to the selected one.
    /// </summary>
    /// <param name="server">The index of the server that we want to set.</param>
    public void ChangeServer(int server)
    {
        Options.ActiveServer = (Options.Server)server;

        Options.SaveOptions();

        OpenPanelMenu(10);
    }


    /// <summary>
    /// Close the application completely.
    /// </summary>
    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion

    #region Pause Panel

    /// <summary>
    /// Opens the pause panel, making the pieces invisible.
    /// </summary>
    public void OpenPause()
    {
        if (!panelPause.activeSelf)
        {
            OpenPanelPause(0);
            Chess.PauseGame(true);
        }

        else
        {
            panelPause.SetActive(false);
            panelGame.SetActive(true);
            Chess.PauseGame(false);
        }
    }

    /// <summary>
    /// Opens the pause panel together with the indicated sub-panel.
    /// </summary>
    /// <param name="panel">The sub-panel to open.</param>
    public void OpenPanelPause(GameObject panel)
    {
        for (int i = 0; i < panelsPause.Length; i++)
        {
            panelsPause[i].SetActive(false);
        }

        panel.SetActive(true);
        panelGame.SetActive(false);
        panelPause.SetActive(true);
    }

    /// <summary>
    /// Opens the pause panel together with the indicated sub-panel.
    /// </summary>
    /// <param name="panel">The index of the sub-panel to open.</param>
    public void OpenPanelPause(int panel)
    {
        for (int i = 0; i < panelsPause.Length; i++)
        {
            panelsPause[i].SetActive(false);
        }

        panelsPause[panel].SetActive(true);
        panelGame.SetActive(false);
        panelPause.SetActive(true);
    }

    /// <summary>
    /// Updates the texts of the load and save buttons, and disables the load buttons if there is no saved game in the block.
    /// </summary>
    public void UpdateSaveDates()
    {
        string[] dates = SaveManager.GetDates();

        textLoad0.text = dates[0] != "0" ? ("AutoSave:  " + dates[0]) : ("AutoSave:  " + EmptyText());
        textLoad1.text = dates[1] != "0" ? ("Save 01:  " + dates[1]) : ("Save 01:  " + EmptyText());
        textLoad2.text = dates[2] != "0" ? ("Save 02:  " + dates[2]) : ("Save 02:  " + EmptyText());
        textLoad3.text = dates[3] != "0" ? ("Save 03:  " + dates[3]) : ("Save 03:  " + EmptyText());

        buttonLoad0.enabled = dates[0] != "0";
        buttonLoad1.enabled = dates[1] != "0";
        buttonLoad2.enabled = dates[2] != "0";
        buttonLoad3.enabled = dates[3] != "0";

        textSave1.text = dates[1] != "0" ? ("Save 01:  " + dates[1]) : ("Save 01:  " + EmptyText());
        textSave2.text = dates[2] != "0" ? ("Save 02:  " + dates[2]) : ("Save 02:  " + EmptyText());
        textSave3.text = dates[3] != "0" ? ("Save 03:  " + dates[3]) : ("Save 03:  " + EmptyText());

        buttonSave1.onClick.AddListener(delegate { ButtonSave(1, dates[1] == "0"); });
        buttonSave2.onClick.AddListener(delegate { ButtonSave(2, dates[2] == "0"); });
        buttonSave3.onClick.AddListener(delegate { ButtonSave(3, dates[3] == "0"); });
    }

    /// <summary>
    /// Updates the listeners of the load and save buttons according to the game mode that is active.
    /// </summary>
    /// <param name="gameMode">1 Single Player, 2 Local Multiplayer, 3 Online Multiplayer</param>
    public void UpdateLoadButton(int gameMode)
    {
        switch (gameMode)
        {
            case 1:
                buttonLoad0.onClick.AddListener(delegate { OpenPanelColoursLoad(0); });
                buttonLoad1.onClick.AddListener(delegate { OpenPanelColoursLoad(1); });
                buttonLoad2.onClick.AddListener(delegate { OpenPanelColoursLoad(2); });
                buttonLoad3.onClick.AddListener(delegate { OpenPanelColoursLoad(3); });
                break;

            case 2:
                buttonLoad0.onClick.AddListener(delegate { LoadGame(0); });
                buttonLoad1.onClick.AddListener(delegate { LoadGame(1); });
                buttonLoad2.onClick.AddListener(delegate { LoadGame(2); });
                buttonLoad3.onClick.AddListener(delegate { LoadGame(3); });
                break;

            case 3:
                buttonLoad0.onClick.AddListener(delegate { NetworkManager.manager.CreateLoadedRoom(0); });
                buttonLoad1.onClick.AddListener(delegate { NetworkManager.manager.CreateLoadedRoom(1); });
                buttonLoad2.onClick.AddListener(delegate { NetworkManager.manager.CreateLoadedRoom(2); });
                buttonLoad3.onClick.AddListener(delegate { NetworkManager.manager.CreateLoadedRoom(3); });
                break;
        }
    }

    /// <summary>
    /// Close the current game and return to the main menu.
    /// </summary>
    public void BackToMenuEndGame()
    {
        Chess.CleanScene();
        buttonRestart.SetActive(true);

        buttonPauseObject.SetActive(true);
        OpenPanelMenu(0);
        NetworkManager.manager.DisconnectFromServer();
    }

    /// <summary>
    /// Save the current game.
    /// </summary>
    /// <param name="saveSlot">The slot in which the game is to be saved.</param>
    /// <param name="emptySlot">Indicates if the slot in which the game will be saved is empty.</param>
    public void ButtonSave(int saveSlot, bool emptySlot)
    {
        // If the slot is empty, we save the game directly.

        if (emptySlot)
        {
            SaveGame(saveSlot);
        }

        // If there is a previously saved game, a message opens for the player to confirm that they want to overwrite it.

        else
        {
            buttonConfirmSave.onClick.AddListener(delegate { SaveGame(saveSlot); });
            OpenPanelPause(4);
        }
    }

    /// <summary>
    /// Activate or deactivate the pause button.
    /// </summary>
    /// <param name="enable">Enable or disable.</param>
    public void EnableButtonPause(bool enable)
    {
        if (enable)
        {
            buttonPauseObject.SetActive(true);
        }

        else
        {
            buttonPauseObject.SetActive(false);
        }
    }

    #endregion

    #region Game Panel

    /// <summary>
    /// Opens the brown panel on the right of the screen along with the selected sub-panel.
    /// </summary>
    /// <param name="panel">The sub-panel to open.</param>
    public void OpenPanelGame(GameObject panel)
    {
        for (int i = 0; i < panelsGame.Length; i++)
        {
            panelsGame[i].SetActive(false);
        }

        panel.SetActive(true);
        mainMenu.SetActive(false);
        panelPause.SetActive(false);
        panelGame.SetActive(true);
    }

    /// <summary>
    /// Opens the brown panel on the right of the screen along with the selected sub-panel.
    /// </summary>
    /// <param name="panel">The index of the sub-panel to open.</param>
    public void OpenPanelGame(int panel)
    {
        for (int i = 0; i < panelsGame.Length; i++)
        {
            panelsGame[i].SetActive(false);
        }

        panelsGame[panel].SetActive(true);
        mainMenu.SetActive(false);
        panelPause.SetActive(false);
        panelGame.SetActive(true);
    }

    /// <summary>
    /// Opens an error panel to indicate that the connection with the other player has been lost, thus closing the game.
    /// </summary>
    public void ErrorPlayerLeftRoom()
    {
        Chess.CleanScene();

        buttonPauseObject.SetActive(true);
        OpenPanelMenu(8);
    }

    /// <summary>
    /// Open the color selection panel in a new match against the AI.
    /// </summary>
    public void OpenPanelColours()
    {
        Chess.CleanScene();
        buttonPauseObject.SetActive(false);

        buttonColourWhite.onClick.AddListener(delegate { NewGameWhite(); });
        buttonColourBlack.onClick.AddListener(delegate { NewGameBlack(); });
        
        OpenPanelGame(7);
    }

    /// <summary>
    /// Open the color selection panel in a loaded match against the AI.
    /// </summary>
    /// <param name="saveSlot">The save slot from which the game is to be loaded.</param>
    public void OpenPanelColoursLoad(int saveSlot)
    {
        Chess.CleanScene();
        buttonPauseObject.SetActive(false);

        buttonColourWhite.onClick.AddListener(delegate { LoadGameWhite(saveSlot); });
        buttonColourBlack.onClick.AddListener(delegate { LoadGameBlack(saveSlot); });
        
        OpenPanelGame(7);
    }

    #endregion

    #region Waiting Panel

    /// <summary>
    /// Activate the message to indicate that it is a player's turn and we are waiting for him to play.
    /// </summary>
    /// <param name="colour">The color of the player in turn.</param>
    public void SetWaitingMessage(Enums.Colours colour)
    {
        OpenPanelGame(0);

        notificationsText.text = (colour == Enums.Colours.White) ? WaitingMessageWhite() : WaitingMessageBlack();
        notificationsImage.color = (colour == Enums.Colours.White) ? Color.white : Color.black;
    }

    #endregion

    #region Waiting Player 2 Panel

    /// <summary>
    /// Open the panel to indicate that we are waiting for a second player to start the game.
    /// </summary>
    public void OpenPanelWaitingPlayer()
    {
        OpenPanelGame(2);
        textWaitingRoomName.text = RoomName();
        buttonSave.SetActive(false);
        buttonRestart.SetActive(false);
    }

    #endregion

    #region Keyboard Panel

    /// <summary>
    /// The name of the room.
    /// </summary>
    string roomName;

    /// <summary>
    /// Activate the panel with the keyboard to enter the name of the room we want to join.
    /// </summary>
    public void OpenPanelKeyboard()
    {
        OpenPanelGame(3);
        roomName = "";
        textRoomName.text = roomName;
        buttonSave.SetActive(false);
        buttonRestart.SetActive(false);
    }

    /// <summary>
    /// Add the letter entered to the name of the room.
    /// </summary>
    /// <param name="character">The letter entered.</param>
    public void EnterRoomLetter(string character)
    {
        if (roomName.Length < 3)
        {
            roomName += character;

            textRoomName.text = roomName;
        }
    }

    /// <summary>
    /// Removes the last letter entered from the room name.
    /// </summary>
    public void DeleteRoomLetter()
    {
        if (roomName.Length > 0)
        {
            string newRoomName = "";

            for (int i = 0; i < roomName.Length - 1; i++)
            {
                newRoomName += roomName[i];
            }

            roomName = newRoomName;
            textRoomName.text = roomName;
        }
    }

    /// <summary>
    /// Enter the room with the name entered.
    /// </summary>
    public void JoinRoom()
    {
        if (roomName.Length == 3)
        {
            NetworkManager.manager.JoinRoom(roomName);
        }
    }

    #endregion

    #region Check Message

    /// <summary>
    /// Activate a panel to indicate that a player is in check.
    /// </summary>
    /// <param name="colour">The color of the player in check.</param>
    public void ActivatePanelCheck(Enums.Colours colour)
    {
        checkText.text = (colour == Enums.Colours.White) ? CheckMessageWhite() : CheckMessageBlack();

        panelCheck.SetActive(true);
    }

    /// <summary>
    /// Disable the check notification panel.
    /// </summary>
    public void DeactivatePanelCheck()
    {
        panelCheck.SetActive(false);
    }

    #endregion

    #region Promotion Message

    /// <summary>
    /// Activate the panel to indicate that a white pawn has been promoted.
    /// </summary>
    /// <param name="inTurn">True if the promoting player is controlled from the current device (AI is false).</param>
    public void ActivatePromotionWhite(bool inTurn)
    {
        OpenPanelGame(1);

        // If it is the player's turn, a panel opens where he can choose one of four possible pieces.

        if (inTurn)
        {
            panelPiecesWhite.SetActive(true);
            promotionText.text = PromotionMessageWhite();
        }

        // If this is not the case, a notification opens to indicate that it is waiting for the other player to choose.

        else
        {
            promotionText.text = WaitPromotionMessageWhite();
        }
    }

    /// <summary>
    /// Activate the panel to indicate that a white pawn has been promoted.
    /// </summary>
    /// <param name="inTurn">True if the promoting player is controlled from the current device (AI is false).</param>
    public void ActivatePromotionBlack(bool inTurn)
    {
        OpenPanelGame(1);

        // If it is the player's turn, a panel opens where he can choose one of four possible pieces.

        if (inTurn)
        {
            panelPiecesBlack.SetActive(true);
            promotionText.text = PromotionMessageBlack();
        }

        // If this is not the case, a notification opens to indicate that it is waiting for the other player to choose.

        else
        {
            promotionText.text = WaitPromotionMessageBlack();
        }
    }

    /// <summary>
    /// Promote a white pawn on the chosen piece.
    /// </summary>
    /// <param name="piece">The chosen piece.</param>
    public void PromotePieceWhite(string piece)
    {
        if (!NetworkManager.manager.IsConnected)
        {
            switch (piece)
            {
                case "Rook":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Rook, Pieces.Colour.White);
                    break;
                case "Knight":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Knight, Pieces.Colour.White);
                    break;
                case "Bishop":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Bishop, Pieces.Colour.White);
                    break;
                case "Queen":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Queen, Pieces.Colour.White);
                    break;
            }
        }

        else
        {
            switch (piece)
            {
                case "Rook":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Rook, Pieces.Colour.White);
                    break;
                case "Knight":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Knight, Pieces.Colour.White);
                    break;
                case "Bishop":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Bishop, Pieces.Colour.White);
                    break;
                case "Queen":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Queen, Pieces.Colour.White);
                    break;
            }
        }
    }

    /// <summary>
    /// Promote a black pawn on the chosen piece.
    /// </summary>
    /// <param name="piece">The chosen piece.</param>
    public void PromotePieceBlack(string piece)
    {
        if (!NetworkManager.manager.IsConnected)
        {
            switch (piece)
            {
                case "Rook":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Rook, Pieces.Colour.Black);
                    break;
                case "Knight":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Knight, Pieces.Colour.Black);
                    break;
                case "Bishop":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Bishop, Pieces.Colour.Black);
                    break;
                case "Queen":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Queen, Pieces.Colour.Black);
                    break;
            }
        }

        else
        {
            switch (piece)
            {
                case "Rook":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Rook, Pieces.Colour.Black);
                    break;
                case "Knight":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Knight, Pieces.Colour.Black);
                    break;
                case "Bishop":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Bishop, Pieces.Colour.Black);
                    break;
                case "Queen":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Queen, Pieces.Colour.Black);
                    break;
            }
        }
    }

    /// <summary>
    /// Disable the pawn promotion panel.
    /// </summary>
    public void DisablePromotions()
    {
        panelsGame[1].SetActive(false);
        panelPiecesBlack.SetActive(false);
        panelPiecesWhite.SetActive(false);
    }

    #endregion

    #region Checkmate Message

    /// <summary>
    /// Activate the checkmate panel.
    /// </summary>
    /// <param name="colour">The color of the player who wins the game.</param>
    public void ActivateCheckmateMessage(Enums.Colours colour)
    {
        if (!NetworkManager.manager.IsConnected)
        {
            OpenPanelGame(4);
        }

        // The panel that opens is different if we are playing online or not. Also, if we are playing online, we disconnect from the server.

        else
        {
            OpenPanelGame(6);
            NetworkManager.manager.DisconnectAll();
        }

        panelNotifications.SetActive(true);
        buttonPauseObject.SetActive(false);
        panelCheck.SetActive(false);

        notificationsText.text = (colour == Enums.Colours.White) ? CheckmateMessageWhite() : CheckmateMessageBlack();
        notificationsImage.color = (colour == Enums.Colours.White) ? Color.white : Color.black;
    }

    #endregion

    #region Draw Message

    /// <summary>
    /// Activate a panel to indicate that the game has ended in a draw.
    /// </summary>
    /// <param name="drawType">The reason the game ended in a draw.</param>
    public void ActivateDrawMessage(Enums.DrawModes drawType)
    {
        if (!NetworkManager.manager.IsConnected)
        {
            OpenPanelGame(4);
        }

        else
        {
            OpenPanelGame(6);
            NetworkManager.manager.DisconnectAll();
        }

        panelNotifications.SetActive(true);
        notificationsImage.color = Color.blue;

        switch (drawType)
        {
            case Enums.DrawModes.Stalemate:
                notificationsText.text = DrawStalemateMessage();
                break;
            case Enums.DrawModes.Impossibility:
                notificationsText.text = DrawImpossibilityMessage();
                break;
            case Enums.DrawModes.Move75:
                notificationsText.text = Draw75MovesMessage();
                break;
            case Enums.DrawModes.ThreefoldRepetition:
                notificationsText.text = DrawRepetitionMessage();
                break;
        }
    }

    #endregion

    #region Texts

    /// <summary>
    /// Indicates that the server is full at the moment (it has reached the limit of 20 people).
    /// </summary>
    /// <returns></returns>
    string ServerFull()
    {
        return Resources.Load<TranslateText>("Texts/ServerFull").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Indicates that there is no internet connection on the current device.
    /// </summary>
    /// <returns></returns>
    string NoInternetConnection()
    {
        return Resources.Load<TranslateText>("Texts/NoInternetConnection").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Indicates that the selected region is not available at the moment (it does not refer to Photon servers in general, but to those of a specific region).
    /// </summary>
    /// <returns></returns>
    string InvalidRegion()
    {
        return Resources.Load<TranslateText>("Texts/InvalidRegion").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Indicates that a generic server error has occurred.
    /// </summary>
    /// <returns></returns>
    string GenericServerError()
    {
        return Resources.Load<TranslateText>("Texts/GenericServerError").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Text to indicate that a save slot is empty.
    /// </summary>
    /// <returns></returns>
    string EmptyText()
    {
        return Resources.Load<TranslateText>("Texts/EmptyText").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// It indicates that we are waiting for White to play.
    /// </summary>
    /// <returns></returns>
    string WaitingMessageWhite()
    {
        return Resources.Load<TranslateText>("Texts/WaitingMessageWhite").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// It indicates that we are waiting for Black to play.
    /// </summary>
    /// <returns></returns>
    string WaitingMessageBlack()
    {
        return Resources.Load<TranslateText>("Texts/WaitingMessageBlack").GetText(Options.ActiveLanguage);
    }

    string CheckMessageWhite()
    {
        return Resources.Load<TranslateText>("Texts/CheckMessageWhite").GetText(Options.ActiveLanguage);
    }

    string CheckMessageBlack()
    {
        return Resources.Load<TranslateText>("Texts/CheckMessageBlack").GetText(Options.ActiveLanguage);
    }

    string PromotionMessageWhite()
    {
        return Resources.Load<TranslateText>("Texts/PromotionMessageWhite").GetText(Options.ActiveLanguage);
    }

    string PromotionMessageBlack()
    {
        return Resources.Load<TranslateText>("Texts/PromotionMessageBlack").GetText(Options.ActiveLanguage);
    }

    string WaitPromotionMessageWhite()
    {
        return Resources.Load<TranslateText>("Texts/WaitPromotionMessageWhite").GetText(Options.ActiveLanguage);
    }

    string WaitPromotionMessageBlack()
    {
        return Resources.Load<TranslateText>("Texts/WaitPromotionMessageBlack").GetText(Options.ActiveLanguage);
    }

    string CheckmateMessageWhite()
    {
        return Resources.Load<TranslateText>("Texts/CheckmateMessageWhite").GetText(Options.ActiveLanguage);
    }

    string CheckmateMessageBlack()
    {
        return Resources.Load<TranslateText>("Texts/CheckmateMessageBlack").GetText(Options.ActiveLanguage);
    }

    string DrawStalemateMessage()
    {
        return Resources.Load<TranslateText>("Texts/DrawStalemateMessage").GetText(Options.ActiveLanguage);
    }

    string DrawImpossibilityMessage()
    {
        return Resources.Load<TranslateText>("Texts/DrawImpossibilityMessage").GetText(Options.ActiveLanguage);
    }

    string Draw75MovesMessage()
    {
        return Resources.Load<TranslateText>("Texts/Draw75MovesMessage").GetText(Options.ActiveLanguage);
    }

    string DrawRepetitionMessage()
    {
        return Resources.Load<TranslateText>("Texts/DrawRepetitionMessage").GetText(Options.ActiveLanguage);
    }

    string RoomName()
    {
        return Resources.Load<TranslateText>("Texts/RoomName").GetText(Options.ActiveLanguage) + "" + NetworkManager.manager.ActiveRoom;
    }

    #endregion
}