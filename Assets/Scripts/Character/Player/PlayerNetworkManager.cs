using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

namespace PH{
public class PlayerNetworkManager : CharacterNetworkManager
{
    public  NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    PlayerManager player;

    [Header("Equipment")]
     public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake(){
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public void SetCharacterActionHand(bool rightHandedAction){
        if(rightHandedAction){
            isUsingLeftHand.Value = false;
            isUsingRightHand.Value = true;
        }else{
            isUsingLeftHand.Value = true;
            isUsingRightHand.Value = false;
        }

    }
    public void SetNewMaxHealthValue(int oldVitality, int newVitality){
        maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
        PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);
        currentHealth.Value = maxHealth.Value;
    }
      public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance){
        maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
        PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
        currentStamina.Value = maxStamina.Value;
    }

    public void OnCurrentRightHandWeaponIDChanged(int oldID, int newID){
    WeaponItem newWeapon = Instantiate(WorldItemDataBase.instance.GetWeaponByID(newID));
    player.playerInventoryManager.currentRighthandWeapon = newWeapon; 
    player.playerEquipmentManager.LoadRightWeapon();
    }

    public void OnCurrentLeftHandWeaponIDChanged(int oldID, int newID){
    WeaponItem newWeapon = Instantiate(WorldItemDataBase.instance.GetWeaponByID(newID));
    player.playerInventoryManager.currentLefthandWeapon = newWeapon; 
    player.playerEquipmentManager.LoadLeftWeapon();
    }

     public void OnCurrentWeaponBeingUsedIDChanged(int oldID, int newID){
    WeaponItem newWeapon = Instantiate(WorldItemDataBase.instance.GetWeaponByID(newID));
    player.playerCombatManager.currentWeaponBeingUsed = newWeapon;
    player.playerEquipmentManager.LoadLeftWeapon();
    }

    //Item actions
    [ServerRpc]
    public void NotifyTheServerOfWeaponActionServerRpc(ulong clientID, int actionID, int weaponID){ 
        if(IsServer){
            NotifyTheServerOfWeaponActionClientRpc(clientID, actionID, weaponID);
        }
    }
    [ClientRpc]
    public void NotifyTheServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID){
        if(clientID != NetworkManager.Singleton.LocalClientId){
            PlayWeaponAction(actionID, weaponID);
        }
    }

    private void PlayWeaponAction(int actionID, int weaponID){
        WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);
        if(weaponAction != null){
            weaponAction.AttemptToPerformAction(player, WorldItemDataBase.instance.GetWeaponByID(weaponID));
        }
        else{
            Debug.LogError("Action is null.");
        }
    }
}}
