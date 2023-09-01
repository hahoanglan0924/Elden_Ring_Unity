using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PH{
public class UI_Character_Save_Slot : MonoBehaviour
{
    SaveFileDataWriter saveFileDataWriter;

    [Header("Game Slot")]
    public CharacterSlot characterSlot;
    [Header("Character Info")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI timePlayed;

    private void OnEnable(){
        LoadSaveSlots();
    }

    public void SelectCurrentSlot(){
      TitleScreenManager.instance.SelectCharacterSlot(characterSlot);
    }
    public void LoadGameFromCharacterSlot(){
      WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
      WorldSaveGameManager.instance.LoadGame();
    }

    private void LoadSaveSlots(){
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath  = Application.persistentDataPath;

      if(characterSlot == CharacterSlot.CharacterSlot_01){
        saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);  
     
        if(saveFileDataWriter.CheckToSeeIfFileExists()){

            characterNameText.text = WorldSaveGameManager.instance.characterSlots01.characterName;
        }
        
        else{
            gameObject.SetActive(false);
        }
      }
      }


  
}
}
