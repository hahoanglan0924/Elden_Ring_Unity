using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PH {

public class PlayerInputManager : MonoBehaviour
{
     // 1. Find a way to read the values of a keyboard
    // 2. Move character based on those values
    [HideInInspector]public static PlayerInputManager instance;
    [HideInInspector]public PlayerManager player;
    PlayerControls playerControls;


    [Header ("Player Movement Input")]
    [SerializeField] Vector2 movementInput;

    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header ("Camera Movement Input")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header ("Player Action Input")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;

    [Header ("Attack")]
    [SerializeField] bool attack01Input = false;


    private void Awake() {
        //When the scene changes, run this logic
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
        
        }
        private void Start() {

                DontDestroyOnLoad(gameObject);
                SceneManager.activeSceneChanged += OnSceneChange;
                    instance.enabled = false;
        
        }

    private void OnSceneChange(Scene oldScene, Scene newScene){

//if we are loading into our world scene, enable our player controls
        if(newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex()){
            instance.enabled = true;
        }else{
            //otherwise we must be at the main menu, disable our player controls
            //this is so our player cant move around if we enter things like character creation meni ect
            instance.enabled = false;
        }

    }

    private void OnEnable() {
        if(playerControls == null){
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerLook.Look.performed += i => cameraInput = i.ReadValue<Vector2>();
            
            //Dodge
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;

            //Sprint
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            //Jump
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;

            //Attack01
            playerControls.PlayerActions.Attack01.performed += i => attack01Input = true;
            
            
    }
    
        playerControls.Enable();


}
private void OnDestroy() {
    //If we destory this object, unsubscibe from this event
    SceneManager.activeSceneChanged -= OnSceneChange;
}

private void OnApplicationFocus(bool focus){
    //If we minimize our lower the window, stop adjusting inputs
    if(enabled){
        if(focus){
            playerControls.Enable();
        }else{
            playerControls.Disable();
        }
    }
}

private void Update() {
 HandelAllInput();
}

private void HandelAllInput(){
    HandleCameraInput();
    HandleMovementInput();
    HandleDodgeInput();
   HandleSprintInput();
   HandleJumpInput();
   HandleAllAttacks();
}

private void HandleAllAttacks(){
HandleAttack01Input();
}

//Movement
private Vector2 smoothedMovementInput;

private void HandleMovementInput() {
    verticalInput = movementInput.y;
    horizontalInput = movementInput.x;

    // Smoothing factor
    float smoothTime = 0.1f;

    // Smooth the movement input
    smoothedMovementInput.x = Mathf.SmoothDamp(smoothedMovementInput.x, horizontalInput, ref smoothedMovementInput.x, smoothTime);
    smoothedMovementInput.y = Mathf.SmoothDamp(smoothedMovementInput.y, verticalInput, ref smoothedMovementInput.y, smoothTime);

    moveAmount = Mathf.Clamp01(Mathf.Abs(smoothedMovementInput.y) + Mathf.Abs(smoothedMovementInput.x));

    if (moveAmount <= 0.5f && moveAmount > 0) {
        moveAmount = 0.5f;
    } else if (moveAmount > 0.5f && moveAmount <= 1) {
        moveAmount = 1;
    }

    if (player == null)
        return;

    player.playerAnimatorManager.UpdateAnimatorManagerParameters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
}

//Camera
private void HandleCameraInput(){
    cameraVerticalInput = cameraInput.y;
    cameraHorizontalInput = cameraInput.x;
    // if(cameraHorizontalInput > 0f) {
    //      player.playerAnimatorManager.UpdateAnimatorManagerParameters(0.5f, 0f,false);
    // }else if(cameraHorizontalInput < -0f) {
    //     player.playerAnimatorManager.UpdateAnimatorManagerParameters(-0.5f, 0,false);
    // }else{
    //       player.playerAnimatorManager.UpdateAnimatorManagerParameters(0, 0,false);
    // }

}

//Dodge
private void HandleDodgeInput(){
    if(dodgeInput){
        dodgeInput = false;

        player.playerLocomotionManager.AttemptToPerformDodge();

    }
}

private void HandleSprintInput(){
    if(sprintInput){
       player.playerLocomotionManager.HandelSprinting(); 
    }else{
        player.playerNetworkManager.isSprinting.Value = false;
    }
}

private void HandleJumpInput(){

    if(jumpInput){
        jumpInput = false;
        
        player.playerLocomotionManager.AttemptToPerformJump();
    }

}

private void HandleAttack01Input(){
    if(attack01Input){
        attack01Input = false;
        player.playerLocomotionManager.AttemptToPerformAttack();
    }
}

}
}