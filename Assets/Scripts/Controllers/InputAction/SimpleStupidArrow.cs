using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleStupidArrow : MonoBehaviour, IPointerClickHandler
{
    

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("SwipeFromArrow");
        GameEvents.Instance.SwipeFromArrow(transform.forward);
    }
}
