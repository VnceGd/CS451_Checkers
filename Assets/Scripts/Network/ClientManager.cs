using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using UnityEngine.UI;

public class ClientManager : MonoBehaviourSingletonPersistent<ClientManager>
{



    /// <summary>
    /// Reference to the DarkRift2 client
    /// </summary>
    public UnityClient clientReference;
    public string address = GameManager.instance.settings.DefaultNetworkAddress;
    public int port = int.Parse(GameManager.instance.settings.DefaultNetworkPort);

    public Text testLogging;

    public BoardGenerator bg;

    private void Awake()
    {
        base.Awake();

        //////////////////
        /// Properties initialization
        clientReference = GetComponent<UnityClient>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //////////////////
        /// Load the game scene
        // SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        bg = BoardGenerator.instance;
        clientReference.MessageReceived += SpawnGameObjects;
        clientReference.MessageReceived += MoveChange;
        clientReference.MessageReceived += RemovePiece;
        //////////////////
        /// Connect to the server manually
        clientReference.ConnectInBackground(
            IPAddress.Parse(address),
            port,
            DarkRift.IPVersion.IPv4,
            null
            );
    }
    public void SendPieceCoordinates(PlayerPiece p)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            //write the piece ID and its updated x and y coordinates
            writer.Write(p.GetComponent<NetworkCheckersPiece>().id);
            writer.Write(p.x);
            writer.Write(p.y);

            using (Message message = Message.Create(NetworkTags.InGame.CALLBACK_PIECE, writer))
                clientReference.SendMessage(message, SendMode.Unreliable);
            Player.instance.canMakeMove = false;
        }
    }
    public void SendMoveComplete()
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            //write the piece ID and its updated x and y coordinates
            writer.Write("Move made");

            using (Message message = Message.Create(NetworkTags.InGame.MOVE_CHANGE, writer))
                clientReference.SendMessage(message, SendMode.Unreliable);
            Player.instance.canMakeMove = false;
        }
    }
    private void MoveChange(object sender, MessageReceivedEventArgs e)
    {
        if (e.Tag == NetworkTags.InGame.MOVE_CHANGE)
        {
            Player.instance.canMakeMove = true;
        }
    }
    public void SendRemovePiece(PlayerPiece p)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            //write the piece ID and its updated x and y coordinates
            writer.Write(p.GetComponent<NetworkCheckersPiece>().id);

            using (Message message = Message.Create(NetworkTags.InGame.DEFEAT, writer))
                clientReference.SendMessage(message, SendMode.Unreliable);
            Player.instance.canMakeMove = false;
        }
    }
    private void SendPromotePiece(PlayerPiece p)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            //write the piece ID and its updated x and y coordinates
            writer.Write(p.GetComponent<NetworkCheckersPiece>().id);

            using (Message message = Message.Create(NetworkTags.InGame.DEFEAT, writer))
                clientReference.SendMessage(message, SendMode.Unreliable);
            Player.instance.canMakeMove = false;
        }
    }
    private void SpawnGameObjects(object sender, MessageReceivedEventArgs e)
    {
        if (e.Tag == NetworkTags.InGame.SPAWN_OBJECT)
        {
            //Get message data
            SpawnMessage spawnMessage = e.GetMessage().Deserialize<SpawnMessage>();
            //Spawn the game object
            string resourcePath = NetworkObjectDictionnary.GetResourcePathFor(spawnMessage.resourceID);
            GameObject go = Resources.Load(resourcePath) as GameObject;
            go.GetComponent<NetworkObject>().id = spawnMessage.networkID;
            go = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
            var piece = go.GetComponent<PlayerPiece>();
            BoardGenerator.instance.SetPieceLocation(piece, piece.x, piece.y);
            Player.instance.canMakeMove = true;
            //Instantiate(go, new Vector3(piece.x, piece.y, 0), Quaternion.identity);
        }
    }
    private void RemovePiece(object sender, MessageReceivedEventArgs e)
    {
        if (e.Tag == NetworkTags.InGame.DEFEAT)
        {
            using (Message message = e.GetMessage() as Message)
            {
                using (DarkRiftReader reader = message.GetReader())
                {
                    int tX = reader.ReadInt32();
                    int tY = reader.ReadInt32();
                    BoardGenerator.instance.RemovePiece(tX, tY);
                }
            }
        }
    }
}