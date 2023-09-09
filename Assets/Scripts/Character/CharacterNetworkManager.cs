using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace PH{
public class CharacterNetworkManager : NetworkBehaviour 
{
    CharacterManager character;

    [Header("Position")]
    public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
   
    public Vector3 networkPositionVelocity;
    public float networkPositionSmoothTime = 0.1f;
    public float networkRotationSmoothTime = 0.1f;

    [Header("Animator")]
    public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(0,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected virtual void Awake(){
        character = GetComponent<CharacterManager>();
    }

    public void CheckHP(int oldValue, int newValue){
        if(currentHealth.Value <= 0){
            StartCoroutine(character.ProcessDeathEvent());
        }

        if(character.IsOwner){
            if (currentHealth.Value > maxHealth.Value)
            {
                currentHealth.Value = maxHealth.Value;
            }
    }
    }

    //A server rpc is a function called from a client, to the server(int our case the host)
    [ServerRpc]
    public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion){

        if(IsServer){
            PlayActionAnimationForAllClientRpc(clientID, animationID, applyRootMotion);
        }

    }
    //A client rpc is send to all clients present, from the server
    [ClientRpc]
    public void PlayActionAnimationForAllClientRpc(ulong clientID, string animationID, bool applyRootMotion){
        if(clientID != NetworkManager.Singleton.LocalClientId){
            PerformActionAnimationFromServer(animationID, applyRootMotion);
        }
    }
    //Attack Animation
    private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion){
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(animationID, 0.2f);
    }

    
    [ServerRpc]
    public void NotifyTheServerOfAttackActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion){

        if(IsServer){
            PlayAttackActionAnimationForAllClientRpc(clientID, animationID, applyRootMotion);
        }

    }
    [ClientRpc]
    public void PlayAttackActionAnimationForAllClientRpc(ulong clientID, string animationID, bool applyRootMotion){
        if(clientID != NetworkManager.Singleton.LocalClientId){
            PerformAttackActionAnimationFromServer(animationID, applyRootMotion);
        }
    }

    private void PerformAttackActionAnimationFromServer(string animationID, bool applyRootMotion){
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(animationID, 0.2f);
    }
    //Damage
    [ServerRpc(RequireOwnership = false)]
    public void NotifyTheServerOfCharacterDamageServerRpc(ulong damagedCharacterID,ulong characterCausingDamageID, float physicalDamage, float angleHitFrom, float contactPointX, float contactPointY, float contactPointZ){
        if(IsServer){
            NotifyTheServerOfCharacterDamageClientRpc(damagedCharacterID, characterCausingDamageID, physicalDamage,angleHitFrom, contactPointX, contactPointY, contactPointZ);
        }
    }
    [ClientRpc]
    public void NotifyTheServerOfCharacterDamageClientRpc(ulong damagedCharacterID,ulong characterCausingDamageID, float physicalDamage, float angleHitFrom, float contactPointX, float contactPointY, float contactPointZ){
        ProcessCharacterDamageFromServer(damagedCharacterID, characterCausingDamageID, physicalDamage, angleHitFrom, contactPointX, contactPointY, contactPointZ);
    }

    public void ProcessCharacterDamageFromServer(ulong damagedCharacterID,ulong characterCausingDamageID, float physicalDamage, float angleHitFrom, float contactPointX, float contactPointY, float contactPointZ){
        CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
        CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();
    
        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectManager.instance.takeDamageEffect);

        damageEffect.physicalDamage = physicalDamage;
        damageEffect.angleHitFrom = angleHitFrom;
        damageEffect.contactPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
        damageEffect.characterCausingDamage = characterCausingDamage;

        damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);

    }


    [Header("Flags")]
    public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isJumping = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    [Header("States")]

    [Header("Stats")]
    public NetworkVariable<int> endurance = new NetworkVariable<int>(1,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> vitality = new NetworkVariable<int>(1,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
   
   [Header("Resources")]
    public NetworkVariable<float> currentStamina = new NetworkVariable<float>(0,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxStamina = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxHealth = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

}
}