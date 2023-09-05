using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PH{
public class PlayerEffectManager : CharacterEffectManager
{
    [Header("Debug")]
    [SerializeField] InstantCharacterEffect effectTotest;
    [SerializeField] bool processEffect = false;

 private void Update(){
    
    if(processEffect){

        processEffect = false; 
        InstantCharacterEffect effect = Instantiate(effectTotest);
        ProcessInstantEffect(effect);
    }
 
}
}
}