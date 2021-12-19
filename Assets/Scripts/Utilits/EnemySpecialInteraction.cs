using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpecialInteraction : MonoBehaviour
{
    [SerializeField] private Transform _interactionTarget;
    [SerializeField] private Transform _interactionTarget2;
    [SerializeField] private string _interactionConfig;
    [SerializeField] private List<string> MovesPhrazes; 
    [SerializeField] private List<string> YouMovesPhrazes; 
    [SerializeField] private List<string> AttackPhrazes; 
    [SerializeField] private List<string> GetdamagePhrazes; 
    [SerializeField] private List<string> DiesPhrazes; 

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
            case "Enemy":
            {
                SoundController.Instance.SendText(this.transform, "HAHAHAHAH");
                break;
            }
            default:
                break;
        }
    }
    private string textMessage;
    public void CallSpecialRareInteraction(stringTypePhrazes switchTypePhrazes)
    {
        StartCoroutine(RareInteraction(switchTypePhrazes));
    }

    public IEnumerator RareInteraction(stringTypePhrazes switchTypePhrazes)
    {
        try
        {
            switch (_interactionConfig)
            {
                case "Mimic":
                    break;
                case "Enemy":
                {
                    if (Random.Range(0, 100) <= 15)
                    {
                        textMessage = "";
                        switch (switchTypePhrazes)
                        {
                            case stringTypePhrazes.MovesPhrazes:
                                textMessage = MovesPhrazes[Random.Range(0, MovesPhrazes.Count)];
                                break;
                            case stringTypePhrazes.YouMovesPhrazes:
                                textMessage = YouMovesPhrazes[Random.Range(0, YouMovesPhrazes.Count)];
                                break;
                            case stringTypePhrazes.AttackPhrazes:
                                textMessage = AttackPhrazes[Random.Range(0, AttackPhrazes.Count)];
                                break;
                            case stringTypePhrazes.GetdamagePhrazes:
                                textMessage = GetdamagePhrazes[Random.Range(0, GetdamagePhrazes.Count)];
                                break;
                            case stringTypePhrazes.DiesPhrazes:
                                textMessage = DiesPhrazes[Random.Range(0, DiesPhrazes.Count)];
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(switchTypePhrazes), switchTypePhrazes, null);
                        }

                        if (textMessage == "") break;
                        SoundController.Instance.SendText(this.transform, textMessage);
                    }
                    break;
                }
                default:
                    break;
            }
        }
        catch (Exception e)
        {
            
        }
        yield return null;
    }

    public enum stringTypePhrazes
    {
        MovesPhrazes,
        YouMovesPhrazes,
        AttackPhrazes,
        GetdamagePhrazes,
        DiesPhrazes
    }
}
