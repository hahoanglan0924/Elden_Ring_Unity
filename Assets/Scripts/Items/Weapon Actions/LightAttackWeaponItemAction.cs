using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PH{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
public class LightAttackWeaponItemAction : WeaponItemAction
{
    [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";
    public override void AttemptToPerformAction(PlayerManager playerPeformingAction, WeaponItem weaponPerformingAction){
        base.AttemptToPerformAction(playerPeformingAction, weaponPerformingAction);

         if(!playerPeformingAction.IsOwner)
         return;

        if(playerPeformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;
        
        if(!playerPeformingAction.isGrounded)
        return;

        PerformLightAttack(playerPeformingAction, weaponPerformingAction);
        
    }

    private void PerformLightAttack(PlayerManager playerPeformingAction, WeaponItem weaponPerformingAction){
       
        if(playerPeformingAction.playerNetworkManager.isUsingRightHand.Value){
            playerPeformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01,light_Attack_01, true);
        }
        if(playerPeformingAction.playerNetworkManager.isUsingLeftHand.Value){

        }

    }
}
}