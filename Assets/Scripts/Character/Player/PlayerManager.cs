using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PH{
public class PlayerManager : CharacterManager
{
    [Header("Debug Menu")]
    [SerializeField] bool respawnCharacter = false;
    [SerializeField] bool SwitchRightWeapon = false;
    [HideInInspector]public  PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector]public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
   // [HideInInspector] public Player
    public Transform cameraRoot;

    // public TwoBoneIKConsraint leftHandConstraint;
    // public TwoBoneIKConsraint rightHandConstraint;

    protected override void Awake(){
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        // leftHandConstraint = GetComponentInChildren<TwoBoneIKConsraint>();
        // rightHandConstraint;
            playerNetworkManager.vitality.Value = 15;
            playerNetworkManager.endurance.Value = 10;
    }

    protected override void Update(){
        base.Update();

        if(!IsOwner)
        return;

        playerLocomotionManager.HandleAllMovement();

        playerStatsManager.RegenerateStamina();
    
    }

    override public void OnNetworkSpawn(){
        base.OnNetworkSpawn();

        

    //If this is the player object owned by the client
        if(IsOwner){

            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
            WorldSaveGameManager.instance.player = this;

            playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;

            //Updates UI Stat bars when a stat changes
            playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaReganTimer;      
        }
    //Stats
        playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;
        //Equipment
        playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChanged;
        playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChanged;
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false){

         if(IsOwner){
            PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();
        }

        return base.ProcessDeathEvent(manuallySelectDeathAnimation);

       
    }

    protected override void LateUpdate(){
        if(!IsOwner)
        return;

        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();

        DebugMenu();
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData){
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
        currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;

        currentCharacterData.vitality =playerNetworkManager.vitality.Value;
        currentCharacterData.endurance = playerNetworkManager.endurance.Value;

    }

    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData){
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition + currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = myPosition;

        
        playerNetworkManager.vitality.Value = currentCharacterData.vitality;
        playerNetworkManager.endurance.Value = currentCharacterData.endurance;
        

            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
    }
        private void DebugMenu(){
        if(respawnCharacter){
        respawnCharacter = false;
        ReviveCharacter();
        }
        if(SwitchRightWeapon){
            SwitchRightWeapon = false;
            playerEquipmentManager.SwitchRightWeapon();
        }
    }

    public override void ReviveCharacter() {
        base.ReviveCharacter();

        if(IsOwner){
            playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
            playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;

            playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
        }
    }
}
}