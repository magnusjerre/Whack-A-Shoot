using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class MainMenuScript : MonoBehaviour {

	[SerializeField]
	private GameObject vrStuffPrefab, nvrStuffPrefab;

	private GameObject vrStuffInstance, nvrStuffInstance;
	private MJNetworkManager networkManager;

	private Text myIpText;

	//NVR Canvas info
	private InputField nvrIpInput;	//Searched for by looking for a single input field in hierarchy
	private Button nvrConnectButton, nvrHostButton;	//Searched for by looking for a single button in hierarchy
	private Text nvrConnectButtonText;
	private bool nvrIsConnecting = false;

	void Awake() {
		if (VRSettings.enabled && VRSettings.isDeviceActive) {
			vrStuffInstance = Instantiate(vrStuffPrefab);
			Text[] allTextComponents = vrStuffInstance.GetComponentsInChildren<Text>();
			foreach (Text text in allTextComponents) {
				if (text.name.Equals("MyIPText")) {
					myIpText = text;
					myIpText.text = MyIP();
					break;
				}
			}
		} else {
			nvrStuffInstance = Instantiate(nvrStuffPrefab);
			nvrIpInput = nvrStuffInstance.GetComponentInChildren<InputField>();
			var buttons = nvrStuffInstance.GetComponentsInChildren<Button>();
			for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                if (button.name.Contains("Host"))
                {
					button.onClick.AddListener(delegate { Host(); });
					Debug.Log("Host button found");
                }
                else if (button.name.Contains("Button"))
                {
                    nvrConnectButton = button;
                    nvrConnectButton.onClick.AddListener(delegate { ConnectToHost(); });

                    nvrConnectButtonText = nvrConnectButton.GetComponentInChildren<Text>();
                    Text[] allTextComponents = nvrStuffInstance.GetComponentsInChildren<Text>();
                    foreach (Text text in allTextComponents)
                    {
                        if (text.name.Equals("MyIPText"))
                        {
                            myIpText = text;
                            myIpText.text = MyIP();
                            break;
                        }
                    }
                }
            }

        }
		networkManager = GameObject.FindObjectOfType<MJNetworkManager>();		
	}

	void Start () {
		if (nvrStuffInstance != null) 
		{
			nvrIpInput.onValueChanged.AddListener(delegate { IPValueChange(); });
			int lastDot = MyIP().LastIndexOf(".");
			nvrIpInput.text = MyIP().Substring(0, lastDot + 1);
		}
	}

	public void ConnectToHost() {
		if (nvrIsConnecting)
        {
			Debug.Log("Disconnecting from : " + nvrIpInput.text);
            networkManager.StopClient();
            nvrConnectButtonText.text = "Connect";
            nvrIsConnecting = false;
        }
        else
        {
            Debug.Log("Trying to connecto to : " + nvrIpInput.text);
            networkManager.networkAddress = nvrIpInput.text;
            networkManager.StartClient();
            nvrConnectButton.GetComponentInChildren<Text>().text = "Connecting... Click to disconnect";
			nvrIsConnecting = true;
        }
	}

	public void OnClientError()
    {
		if (nvrStuffInstance != null)
        {
            nvrIsConnecting = false;
            nvrConnectButtonText.text = "Connect";
        }
    }

    public void OnClientDisconnect()
    {
		if (nvrStuffInstance != null)
        {
            nvrIsConnecting = false;
            nvrConnectButtonText.text = "Connect";
        }
    }

	private void IPValueChange() {
		bool isMatch = Regex.IsMatch(nvrIpInput.text, "^\\d{1,3}.\\d{1,3}.\\d{1,3}.\\d{1,3}$");
		if (isMatch) {
			nvrConnectButton.interactable = true;
		} else {
			nvrConnectButton.interactable = false;
		}
	}

	private string MyIP() {
		return Network.player.ipAddress;
	}

	public void Host() {
		Debug.Log("Hosting game at: " + MyIP());
		networkManager.StartHost();
	}
}
