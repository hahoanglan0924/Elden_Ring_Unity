using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PH{

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/TakeDamage")]
public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damager")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightingDamage = 0;
    public float holyDamage = 0;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool managuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundFX;

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint;


    [Header("Final Damage")]
    private int finalDamageDealt = 0;
    public override void ProcessEffect(CharacterManager character){
        base.ProcessEffect(character);

        if(character.isDead.Value)
            return;

        CalculateDamage(character);
        
    }

    private void CalculateDamage(CharacterManager character){
    if(character.isDead.Value)
            return;
            
    if(characterCausingDamage != null){
        Debug.Log("Damage");
        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightingDamage + holyDamage);

    if(finalDamageDealt <= 0){
        finalDamageDealt = 1;
    }

    character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
    }

   
    }
}
}