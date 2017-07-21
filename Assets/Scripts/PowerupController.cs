using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.VR;

public class PowerupController : NetworkBehaviour
{
	[SerializeField]
	private string[] powerupNames;

	private int indexOfChosenPowerupName;

	private Image powerupIcon;
	private Text powerupTextBackground, powerupTextInput;

	public byte showPowerup; 	//0 = no, 1 = increase, 2 = decrease
	public int scoreAmount = 10;
	[SyncVar]
	private bool isPowerupAvailable;

	private ScoreManager scoreManager;

	public float timeBetweenPowerups = 1f;
	public float minTimeBetweenPowerups = 2f, maxTimeBetweenPowerups = 5f;
	private float elapsedTime;


	void Start ()
	{

		if (VRSettings.enabled && VRSettings.isDeviceActive) {
			return;
		}
		powerupIcon = GameObject.FindGameObjectWithTag ("Powerup").GetComponent<Image>();
		var textChildren = powerupIcon.GetComponentsInChildren<Text> ();
		foreach (var t in textChildren) 
		{
			if (t.name.Contains("Background")) 
			{
				powerupTextBackground = t;
			}
			else if (t.name.Contains("Input"))
			{
				powerupTextInput = t;
			}
		}
		Hide ();
		scoreManager = GameObject.FindObjectOfType<ScoreManager> ();

		indexOfChosenPowerupName = 0;
	}
	
	void Update ()
	{
		if (VRSettings.enabled && VRSettings.isDeviceActive) {
			return;
		}
		if (!isPowerupAvailable)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= timeBetweenPowerups)
			{
				Debug.Log ("Heio");
				isPowerupAvailable = true;
				elapsedTime = 0f;
				timeBetweenPowerups = Random.Range (minTimeBetweenPowerups, maxTimeBetweenPowerups);
				indexOfChosenPowerupName = Random.Range (0, powerupNames.Length);
				RpcShowPossiblePowerup (powerupNames[indexOfChosenPowerupName], showPowerup == 1 ? showPowerup = 2 : showPowerup = 1);
			}
			return;
		}

		if (Input.GetKeyUp(KeyCode.A)) 
		{
			UpdateInput ("A");
		}
		else if (Input.GetKeyUp(KeyCode.W))
		{
			UpdateInput ("W");
		}
		else if (Input.GetKeyUp(KeyCode.S))
		{
			UpdateInput ("S");
		}
		else if (Input.GetKeyUp(KeyCode.D))
		{
			UpdateInput ("D");
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			UpdateInput (" ");
		}
	}

	private void UpdateInput(string letter)
	{
		string newWord = (powerupTextInput.text + letter).ToLower();
		if (powerupNames[indexOfChosenPowerupName].Equals(newWord))
		{
			powerupTextInput.text = newWord;
			CmdTakePowerup (scoreManager.playerId);
		} 
		else if (powerupNames[indexOfChosenPowerupName].StartsWith(newWord))
		{
			powerupTextInput.text = newWord;
		}
		else
		{
			powerupTextInput.text = "";
		}
	}

	[Command]
	public void CmdTakePowerup(NetworkInstanceId playerId)
	{
		if (!isPowerupAvailable)
		{
			return;
		}

		isPowerupAvailable = false;
		scoreManager.CmdIncreaseScoreBy (showPowerup == 1 ? scoreAmount : -scoreAmount, playerId);
		RpcHidePossiblePowerup ();
	}

	[ClientRpc]
	public void RpcShowPossiblePowerup(string name, byte showPowerup)
	{
		powerupTextBackground.text = name;
		powerupTextInput.text = "";
		powerupIcon.color = showPowerup == 1 ? Color.green : Color.red;
		powerupIcon.gameObject.SetActive (true);
	}

	[ClientRpc]
	public void RpcHidePossiblePowerup()
	{
		Debug.Log ("RpcHide");
		powerupIcon.gameObject.SetActive (false);
	}

	void Hide() 
	{
		Debug.Log ("Hide");
		powerupIcon.gameObject.SetActive (false);
	}

}

