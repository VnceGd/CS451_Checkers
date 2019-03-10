using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class ClientManager : MonoBehaviourSingletonPersistent<ClientManager>
{



    /// <summary>
    /// Reference to the DarkRift2 client
    /// </summary>
    public UnityClient clientReference;
    public string address = "127.0.0.1";
    public int port = 4296;


 

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
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);


        clientReference.MessageReceived += SpawnGameObjects;
        //////////////////
        /// Connect to the server manually
        clientReference.ConnectInBackground(
            IPAddress.Parse(address),
            port,
            DarkRift.IPVersion.IPv4,
            null
            );

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
            Instantiate(go, new Vector3(spawnMessage.x, spawnMessage.y, 0), Quaternion.identity);
        }
    }
}