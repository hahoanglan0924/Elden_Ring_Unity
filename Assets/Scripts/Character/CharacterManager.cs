using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace PH{
public class CharacterManager : NetworkBehaviour
{
	[Header("Status")]
	public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
	public CharacterController characterController;

	public Animator animator;

	[Header("Flags")]
	public bool isPerformingAction = false;
	public bool isJumping = false;
	public bool isGrounded = true;
	public bool applyRootMotion = false;
	public bool canRotate = true;
	public bool canMove = true;
	


	public CharacterNetworkManager characterNetworkManager;
	[HideInInspector] public CharacterEffectManager characterEffectsManager;
	protected virtual void Awake()
	{
		DontDestroyOnLoad(this);

		characterController = GetComponent<CharacterController>();
		characterNetworkManager = GetComponent<CharacterNetworkManager>();
		animator = GetComponent<Animator>();
		characterEffectsManager = GetComponent<CharacterEffectManager>();
	}

	protected virtual void Update(){

		animator.SetBool("isGrounded", isGrounded);

		//If this character is being controlled from our side, then assin its network position to the position of our transform
		if(IsOwner){
			characterNetworkManager.networkPosition.Value = transform.position;
			characterNetworkManager.networkRotation.Value = transform.rotation;
		}else{
			//If this character is being controlled by else where, then assign its position here locally by the position of its network transform
			//Position
			transform.position = Vector3.SmoothDamp
			(
				transform.position,
				 characterNetworkManager.networkPosition.Value,
				  ref characterNetworkManager.networkPositionVelocity,
				   characterNetworkManager.networkPositionSmoothTime);
			//Rotation
			transform.rotation = Quaternion.Slerp
            (
                transform.rotation,
                 characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);
		}
	}

	protected virtual void LateUpdate() {
		
	}


}
}