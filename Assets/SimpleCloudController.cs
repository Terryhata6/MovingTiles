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
    [SerializeField] private bool needMoveClouds;
    

    private void Start()
    {
        
    }

    private IEnumerator MovingClouds()
    {
        int gater = 0;
        Vector3 basePos1 = cloudSet1.transform.position;
        Vector3 basePos2 = cloudSet2.transform.position;
        while (needMoveClouds) {
            
            gater = 1;
            cloudSet1.transform.DOMoveX(270, time).OnComplete(() =>
            {
                gater = 0;
                cloudSet1.transform.position = basePos1;
            }); 
            yield return new WaitUntil(() =>  gater.Equals(0));
            gater = 1;
            cloudSet2.transform.DOMoveX(270, time).OnComplete(() =>
            {
                gater = 0;
                cloudSet2.transform.position = basePos2;
            });
            yield return new WaitUntil(() =>  gater.Equals(0));
        }
    }
    private void OnEnable()
    {
        StartCoroutine(MovingClouds());
    }

    private void OnDisable()
    {
        StopCoroutine(MovingClouds());
    }
}
