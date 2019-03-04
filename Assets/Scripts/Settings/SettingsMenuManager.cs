using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    private GameManager _gm;

    [Header("Settings Placeholder Elements")]
    public Text PlayerNameTextPL;
    public Text RoomNameTextPL;
    public Text NetworkAddrTextPL;
    public Text NetworkPortTextPL;
    public Text MssgSizeTextPL;
    public Text CombinedMsgSizeTextPL;

    [Header("Settings Value Elements")]
    public Text PlayerNameText;
    public Text RoomNameText;
    public Text NetworkAddrText;
    public Text NetworkPortText;
    public Text MssgSizeText;
    public Text CombinedMsgSizeText;

    [Header("Error Handling")]
    public GameObject ErrorPopup;
    public Text ErrorPopupText;

    void OnDisable()
    {
        this.UpdateSettingsFile();    
    }

    void UpdateSettingsFile()
    {
        if(_gm != null)
        {
            if (!_gm.SaveChanges())
            {
                //show popup on fail
                ErrorPopup.SetActive(true);
                ErrorPopupText.text = "An error has occured while saving the settings.";
            }
        }
    }
    public void UpdateSettingsGUI()
    {
        if (_gm != null)
        {
            PlayerNameTextPL.text = _gm.settings.PlayerName;
            RoomNameTextPL.text = _gm.settings.RoomName;
            NetworkAddrTextPL.text = _gm.settings.DefaultNetworkAddress;
            NetworkPortTextPL.text = _gm.settings.DefaultNetworkPort;
            MssgSizeTextPL.text = _gm.settings.MaxMessageQueueSize.ToString();
            CombinedMsgSizeTextPL.text = _gm.settings.MaxCombinedReliableMessageSize.ToString();
        }
        else
        {
            PlayerNameTextPL.text = "???";
            RoomNameTextPL.text = "???";
            NetworkAddrTextPL.text = "???";
            NetworkPortTextPL.text = "???";
            MssgSizeTextPL.text = "???";
            CombinedMsgSizeTextPL.text = "???";
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        _gm = GameManager.instance;
        Debug.Log(_gm);
    }
    void OnEnable()
    {
        UpdateSettingsGUI();
    }
    public void UpdatePlayerName()
    {
        _gm.SetPlayerName(PlayerNameText.text);
    }
    public void UpdateRoomName()
    {
        _gm.SetRoomName(RoomNameText.text);
    }
    public void UpdateNetworkAddress()
    {
        _gm.SetNetworkAddress(NetworkAddrText.text);
    }
    public void UpdateNetworkPort()
    {
        _gm.SetNetworkPort(NetworkPortText.text);
    }
    public void UpdateMessageSize()
    {
        _gm.SetMessageSize(MssgSizeText.text);
    }
    public void UpdateCombinedMessagSize()
    {
        _gm.SetCombinedMessageSize(CombinedMsgSizeText.text);
    }
}
