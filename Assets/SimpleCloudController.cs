using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SimpleCloudController : MonoBehaviour
{

    [SerializeField] private GameObject cloudSet1;
    [SerializeField] private GameObject cloudSet2;
    [SerializeField] private float time;
    private Tween cloud1tween;
    private Tween cloud2tween;
    private float x = 0;
    private float middleX = -127; 
    private void OnEnable()
    {
        
        cloud1tween = cloudSet1.transform.DOMoveX(0, 0);
        cloud2tween = cloudSet2.transform.DOMoveX(0, 0);
        cloud1tween.Play();
    }

    private void OnDisable()
    {
        
    }
}
