using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PH{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Test Action")]
public class WeaponItemAction : ScriptableObject
{
    public int actionID;

    public virtual void AttemptToPerformAction(PlayerManager playerPeformingAction, WeaponItem weaponPerformingAction){
        //what does every weapon actions have
        if(playerPeformingAction.IsOwner){
            playerPeformingAction.playerNetworkManager.currentWeaponBeingUsed.Value = weaponPerformingAction.itemID;
        }
        Debug.Log("THe action has fired");
    }
}
}