using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PH
{
        public class WeaponItem : Item
{
   [Header("Weapon Models")]
   public GameObject weaponModel;

   [Header("Weapon Requirements")]
   public int strengthREQ = 0;
   public int dexREQ = 0;
   public int intREQ = 0;
   public int faithREQ = 0;

   [Header("Weapon Base Damage")]
   public int physicalDamage = 0;
   public int magicDamage = 0;
   public int fireDamage = 0;
   public int holyDamage = 0;
   public int lightingDamage = 0;

   [Header("Stamina Costs")]
   public int baseStaminaCost = 20;
   public float lightAttackStaminaCostMultiplayer = 0.9f;
   [Header("Attack modifier")]
   public float light_attack_01_modifier = 1.1f;

   [Header("Weapon Poise")]
   public float poiseDamage = 10;

   [Header("Actions")]
   public WeaponItemAction oh_RB_Action;

}
}