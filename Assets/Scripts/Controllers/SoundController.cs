using System.Collections;
using System.Collections.Generic;
using Core.UtilitsSpace;
using MoreMountains.Feedbacks;
using UnityEngine;

public class SoundController : Singleton<SoundController>
{
    [SerializeField] private MMFeedbacks _mmFeedbacks;
    [SerializeField] private MMFeedbackFloatingText _textSender;


    public void SendText(Transform targetTransform, string text)
    {
        if (_textSender == null)
        {
            _textSender = _mmFeedbacks.GetComponent<MMFeedbackFloatingText>();
        }
        _textSender.Value = text;
        _textSender.TargetTransform = targetTransform;
        _textSender.Play(targetTransform.position);
        _mmFeedbacks.PlayFeedbacks(targetTransform.position);
    }
}
