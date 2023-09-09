using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PH{
public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Character")]
    public CharacterManager characterCausingDamage;
    [Header("Weapon Attack modifier")]
    public float light_attack_01_modifier;

    protected override void Awake(){
        base.Awake();

        if(damageCollider == null)
        {
            damageCollider = GetComponent<Collider>();
        }
        damageCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other){
         CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

       

        if(damageTarget != null) 
        {
              if(damageTarget == characterCausingDamage)
         return;
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            DamageTarget(damageTarget);
        }
    }

    protected override void DamageTarget(CharacterManager damageTarget){
       if(charactersDamaged.Contains(damageTarget))
        return;

        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.lightingDamage = lightingDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.contactPoint = contactPoint;

       // damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
       switch(characterCausingDamage.characterCombatManager.currentAttackType){
        case AttackType.LightAttack01:
        ApplyAttackDamageModifiers(light_attack_01_modifier, damageEffect);
        break;
        default:
        break;
       }

       if(characterCausingDamage.IsOwner){
        damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId, characterCausingDamage.NetworkObjectId, damageEffect.physicalDamage, damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);
       }

    }

    private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage){
        damage.physicalDamage *= modifier;
        
        //if attack is a fully charged heavy, mutliply by ful charge modifer after normal modifer have been calculated
    }


}
}