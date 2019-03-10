using DarkRift;
using DarkRift.Server;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkCheckersPiece : NetworkObject
{

    public PlayerPiece piece;

    /// Last received message from the server
    public SyncMessageModel lastReceivedMessage;

    /// Tick counted by the client
    public int clientTick = -1;

    public List<SyncMessageModel> reconciliationInfoList;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        ////////////////////////////////////
        // Get references

        piece = GetComponent<PlayerPiece>();

        //If we are on client side
        if (!Equals(ClientManager.instance, null))
        {
            //////////////////
            /// Suscribe to events
            ClientManager.instance.clientReference.MessageReceived += UpdateFromServerState;
            reconciliationInfoList = new List<SyncMessageModel>();
        }

    }

    private void FixedUpdate()
    {
        //If we are on server side
        if (!Equals(GameServerManager.instance, null))
        {
            if (GameServerManager.instance.currentTick % 10 == 0)
                SendPiecePositionToClients();
        }

        else if (!Equals(ClientManager.instance, null) && clientTick != -1)
        {
            clientTick++;
            reconciliationInfoList.Add(new SyncMessageModel
            {
                
                x = piece.x,
                serverTick = clientTick,
                y = piece.y,

            });
            Reconciliate();
        }
    }

    /// <summary>
    /// Reconciliate the client with the server data
    /// </summary>
    private void Reconciliate()
    {
        if (reconciliationInfoList.Count() > 0)
        {
            //Get the position of the client at this specific frame
            SyncMessageModel clientInfo = reconciliationInfoList.Where(i => i.serverTick == lastReceivedMessage.serverTick).FirstOrDefault();
            //If there is more than 50 tick that the ball has not been updated depending to the server position
            if (reconciliationInfoList.Count() > 50)
            {
                piece.x = lastReceivedMessage.x;
                piece.y = lastReceivedMessage.y;
                clientTick = lastReceivedMessage.serverTick;
                clientInfo = lastReceivedMessage;
            }
            if (!Equals(clientInfo, null))
            {
                //Check for position divergence
                if (!Equals(clientInfo.x, lastReceivedMessage.x))
                {
                    //Update data
                    piece.x = lastReceivedMessage.x;
                }
                if (!Equals(clientInfo.y, lastReceivedMessage.y))
                {
                    //Update data
                    piece.y = lastReceivedMessage.y;
                }
                //Empty the list
                reconciliationInfoList.Clear();
            }
        }
    }

    private void OnDestroy()
    {
        //If we are on client side
        if (!Equals(ClientManager.instance, null))
        {
            ClientManager.instance.clientReference.MessageReceived -= UpdateFromServerState;
        }
    }

    /// <summary>
    /// update from the server state
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateFromServerState(object sender, DarkRift.Client.MessageReceivedEventArgs e)
    {
        if (e.Tag == NetworkTags.InGame.PIECE_SYNC_POS)
        {
            //Get message data
            SyncMessageModel syncMessage = e.GetMessage().Deserialize<SyncMessageModel>();
            //If this is the first time we receive the message
            if (Object.Equals(null, lastReceivedMessage))
            {
                //Update data
                piece.x = syncMessage.x;
                piece.y = syncMessage.y;
                clientTick = syncMessage.serverTick;
                lastReceivedMessage = syncMessage;
            }
            //If the message regards this object and is older than the previous one
            if (id == syncMessage.networkID && syncMessage.serverTick > lastReceivedMessage.serverTick)
            {
                lastReceivedMessage = syncMessage;
            }
        }
    }

    /// <summary>
    /// Send piece server position to all clients
    /// </summary>
    private void SendPiecePositionToClients()
    {
        //Create the message
        SyncMessageModel piecePositionMessageData = new SyncMessageModel
        {
            networkID = base.id,
            serverTick = GameServerManager.instance.currentTick,
            x = piece.x,
            y = piece.y
        };
        //create the message 
        using (Message m = Message.Create(
            NetworkTags.InGame.PIECE_SYNC_POS,        //Tag
            piecePositionMessageData)                  //Data
        )
        {
            foreach (IClient client in GameServerManager.instance.serverReference.Server.ClientManager.GetAllClients())
            {
                client.SendMessage(m, SendMode.Reliable);
            }
        }
    }
}
