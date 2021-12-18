using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventHolder : MonoBehaviour
{
    [SerializeField] private GameObject _listner;

    public void EndAttack()
    {
        _listner.SendMessage("EndAttack", options:SendMessageOptions.DontRequireReceiver);
    }

}

public interface IAnimatorEventListner 
{
    void EndAttack();
    
}
