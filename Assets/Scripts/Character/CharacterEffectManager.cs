using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PH{
public class CharacterEffectManager : MonoBehaviour
{
    CharacterManager character;

    protected virtual void Awake() {
        character = GetComponent<CharacterManager>();
        
    }
   public virtual void ProcessInstantEffect(InstantCharacterEffect effect){
    Debug.Log("Proceed");
    effect.ProcessEffect(character);
   }
}
}