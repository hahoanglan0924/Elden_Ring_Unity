using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PH{
public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;
    public WeaponModelInstantiationLocation rightHandSlot;
    public WeaponModelInstantiationLocation leftHandSlot;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;

    protected override void Awake() {
        base.Awake();
        
        player = GetComponentInParent<PlayerManager>();

        InitializeWeaponSlots();
    }
    protected override void Start(){
        base.Start();

        LoadRightWeapon();
    }
private void InitializeWeaponSlots()
{
    WeaponModelInstantiationLocation[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationLocation>();

    foreach (var weaponSlot in weaponSlots)
    {
        if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
        {
            rightHandSlot = weaponSlot;
        }
        else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
        {
            leftHandSlot = weaponSlot;
        }
    }
}

    public void LoadWeaponOnBothHands(){
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    public void LoadRightWeapon(){
        if(player.playerInventoryManager.currentRighthandWeapon != null){
            //Remove the old weapon
            rightHandSlot.UnLoadWeapon();
            //Load the new weapon
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRighthandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRighthandWeapon);

        }
    }

    public void SwitchRightWeapon(){
        if(!player.IsOwner)
            return;

        player.playerAnimatorManager.PlayTargetActionAnimation("Equip", false, true, true, true);

        WeaponItem selectedWeapon = null;

        //Add one to our index to switch next potential weapon
        player.playerInventoryManager.rightHandWeaponIndex += 1;

        //If our index is out of bounds, reset it to position (0)
        if(player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
        {
            player.playerInventoryManager.rightHandWeaponIndex = 0;

            //We check if we are holding more than one weapon
              float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for(int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++){
                if(player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDataBase.instance.unarmedWeapon.itemID){
                weaponCount++;

                if(firstWeapon == null){
                    firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                    firstWeaponPosition = i;
                }
                }  
            }
            if(weaponCount <= 1){
                player.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedWeapon = WorldItemDataBase.instance.unarmedWeapon;
                player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
            }else{
                player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
            }

            return;
        }
        

        foreach(WeaponItem item in player.playerInventoryManager.weaponsInRightHandSlots)
        {
            //If the next ponential weapon does not equal the unarmed weapon
            if(player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDataBase.instance.unarmedWeapon.itemID){
                selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                //Assign the network weapon id so it switches for all connected clients
                player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                return;
            }

        }

        if(selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2){
            SwitchRightWeapon();
        }
    }

    public void LoadLeftWeapon(){
           if(player.playerInventoryManager.currentLefthandWeapon != null){
            leftHandSlot.UnLoadWeapon();
            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLefthandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLefthandWeapon);
        }
    }
}
}