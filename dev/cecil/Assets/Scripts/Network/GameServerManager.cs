using DarkRift;
using DarkRift.Server;
using DarkRift.Server.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using UnityEngine.UI;
using System.Linq;

public class GameServerManager : MonoBehaviourSingletonPersistent<GameServerManager>
{

    public Text testText;

    public IClient connectedClient;

    public List<int> clientsId; //list of connected clients

    public List<NetworkObject> networkObjects;

    public XmlUnityServer serverReference;

    /// <summary>
    /// Last tick received from the server
    /// </summary>
    public int currentTick = -1;

    // Start is called before the first frame update
    void Start()
    {
        clientsId = new List<int>();
        serverReference = GetComponent<XmlUnityServer>();
        networkObjects = new List<NetworkObject>();

        serverReference.Server.ClientManager.ClientConnected += ClientConnected;
        serverReference.Server.ClientManager.ClientDisconnected += ClientDisconnected;

        SceneManager.LoadScene("Game", LoadSceneMode.Additive);

    }

    void FixedUpdate()
    {
        currentTick++;
    }

    /// <summary>
    /// When a client connects to the DarkRift server
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ClientConnected(object sender, ClientConnectedEventArgs e)
    {
        clientsId.Add(e.Client.ID);

        SendAllObjectsToSpawnTo(e.Client);
        connectedClient = e.Client;
        e.Client.MessageReceived += UpdateServerMessageRecieved;
        e.Client.MessageReceived += MoveChange;
        e.Client.MessageReceived += RemovePiece;
    }

    void UpdateServerMessageRecieved(object sender, MessageReceivedEventArgs e)
    {
        using(Message message = e.GetMessage() as Message)
        {
            if(message.Tag == NetworkTags.InGame.CALLBACK_PIECE)
            {
                using(DarkRiftReader reader = message.GetReader())
                {
                    //BoardGenerator.instance.Set
                    var id = reader.ReadInt32();
                    int targetX = reader.ReadInt32();
                    int targetY = reader.ReadInt32();
                    testText.text = id.ToString();
                    NetworkObject obj = networkObjects.FirstOrDefault(x => x.id == id);
                    PlayerPiece targetPiece = obj.GetComponent<PlayerPiece>();
                    BoardGenerator.instance.SetPieceLocation(targetPiece, targetX, targetY);
                    SendObjectToSpawnTo(obj, e.Client);
                }
            }
        }
    }
    void MoveChange(object sender, MessageReceivedEventArgs e)
    {
        if(e.Tag == NetworkTags.InGame.MOVE_CHANGE)
        {
            Player.instance.canMakeMove = true;
        }
    }
    void RemovePiece(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage() as Message)
        {
            if (message.Tag == NetworkTags.InGame.DEFEAT)
            {
                using (DarkRiftReader reader = message.GetReader())
                {
                    var id = reader.ReadInt32();
                    NetworkObject obj = networkObjects.FirstOrDefault(x => x.id == id);
                    BoardGenerator.instance.RemovePiece(obj.GetComponent<PlayerPiece>().x, obj.GetComponent<PlayerPiece>().y) ;
                    SendRemovePiece(obj.GetComponent<PlayerPiece>());
                }
            }
        }
    }

    /// <summary>
    /// When a client disconnects to the DarkRift server
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
    {
        throw new NotImplementedException();
    }

    #region Implementation
    /// <summary>
    /// Use this function to add a network object that must be handle by the server
    /// </summary>
    /// <param name="pNetworkObject"></param>
    public void RegisterNetworkObject(NetworkObject pNetworkObject)
    {
        //Add the object to the list
        networkObjects.Add(pNetworkObject);
    }
    /// <summary>
    /// Send a message to the client to spawn an object into its scene
    /// </summary>
    /// <param name="pClient"></param>
    public void SendObjectToSpawnTo(NetworkObject pNetworkObject, IClient pClient)
    {
        //Spawn data to send
        SpawnMessage spawnMessageData = new SpawnMessage
        {
            networkID = pNetworkObject.id,
            resourceID = pNetworkObject.resourceId,
            x = pNetworkObject.GetComponent<PlayerPiece>().x,
            y = pNetworkObject.GetComponent<PlayerPiece>().y
        };
        //create the message 
        using (Message m = Message.Create(
            NetworkTags.InGame.SPAWN_OBJECT,                //Tag
            spawnMessageData)                               //Data
        )
        {
            //Send the message in TCP mode (Reliable)
            pClient.SendMessage(m, SendMode.Reliable);
        }
    }

    /// <summary>
    /// Send a message with all objects to spawn
    /// </summary>
    /// <param name="pClient"></param>
    public void SendAllObjectsToSpawnTo(IClient pClient)
    {
        foreach (NetworkObject networkObject in networkObjects)
            SendObjectToSpawnTo(networkObject, pClient);
    }

    public void SendMoveChange()
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            //write the piece ID and its updated x and y coordinates
            writer.Write("Move made");

            using (Message message = Message.Create(NetworkTags.InGame.MOVE_CHANGE, writer))
            {
                connectedClient.SendMessage(message, SendMode.Unreliable);
            }
            Player.instance.canMakeMove = false;
        }
    }
    public  void SendRemovePiece(PlayerPiece p)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            //write the piece ID and its updated x and y coordinates
            writer.Write(p.x);
            writer.Write(p.y);

            using (Message message = Message.Create(NetworkTags.InGame.DEFEAT, writer))
                connectedClient.SendMessage(message, SendMode.Unreliable);
        }
    }
    private void SendPromotePiece(PlayerPiece p)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            //write the piece ID and its updated x and y coordinates
            writer.Write(p.x);
            writer.Write(p.y);

            using (Message message = Message.Create(NetworkTags.InGame.DEFEAT, writer))
                connectedClient.SendMessage(message, SendMode.Unreliable);
            Player.instance.canMakeMove = false;
        }
    }
    #endregion

}
