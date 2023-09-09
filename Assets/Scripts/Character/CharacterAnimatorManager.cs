using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
//using UnityEngine.Animation.Rigging;

namespace PH{
public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager characterManager;
    public float smoothBlend = 0.1f;
    int verticalHash;
    int horizontalHash;
   // bool rightPose = true;

    // protected RigBuilder rigBuilder;

    // public TwoBoneIKConsraint leftHandConstraint;
    // public TwoBoneIKConsraint rightHandConstraint;

    protected virtual void Awake(){
        characterManager = GetComponent<CharacterManager>();

        verticalHash = Animator.StringToHash("Vertical");
        horizontalHash = Animator.StringToHash("Horizontal");
      //  rigBuilder = GetComponent<RigBuilder>();
    }
    public void UpdateAnimatorManagerParameters(float horizontalValue, float verticalValue, bool isSprinting){

        float horizontal = horizontalValue;
        float vertical = verticalValue;
       if(isSprinting){
        vertical = 2f;
       }
        characterManager.animator.SetFloat(horizontalHash, horizontal,smoothBlend,Time.deltaTime);
        characterManager.animator.SetFloat(verticalHash, vertical,smoothBlend,Time.deltaTime);
    }
    
    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false){
        characterManager.applyRootMotion = applyRootMotion;
        characterManager.animator.CrossFade(targetAnimation, 0.15f);
        characterManager.isPerformingAction = isPerformingAction;
        characterManager.canMove = canMove; 
        characterManager.canRotate = canRotate;

        characterManager.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }
      public virtual void PlayTargetAttackActionAnimation(AttackType attackType, string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = true, bool canMove = true){
      
        characterManager.characterCombatManager.currentAttackType = attackType;
        characterManager.applyRootMotion = applyRootMotion;
        characterManager.animator.CrossFade(targetAnimation, 0.15f);
        characterManager.isPerformingAction = isPerformingAction;
        characterManager.canMove = canMove; 
        characterManager.canRotate = canRotate;

        characterManager.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

    // public virtual void TurnPose(){
    //     characterManager.animator.SetBool("RightPose", !rightPose);
    // }

    // public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandIKTarget, LeftHandIKTarget leftHandIKTarget, bool isTwoHandingWeapons){
    //     if(isTwoHandingWeapons){
    //         rightHandConstraint.data.target = rightHandIKTarget.transform;
    //         rightHandConstraint.data.targetPositionWeight = 1f;
    //         rightHandConstraint.data.targetRotationWeight = 1f;

    //         leftHandConstraint.data.target = leftHandIKTarget.transform;
    //         leftHandConstraint.data.targetPositionWeight = 1f;
    //         leftHandConstraint.data.targetRotationWeight = 1f;
    //     }else{
    //         rightHandConstraint.data.target = null;
    //         leftHandConstraint.data.target = null;
    //     }

    //     rigBuilder.Build();
    // }

    // public virtual void EraseHandIKForWeapon(){
    
    // }


    
    
    
    }
    
}

