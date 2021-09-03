using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

/// <summary>
/// It contains the necessary methods for the online functions of the game. Inherited from MonoBehaviour so it must be assigned to an object in scene.
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class NetworkManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// The singleton of the class.
    /// </summary>
    public static NetworkManager manager;

    /// <summary>
    /// The loaded game, if it exists.
    /// </summary>
    SaveData loadData = null;

    #region Properties

    /// <summary>
    /// It indicates if we are connected to the Photon servers.
    /// </summary>
    public bool IsConnected { get; private set; }

    /// <summary>
    /// The name (three characters) of the room in which we are playing.
    /// </summary>
    public string ActiveRoom { get; private set; }

    /// <summary>
    /// The identifier of the server to connect to. These are given by the Photon PUN documentation.
    /// The value used is the one saved in the game options, and can be changed from there.
    /// </summary>
    string Token
    {
        get
        {
            switch (Options.ActiveServer)
            {
                case Options.Server.Asia:
                    return "asia";
                case Options.Server.Australia:
                    return "au";
                case Options.Server.CanadaEast:
                    return "cae";
                case Options.Server.Europe:
                    return "eu";
                case Options.Server.India:
                    return "in";
                case Options.Server.Japan:
                    return "jp";
                case Options.Server.RussiaEast:
                    return "rue";
                case Options.Server.RussiaWest:
                    return "ru";
                case Options.Server.SouthAfrica:
                    return "za";
                case Options.Server.SouthAmerica:
                    return "sa";
                case Options.Server.SouthKorea:
                    return "kr";
                case Options.Server.Turkey:
                    return "tr";
                case Options.Server.USAEast:
                    return "us";
                case Options.Server.USAWest:
                    return "usw";
                default:
                    return "eu";
            }
        }
    }

    /// <summary>
    /// String composed of three random letters used to create the name of the room.
    /// </summary>
    string RandomRoom
    {
        get
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            char char1 = characters[Random.Range(0, characters.Length)];
            char char2 = characters[Random.Range(0, characters.Length)];
            char char3 = characters[Random.Range(0, characters.Length)];

            return char1.ToString() + char2.ToString() + char3.ToString();
        }
    }

    #endregion

    private void Awake()
    {
        manager = this;
    }

    #region Conection

    /// <summary>
    /// Connect the game to the Photon servers.
    /// </summary>
    public void ConnectToServer()
    {
        // A message appears on the screen indicating that we are connecting.

        Interface.interfaceClass.OpenPanelMenu(2);

        // We connect to the selected server.

        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = Token;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // We indicate that we are connected to Photon.

        IsConnected = true;

        // We tell Photon how to serialize and deserialize the SaveData class and be able to transmit it.
        // This will be necessary for when we load games online.

        PhotonPeer.RegisterType(typeof(SaveData), (byte)'S', SaveManager.Serialize, SaveManager.Deserialize);

        // The "Connecting" message disappears and the game selection menu appears.

        Interface.interfaceClass.UpdateServerName();
        Interface.interfaceClass.OpenPanelMenu(6);
    }

    /// <summary>
    /// The game's connection to the Photon servers ends.
    /// </summary>
    public void DisconnectFromServer()
    {
        // We indicate that we are disconnected and call the necessary function to disconnect.

        IsConnected = false;
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // There can be many unintended reasons for server disconnections. From here we launch the error messages to give feedback to the user.

        switch (cause)
        {
            // The server is full (the limit of 20 people playing has been reached).
            case DisconnectCause.MaxCcuReached:
                Interface.interfaceClass.OpenErrorPanel(cause);
                break;

            // The device in use does not have an Internet connection at this time.
            case DisconnectCause.DnsExceptionOnConnect:
                Interface.interfaceClass.OpenErrorPanel(cause);
                break;

            // The server (the specific region selected) is not working at the moment
            case DisconnectCause.InvalidRegion:
                Interface.interfaceClass.OpenErrorPanel(cause);
                break;

            // This case occurs when the player is disconnected manually or when the connection with the room host is lost.
            // For the first case it is not necessary, but for the latter, we must throw an error message.
            case DisconnectCause.DisconnectByClientLogic:
                if (Chess.IsPlaying)
                    Interface.interfaceClass.ErrorPlayerLeftRoom();
                break;

            // The rest of the possible errors are included here, launching a more generic error message.
            default:
                Interface.interfaceClass.OpenErrorPanel(cause);
                break;
        }

        Debug.Log(cause);
    }

    /// <summary>
    /// Create a room with a random name and start waiting for a second player to start a game from the beginning.
    /// </summary>
    public void CreateRoom()
    {
        // We indicate that there is no loaded game, so the game starts from the beginning.

        loadData = null;

        // We get a random name for the room.

        ActiveRoom = RandomRoom;

        // We create a room with the name obtained previously and establishing a maximum of two players.

        PhotonNetwork.CreateRoom(ActiveRoom, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    /// <summary>
    /// Create a room with a random name and start waiting for a second player to start a loaded game.
    /// </summary>
    /// <param name="saveSlot">The save slot of the game to be loaded.</param>
    public void CreateLoadedRoom(int saveSlot)
    {
        // We load the game according to the chosen slot.

        loadData = SaveManager.LoadGame(saveSlot);

        // We get a random name for the room.

        ActiveRoom = RandomRoom;

        // We create a room with the name obtained previously and establishing a maximum of two players.

        PhotonNetwork.CreateRoom(ActiveRoom, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        // If the room has been created without problems, we open the menu where it is
        // indicated that we arewaiting for the second player and the name of the room is displayed.

        Interface.interfaceClass.OpenPanelWaitingPlayer();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // If the creation of the room has failed (the name already existed before creating it), we repeat the function with a different room name.

        ActiveRoom = "";

        CreateRoom();
    }

    /// <summary>
    /// Initiates the connection with a room previously created by another player.
    /// </summary>
    /// <param name="roomName">Name of the room to connect to.</param>
    public void JoinRoom(string roomName)
    {
        ActiveRoom = roomName;
        PhotonNetwork.JoinRoom(ActiveRoom);
    }

    public override void OnJoinedRoom()
    {
        // If when a player joins the room, the number of players is one (you have just created the room), it is assigned the color white.

        if (PhotonNetwork.PlayerList.Length == 1)
        {
            Chess.PlayerColour = Enums.Colours.White;
        }

        // If when a player joins the room, the number of players is two (in the room there was another player waiting), the color black is assigned.

        else if (PhotonNetwork.PlayerList.Length == 2)
        {
            Chess.PlayerColour = Enums.Colours.Black;
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // If the room we want to join does not exist or already contains two players, we send an error message to the user.

        Interface.interfaceClass.OpenPanelGame(5);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // When the second player enters the room, we start the game.
        // This function is only called on the device that created the room.

        if (PhotonNetwork.PlayerList.Length == 2)
        {
            // If there is no loaded game, it is played from the beginning.

            if (loadData == null)
            {
                StartGame();
            }

            // If there is a loaded game, this game is started.

            else
            {
                StartLoadedGame();
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // If one of the players leaves the room, the other player is disconnected, displaying an error message on the screen.

        DisconnectFromServer();
    }

    #endregion

    #region Game RPCs

    /// <summary>
    /// Start a new game on the two devices connected to the room.
    /// </summary>
    public void StartGame()
    {
        photonView.RPC("StartGameRPC", RpcTarget.All);
    }

    /// <summary>
    /// A new game is started on this device and remotely on player two's device.
    /// </summary>
    [PunRPC]
    void StartGameRPC()
    {
        Chess.StartNewGame();
    }

    /// <summary>
    /// Start a loaded game on the two devices connected to the room.
    /// </summary>
    public void StartLoadedGame()
    {
        photonView.RPC("StartLoadedGameRPC", RpcTarget.All, loadData);
    }

    /// <summary>
    /// A new game is started on this device and remotely on player two's device.
    /// Player one sends the save to player two and he loads it too.
    /// </summary>
    [PunRPC]
    void StartLoadedGameRPC(SaveData data)
    {
        Chess.StartLoadedGame(data);
    }

    /// <summary>
    /// Move a piece across all devices connected to the same room.
    /// </summary>
    /// <param name="piecePosition">The position of the piece to be moved.</param>
    /// <param name="movePosition">The position where the piece is going to move.</param>
    public void MovePiece(Vector2 piecePosition, Vector2 movePosition)
    {
        photonView.RPC("MovePieceRPC", RpcTarget.All, piecePosition, movePosition);
    }

    /// <summary>
    /// The indicated piece is moved on this server and an RPC is launched to the other device to do the same.
    /// </summary>
    /// <param name="piecePosition">The position of the piece to be moved.</param>
    /// <param name="movePosition">The position where the piece is going to move.</param>
    [PunRPC]
    void MovePieceRPC(Vector2 piecePosition, Vector2 movePosition)
    {
        // We locate the piece in the indicated position.

        Chess.SelectPiece(piecePosition);

        // We move the piece to the indicated position.

        Chess.MovePiece(movePosition);
    }

    /// <summary>
    /// We make a pawn promote (exchange for another piece) on all devices in the room.
    /// </summary>
    /// <param name="piece">The type of piece to be promoted in.</param>
    /// <param name="colour">The color of the piece to be promoted.</param>
    public void PromotePiece(Enums.PromotablePieces piece, Pieces.Colour colour)
    {
        photonView.RPC("PromotePieceRPC", RpcTarget.All, piece, colour);
    }

    /// <summary>
    /// We exchange the promotional pawn for the selected piece on this device and we do the same on the other device through an RPC.
    /// </summary>
    /// <param name="piece">The type of piece to be promoted in.</param>
    /// <param name="colour">The color of the piece to be promoted.</param>
    [PunRPC]
    void PromotePieceRPC(Enums.PromotablePieces piece, Pieces.Colour colour)
    {
        Chess.PieceSelectedToPromotion(piece, colour);
    }

    /// <summary>
    /// Disconnect all players from the game after finishing the game.
    /// </summary>
    public void DisconnectAll()
    {
        photonView.RPC("DisconnectAllRPC", RpcTarget.AllViaServer);
    }

    /// <summary>
    /// Launch an RPC to the server to call the disconnect function in sync.
    /// </summary>
    [PunRPC]
    void DisconnectAllRPC()
    {
        // Before disconnecting from the server, we indicate that we are not playing to avoid an error message.

        Chess.IsPlaying = false;

        DisconnectFromServer();
    }

    #endregion
}