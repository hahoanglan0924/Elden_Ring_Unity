using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace PH{
public class TitleScreenManager : MonoBehaviour
{
	public static TitleScreenManager instance;
	[Header("Menus")]
	[SerializeField] GameObject titleScreenMainMenu;
	[SerializeField] GameObject titleScreenLoadMenu;
	
	[Header("Buttons")] 
	[SerializeField] Button mainMenuNewGameButton;
	[SerializeField] Button loadMenuReturnButoon;
	[SerializeField] Button mainMenuLoadGameButton;
	[SerializeField] Button deleteCharacterPopUpConfirmButton;

	[Header("Pop Ups")]
	[SerializeField] GameObject noCharactersSlotsPopUp;
	[SerializeField] Button noCharactersSlotsOkeyButton;
	[SerializeField] GameObject deleteCharacterSlotPopUp;

	[Header("Save Slots")]
	public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLot;


	[Header("Title Screen Inputs")]
	[SerializeField] bool deleteCharacterSlot = false;

	private void Awake() {
		if(instance == null) {
			instance = this;
        } else {
			Destroy(gameObject);
		}
	}

	public void StartNetworkAsHost()
	{
		NetworkManager.Singleton.StartHost();
	}

	public void StartNewGame()
	{
		WorldSaveGameManager.instance.AttemptToCreateNewGame();
		//StartCoroutine(WorldSaveGameManager.instance.LoadWorldScene());
	}

	public void OpenLoadGameMenu(){
		titleScreenLoadMenu.SetActive(true);
		titleScreenMainMenu.SetActive(false);
		loadMenuReturnButoon.Select();
	}

	public void CloseLoadGameMenu(){
		titleScreenLoadMenu.SetActive(false);
		titleScreenMainMenu.SetActive(true);
		mainMenuLoadGameButton.Select();
	}

	public void DisplayNoFreeCharacterSlotsPopUp(){
		noCharactersSlotsPopUp.SetActive(true);
		noCharactersSlotsOkeyButton.Select();
	}	
	
	public void CloseNoFreeCharacterSlotsPopUp(){
        noCharactersSlotsPopUp.SetActive(false);
		mainMenuNewGameButton.Select();
    }

	public void SelectCharacterSlot(CharacterSlot characterSlot){
	currentSelectedSlot = characterSlot;
	}

	public void AttemptToDeleteCharacterSlot(){
		if(currentSelectedSlot != CharacterSlot.NO_SLot){
		deleteCharacterSlotPopUp.SetActive(true);
		deleteCharacterPopUpConfirmButton.Select();
		}
	}

	public void DeleteCharacterSlot(){
		deleteCharacterSlotPopUp.SetActive(false);
		WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);
		titleScreenLoadMenu.SetActive(false);
		titleScreenLoadMenu.SetActive(true);
		loadMenuReturnButoon.Select();

	}

	public void CloseDeleteCharacterSlotPopUp(){
        deleteCharacterSlotPopUp.SetActive(false);
        loadMenuReturnButoon.Select();
    }

	public void SelectNoSlop(){
		currentSelectedSlot = CharacterSlot.NO_SLot;
	}
}
}