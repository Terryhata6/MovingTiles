using System;
using System.Collections;
using System.Collections.Generic;
using Core.UtilitsSpace;
using MoreMountains.Feedbacks;
using UnityEngine;

public class SoundController : Singleton<SoundController>
{
    [SerializeField] private MMFeedbacks _mmFeedbacks;
    [SerializeField] private MMFeedbackFloatingText _textSender;
    [SerializeField] private GameObject _spawnerExample;
    [SerializeField] private GameObject _currentSpawner;
    [SerializeField] private MMFeedbackMMSoundManagerSound _mainTrackSound;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        _mainTrackSound.Play(Vector3.zero, 1f);
    }

    public void SendText(Transform targetTransform, string text)
    {
        if (_currentSpawner == null)
        {
            _currentSpawner = Instantiate(_spawnerExample);
        }

        if (_textSender == null)
        {
            _textSender = _mmFeedbacks.GetComponent<MMFeedbackFloatingText>();
        }
        _textSender.Value = text;
        _textSender.TargetTransform = targetTransform;
        //_textSender.Play(targetTransform.position);
        _mmFeedbacks.PlayFeedbacks(targetTransform.position + Vector3.up);
    }
}
