using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace PH{
public class PlayerCombatManager : CharacterCombatManager
{
    
    public WeaponItem currentWeaponBeingUsed;

    PlayerManager player;
    protected override void Awake(){
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPeformingAction){
        if(player.IsOwner){
                  //Perform the action
weaponAction.AttemptToPerformAction(player,weaponPeformingAction);
 //notify the server we have performed the action, so we perofrm it from there perspective also
 player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId,weaponAction.actionID, weaponPeformingAction.itemID);
        }
  
        
       
    }

    public virtual void DrainStaminaBasedOnAttack(){
        if(!player.IsOwner)
        return;

        if(currentWeaponBeingUsed == null)
        return;

        float staminaDeducted = 0;

        switch(currentAttackType){
            case AttackType.LightAttack01:
            staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplayer;
            break;
            default:
            break;
        }

        player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
    }
}
}