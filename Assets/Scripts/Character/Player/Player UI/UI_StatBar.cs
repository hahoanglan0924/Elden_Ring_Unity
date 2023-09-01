using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PH{
public class UI_StatBar : MonoBehaviour
{
  private Slider slider;
  private RectTransform rectTransform;

  [Header("Bar Options")]
  [SerializeField] protected bool scaleBarLengthWithStats = true;
  [SerializeField] protected float widthScaleMultiplayer = 1;
  
  protected virtual void Awake(){
 slider = GetComponent<Slider>();
 rectTransform = GetComponent<RectTransform>();

}

public virtual void SetStats(int newValue){

  slider.value = newValue;

}
public virtual void SetMaxStats(int maxValue){
    slider.maxValue = maxValue;
    slider.value = maxValue;

    if(scaleBarLengthWithStats){
      rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplayer, rectTransform.sizeDelta.y);

      PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
    }
}
}
}