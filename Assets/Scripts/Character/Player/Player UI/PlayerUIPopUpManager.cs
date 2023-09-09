using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PH{
public class PlayerUIPopUpManager : MonoBehaviour
{

    [Header("You Died Pop Up")]
   [SerializeField] GameObject youDiedPopUpGameObject;
   [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
   [SerializeField] TextMeshProUGUI youDiedPopUpText;
   [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;

   public void SendYouDiedPopUp(){

    youDiedPopUpGameObject.SetActive(true);
    youDiedPopUpBackgroundText.characterSpacing = 0;
    StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 15f));
    StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup,5));
    StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5)); 


    }

    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount){
        if(duration > 0f){
            text.characterSpacing = 0;
            float timer = 0;
            yield return null;

            while(timer < duration){
                timer += Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvasGroup, float duration){
    
    if(duration > 0){

        canvasGroup.alpha = 0f;
        float timer = 0;

        yield return null;
        while(timer < duration){
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, duration * Time.deltaTime);
                   yield return null;
        }

    }

    
        canvasGroup.alpha = 1f;
        yield return null;
    }

    private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvasGroup, float duration, float delay){
        if(duration > 0){

            while(delay > 0){
                delay = delay -Time.deltaTime;
                yield return null;
            }
        canvasGroup.alpha = 1f;
        float timer = 0;

        yield return null;
        while(timer < duration){
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, duration * Time.deltaTime);
                   yield return null;
        }

    }

    
        canvasGroup.alpha = 0f;
        yield return null;
    }


}
}
