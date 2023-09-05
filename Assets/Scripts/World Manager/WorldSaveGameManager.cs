using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PH{
public class WorldSaveGameManager : MonoBehaviour
{
	public static WorldSaveGameManager instance;

	public PlayerManager player;
	[Header("Save/Load")]
	[SerializeField] bool saveGame;
	[SerializeField] bool loadGame;

	[Header("World Scene Index")]
	[SerializeField] int worldSceneIndex = 1;

	[Header("Save Data Index")]
	private SaveFileDataWriter saveFileDataWriter;

	[Header("Current Character Data")]
	public CharacterSlot currentCharacterSlotBeingUsed;
	public CharacterSaveData currentCharacterData;
	private string saveFileName;
	[Header("Character Slots")]
	public CharacterSaveData characterSlots01;
	public CharacterSaveData characterSlots02;
	public CharacterSaveData characterSlots03;
	public CharacterSaveData characterSlots04;
	public CharacterSaveData characterSlots05;



	private void Awake()
	{
		//THERE CAN ONLY BE ONE INSTANCE OF THIS SCRIPTS AT ONE TIME, IF ANOTHER EXISTS, DESTROYS IT
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
		LoadAllCharacterProfiles();

	}
	private void Update() {
		if(saveGame){
			saveGame = false;
			SaveGame();
		}
		if(loadGame){
		loadGame = false;
		LoadGame();
		}
	}

	public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot){

		string fileName = "";
		switch(characterSlot)
		{
			case CharacterSlot.CharacterSlot_01:
			fileName = "Character_01";
				break;
			case CharacterSlot.CharacterSlot_02:
			fileName = "Character_02";
				break;
				case CharacterSlot.CharacterSlot_03:
			fileName = "Character_03";
				break;
				case CharacterSlot.CharacterSlot_04:
			fileName = "Character_04";
				break;
				case CharacterSlot.CharacterSlot_05:
			fileName = "Character_05";
				break;
		}
		return fileName;
	}

	public void AttemptToCreateNewGame(){

		saveFileDataWriter = new SaveFileDataWriter();
		saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
		
		saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

		if(!saveFileDataWriter.CheckToSeeIfFileExists())
		{
			currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
			currentCharacterData = new CharacterSaveData();
			NewGame();
			return;
		}
		saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

		if(!saveFileDataWriter.CheckToSeeIfFileExists())
		{
			currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
			currentCharacterData = new CharacterSaveData();
			NewGame();
			return;
		}
		saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

		if(!saveFileDataWriter.CheckToSeeIfFileExists())
		{
			currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
			currentCharacterData = new CharacterSaveData();
			NewGame();
			return;
		}
		saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

		if(!saveFileDataWriter.CheckToSeeIfFileExists())
		{
			currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
			currentCharacterData = new CharacterSaveData();
			NewGame();
			return;
		}
		saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

		if(!saveFileDataWriter.CheckToSeeIfFileExists())
		{
			currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
			currentCharacterData = new CharacterSaveData();
			NewGame();
			return;
		}

		TitleScreenManager.instance.DisplayNoFreeCharacterSlotsPopUp();



	}

	private void NewGame(){
		player.playerNetworkManager.vitality.Value = 15;
		player.playerNetworkManager.endurance.Value = 10;
		SaveGame();
		StartCoroutine(LoadWorldScene());
	}

	public void LoadGame(){
		saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

		saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
		saveFileDataWriter.saveFileName = saveFileName;
		currentCharacterData = saveFileDataWriter.LoadSaveFile();

		StartCoroutine(LoadWorldScene());
	}

	public void SaveGame(){

		saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
		Debug.Log(currentCharacterData);

		saveFileDataWriter = new SaveFileDataWriter();
		saveFileDataWriter.saveFileName = saveFileName;

        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

		player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
	
		saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
	}

	public void DeleteGame(CharacterSlot characterSlot){
		saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
		saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

		saveFileDataWriter.DeleteSaveFile();
	}

	private void LoadAllCharacterProfiles(){
		saveFileDataWriter = new SaveFileDataWriter();
		saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

		saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
		characterSlots01 = saveFileDataWriter.LoadSaveFile();

		saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
		characterSlots02 = saveFileDataWriter.LoadSaveFile();

		saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
		characterSlots03 = saveFileDataWriter.LoadSaveFile();
		
		saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
		characterSlots04 = saveFileDataWriter.LoadSaveFile();

		saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
		characterSlots05 = saveFileDataWriter.LoadSaveFile();


	}

	public IEnumerator LoadWorldScene()
	{
		AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
		player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);
		yield return null;
	}


	public int GetWorldSceneIndex(){
		return worldSceneIndex;
	}
}
}