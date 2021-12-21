using System;
using System.Collections;
using System.Collections.Generic;
using Core.Entities;
using Core.UtilitsSpace;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core
{
    public class DragUITilableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private TilableObject _tilableObjectInstance;
        [SerializeField] private TilableObject _tilableObjectExmple;
        [SerializeField] private GameObject _tutorial;
        [SerializeField] private int _charges = 0;
        private bool _tutorialReady = false;
        private RectTransform _draggingObjectRectTransform;
        private Vector3 _beginPosition;
        private bool _startCheckingTilable = false;
        private Vector3 _velocityVector = Vector3.zero;
        private Vector3 _tempPos;
        [SerializeField] private float dampingSpeed;
        private Vector3 globalMousePosition;
        [SerializeField] private Image _image;
        public bool ItsFree => _charges <= 0;
        
        public void Awake()
        {
            _draggingObjectRectTransform = transform as RectTransform;
            EnableImage(false);
            if (_tutorial != null)
            {
                if(PlayerPrefs.GetInt("IconTutorialWasLearned", 0) < 1)
                {
                    _tutorialReady = true;
                }
            }
        }


        public void SetNewObject(Sprite image,TilableObject tilableObjectExmple)
        {
            
            if (_tilableObjectInstance != null)
            {
                Destroy(_tilableObjectInstance.gameObject);
            }

            if (tilableObjectExmple != null)
            {
                _tilableObjectExmple = tilableObjectExmple;
                _image.sprite = image;
                _tilableObjectInstance = Instantiate(_tilableObjectExmple.gameObject, transform.position,
                    Quaternion.identity,transform).GetComponent<TilableObject>();
                _tilableObjectInstance.gameObject.SetActive(false);
                _tilableObjectInstance.gameObject.transform.localScale =
                    Vector3.one*(_tilableObjectInstance.gameObject.transform.localScale.x /
                    UIController.Current.transform.localScale.x);
                EnableImage(true);
                if (_tutorialReady)
                {
                    _tutorial.SetActive(true);
                }

                AddCharge(1);
            }
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (_charges > 0)
            {
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    _draggingObjectRectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out globalMousePosition))
                {
                    if (!_startCheckingTilable)
                    {
                        _draggingObjectRectTransform.position = Vector3.SmoothDamp(
                            _draggingObjectRectTransform.position, globalMousePosition, ref _velocityVector,
                            dampingSpeed);
                        if (Vector3.Distance(globalMousePosition, _beginPosition) > 100.0f)
                        {
                            _startCheckingTilable = true;
                            StartCoroutine(
                                TilableObjectsController.Instance.Pointer.PointSkill(_tilableObjectInstance,
                                    SpawnOrNotSpawnCallBack));
                            EnableImage(false);
                            if (_tutorialReady)
                            {
                                _tutorial.SetActive(false);
                                PlayerPrefs.SetInt("IconTutorialWasLearned", 1);
                                _tutorialReady = false;
                            }
                        }
                    }
                }
            }
        }

        private void SpawnOrNotSpawnCallBack()
        {
            RemoveCharge(1);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_charges > 0)
            {
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    _draggingObjectRectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out globalMousePosition))
                {
                    _beginPosition = globalMousePosition;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_charges > 0)
            {
                _tilableObjectInstance.transform.parent = TileController.Instance.transform;
                _startCheckingTilable = false;
                EnableImage(true);
                _draggingObjectRectTransform.position = _beginPosition;
                _tilableObjectInstance = Instantiate(_tilableObjectExmple.gameObject, transform.position,
                                Quaternion.identity,transform).GetComponent<TilableObject>();
                _tilableObjectInstance.gameObject.SetActive(false);
            }
        }

        public void ChangeChargesAmount(int charge)
        {
            _charges = charge;
            if (_charges <= 0)
            {
                EnableImage(false);
            }
            
        }

        public void AddCharge(int charge)
        {
            if (charge > 0)
            {
                ChangeChargesAmount(_charges+charge);
            }
        }
        public void RemoveCharge(int charge)
        {
            if (charge > 0)
            {
                ChangeChargesAmount(_charges-charge);
            }
        }

        public void EnableImage(bool trueOrFalse)
        {
            _image.enabled = trueOrFalse;
        }

        public bool CompareObject(TilableObject obj)
        {
            Debug.Log(_tilableObjectExmple.Equals(obj));
            return _tilableObjectExmple.Equals(obj);
        }
    }
}