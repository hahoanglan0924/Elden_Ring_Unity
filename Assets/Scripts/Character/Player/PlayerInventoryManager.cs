using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PH{
public class PlayerInventoryManager : CharacterInventoryManager
{
  public WeaponItem currentRighthandWeapon;
  public WeaponItem currentLefthandWeapon;

  [Header("Quick Slots")]
  public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
  public int rightHandWeaponIndex = 0;
  public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];
    public int leftHandWeaponIndex = 0;
}
}