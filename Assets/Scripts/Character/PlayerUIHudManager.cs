using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PH{
public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] UI_StatBar healthBar;
    [SerializeField] UI_StatBar staminaBar;

    public void Start(){
        if(staminaBar == null){
        Debug.Log("Null");
        }
    }

    public void RefreshHUD(){
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);
    }

    
    public void SetNewHealthValue(int oldValue, int newValue){
        healthBar.SetStats(newValue);
    }

    public void SetMaxHealthValue(int maxHealth){
        healthBar.SetMaxStats(maxHealth);
    }

    public void SetNewStaminaValue(float oldValue, float newValue){
        staminaBar.SetStats(Mathf.RoundToInt(newValue));
    }

    public void SetMaxStaminaValue(int maxStamina){
        staminaBar.SetMaxStats(maxStamina);
    }


}
}