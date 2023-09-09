using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PH {
    public class PlayerLocomotionManager : CharacterLocomotionManager {
        PlayerManager player;

        [Header ("Movement Settings")]
        public float verticalMovement;
        public float horizontalMovement;

        public float moveAmount;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float sprintingSpeed = 6.5f;
        [SerializeField] private int sprintingStaminaCost = 2;

        [Header("Aim")]
        [SerializeField] private Transform aimTarget;
        [SerializeField] private float aimDistance = 1f;

        private Vector3 moveDirection;

        [SerializeField] float rotationSpeed = 5;

        [Header ("Dodge")]
        private Vector3 rollDirection;

        [SerializeField] float dodgeStaminaCost = 10;

        [Header("Jump")]
        [SerializeField] float jumpStaminaCost = 25; 
        [SerializeField] float jumpHeight = 4f;
        [SerializeField] float jumpForwardVelocity = 4;
        [SerializeField] float freeFallVelocity = 3;
        private Vector3 jumpDirection;


        protected override void Awake() {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update() {
            aimTarget.position = PlayerCamera.instance.transform.position + PlayerCamera.instance.transform.forward * aimDistance;
            base.Update();
            if(player.IsOwner){
                player.characterNetworkManager.verticalMovement.Value = verticalMovement;
                player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            }else{
                verticalMovement = player.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;

                player.playerAnimatorManager.UpdateAnimatorManagerParameters(horizontalMovement,verticalMovement, player.playerNetworkManager.isSprinting.Value);
            }
        }

        private void GetVerticalAndHorizontalInput() {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
        }

        public void HandleAllMovement() {
            HandleGroundedMovement();
            HandleRotation();
            HandleFreeFallMovement();
            HandleJumpingMovement();
        }

       private void HandleGroundedMovement() {

        if(!player.canMove)
        return;
            GetVerticalAndHorizontalInput();

            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if(player.playerNetworkManager.isSprinting.Value){
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }else{
             if (PlayerInputManager.instance.moveAmount > 0.5f) {
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            } else if (PlayerInputManager.instance.moveAmount <= 0.5f) {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
            }

           
        }

        private void HandleJumpingMovement(){
            if(player.playerNetworkManager.isJumping.Value) {
            player.characterController.Move(jumpDirection * jumpForwardVelocity * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement(){
            if(!player.isGrounded){
                Vector3 freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * freeFallVelocity * Time.deltaTime);
            }
        }

        private void HandleRotation() {
        if(!player.canRotate)
        return;
            float targetAngle = PlayerCamera.instance.transform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        public void HandelSprinting(){

            if(player.playerNetworkManager.currentStamina.Value <= 0){
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }
            

           if(verticalMovement >= 1f) {

            player.playerNetworkManager.isSprinting.Value = true;

        }else{
            player.playerNetworkManager.isSprinting.Value = false;
        }
        if(player.playerNetworkManager.isSprinting.Value) {
           
        player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
         }
}

public void AttemptToPerformDodge(){
    if(player.isPerformingAction)
    return;
    if(PlayerInputManager.instance.moveAmount > 0){
 rollDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
 rollDirection += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;

 rollDirection.y = 0;
 rollDirection.Normalize();

 Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
 player.transform.rotation = playerRotation;

 player.playerAnimatorManager.PlayTargetActionAnimation("Roll", true, true);

 player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;

    }

}

public void AttemptToPerformJump(){
    if(player.isPerformingAction)
    return;

    if(player.playerNetworkManager.currentStamina.Value <= 0)
    return;

    if(player.playerNetworkManager.isJumping.Value)
    return;

    if(!player.isGrounded)
    return;

    player.playerAnimatorManager.PlayTargetActionAnimation("Jump_start", false, false, true, false);
    yVelocity.y = Mathf.Sqrt(-2 * jumpHeight * gravityForce);

    player.playerNetworkManager.isJumping.Value = true;

    player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

    jumpDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
    jumpDirection += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;

    jumpDirection.y = 0;

    if(jumpDirection != Vector3.zero){
          if(player.playerNetworkManager.isSprinting.Value) {
    jumpDirection *=1;
    }else if(PlayerInputManager.instance.moveAmount > 0.5f) {
        jumpDirection *= 0.5f;
    }else if(PlayerInputManager.instance.moveAmount <= 0.5f) {
    jumpDirection *= 0.25f;
    }
    }
  
}

public void ApplyJumpingVelocity(){
   

}

public void AttemptToPerformAttack(){
    if(player.isPerformingAction)
    return;
        if(player.playerNetworkManager.isJumping.Value)
    return;

   // player.playerAnimatorManager.PlayTargetActionAnimation("Attack01", false,true, true);
}




}
}