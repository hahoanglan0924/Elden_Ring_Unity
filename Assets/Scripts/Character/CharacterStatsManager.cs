using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PH{
public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;
    [Header("Stamina Regeneration")]
	[SerializeField] float staminaRegenerationAmount = 0.1f;
	private float staminaRegenerationTimer = 0;
	private float staminaTickTimer = 0;
	[SerializeField] float regenerationDelay = 4;

    protected virtual void Awake() {
        character = GetComponent<CharacterManager>();
    }

	protected virtual void Start(){
		
	}

    public int CalculateStaminaBasedOnEnduranceLevel(int endurance){
        float stamina =0;

        stamina = endurance * 10;
        return Mathf.RoundToInt(stamina);
    } 

	 public int CalculateHealthBasedOnVitalityLevel(int vitality){
        float health =0;

        health = vitality * 10;
        return Mathf.RoundToInt(health);
    } 

    	public virtual void RegenerateStamina(){
		if(!character.IsOwner)
		return;

		if(character.characterNetworkManager.isSprinting.Value)
			return;

		staminaRegenerationTimer += Time.deltaTime;

		if(staminaRegenerationTimer >= regenerationDelay){
			if(character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value){
				staminaTickTimer = staminaTickTimer + Time.deltaTime;

				if(staminaTickTimer >= 0.1){
					staminaTickTimer = 0;
					character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount ;
				}
			}
		}

		
		
	}

    public virtual void ResetStaminaReganTimer(float previousStaminaAmount, float currentStaminaAmount){

        if(currentStaminaAmount < previousStaminaAmount){
        staminaRegenerationTimer = 0;
    }
    }
}
}