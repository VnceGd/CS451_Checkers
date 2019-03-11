using DarkRift;
using UnityEngine;
public class SyncMessageModel : IDarkRiftSerializable
{
    #region Properties
    public int networkID;
    public int serverTick;
    public int x;
    public int y;

    #endregion
    #region IDarkRiftSerializable implementation
    public void Deserialize(DeserializeEvent e)
    {
        networkID = e.Reader.ReadInt32();
        serverTick = e.Reader.ReadInt32();
        x =  e.Reader.ReadInt32();
        y = e.Reader.ReadInt32();

    }
    public void Serialize(SerializeEvent e)
    {
        //Write id of the network object
        e.Writer.Write(networkID);
        //Write id of the network object
        e.Writer.Write(serverTick);
        //Write position
        e.Writer.Write(x);
        e.Writer.Write(y);


    }
    #endregion
}