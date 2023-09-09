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
    public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake(){
        base.Awake();
        player = GetComponent<PlayerManager>();
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
}}
