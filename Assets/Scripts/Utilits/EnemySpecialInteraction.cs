using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemySpecialInteraction : MonoBehaviour
{
    [SerializeField] private Transform _interactionTarget;
    [SerializeField] private Transform _interactionTarget2;
    [SerializeField] private string _interactionConfig;

    private float value;
    public void CallSpecialInteraction()
    {
        switch (_interactionConfig)
        {
            case "Mimic":
                value = Random.Range(-35f, -55f);
                _interactionTarget2.transform.DOLocalMove(Vector3.forward* 0.2f, 0.2f);
                _interactionTarget.DORotate(new Vector3(0, value, 0), 0.3f).OnComplete(
                    () =>
                    {
                        _interactionTarget.DORotate(new Vector3(0, -value, 0), 0.2f);
                    });
                //TODO FeedBack OpeningDoor
                break;
            default:
                break;
        }
    }
}
