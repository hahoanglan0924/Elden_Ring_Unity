using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
	public static PlayerUIManager Instance;

	[Header("NETWORK JOIN")]
	
	[SerializeField] 
	bool startGameAsClient;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(Instance);
		}
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
