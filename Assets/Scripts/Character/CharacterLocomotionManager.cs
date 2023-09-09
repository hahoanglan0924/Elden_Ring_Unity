using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PH
{
  public class CharacterLocomotionManager : MonoBehaviour
  {
    CharacterManager character;
    [Header("Ground Check & Jumping")]
    [SerializeField] protected float gravityForce = -5.55f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 1;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundYVelocity = -20;
    [SerializeField] protected float fallStartVelocity = -5;
    protected bool fallingVelocityHasBeenSet = false;
    protected float inAirTimer = 0;
    protected virtual void Awake()
    {
      character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
      HandleGroundCheck();

      if (character.isGrounded)
      {
        if (yVelocity.y < 0)
        {
          inAirTimer = 0;
          fallingVelocityHasBeenSet = false;
          yVelocity.y = groundYVelocity;
        }
      }
      else
      {
        //If we are not jumping, and our falling velocity has not been set
        if (!character.characterNetworkManager.isJumping.Value && !fallingVelocityHasBeenSet)
        {
          fallingVelocityHasBeenSet = true;
          yVelocity.y = fallStartVelocity;
        }
        inAirTimer = inAirTimer + Time.deltaTime;

        yVelocity.y += gravityForce * Time.deltaTime;
        character.animator.SetFloat("inAirTimer", inAirTimer);

        
      }
      character.characterController.Move(yVelocity * Time.deltaTime);
     
    }



    protected void HandleGroundCheck()
    {
      character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }

    protected void OnDrawGizmosSelected()
    {
     // Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }
  }
}