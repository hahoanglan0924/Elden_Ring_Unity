using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace PH{
public class PlayerUIManager : MonoBehaviour
{
	public static PlayerUIManager instance;

	[Header("NETWORK JOIN")]
	
	[SerializeField] 
	bool startGameAsClient;

	[HideInInspector] public PlayerUIHudManager playerUIHudManager;
	[HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;

	private void Awake()
	{

		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(instance);
		}

		playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
		playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
	}

	private void Update()
	{
		if (startGameAsClient)
		{
			startGameAsClient = false;
			// WE MUST FIRST SHUT DOWN , BECAUSE WE HAVE STARTED AS A HOST DURING THE TITLE SCREEN
			NetworkManager.Singleton.Shutdown();
			// WE THEN RESTARTM AS A CLIENT
			NetworkManager.Singleton.StartClient();
		}
	}

	private void Start()
	{
		DontDestroyOnLoad (gameObject);
	}
}
}