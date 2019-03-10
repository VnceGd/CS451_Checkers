using DarkRift;
using DarkRift.Server;
using DarkRift.Server.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class GameServerManager : MonoBehaviourSingletonPersistent<GameServerManager>
{

    
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
            x = pNetworkObject.gameObject.transform.position.x,
            y = pNetworkObject.gameObject.transform.position.y
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
    #endregion

}
