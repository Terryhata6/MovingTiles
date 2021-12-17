using System;
using System.Collections;
using System.Collections.Generic;
using Core.Entities;
using Core.UtilitsSpace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core
{
    public class DragUITilableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private TilableObject _tilableObjectInstance;
        [SerializeField] private TilableObject _tilableObjectExmple;
        [SerializeField] private int _charges = 0;
        private RectTransform _draggingObjectRectTransform;
        private Vector3 _beginPosition;
        private bool _startCheckingTilable = false;
        private Vector3 _velocityVector = Vector3.zero;
        [SerializeField] private float dampingSpeed;
        private Vector3 globalMousePosition;
        [SerializeField] private Image _image;

        public void Awake()
        {
            _draggingObjectRectTransform = transform as RectTransform;
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
                            _image.enabled = false;
                        }
                    }
                }
            }
        }

        private void SpawnOrNotSpawnCallBack()
        {
            Debug.Log("SpawnOrNotSpawnCallBack");
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
                _image.enabled = true;
                _draggingObjectRectTransform.position = _beginPosition;
                _tilableObjectInstance =
                    Instantiate(_tilableObjectExmple.gameObject, transform).GetComponent<TilableObject>();
                _tilableObjectInstance.gameObject.SetActive(false);
                _charges--;
            }
        }
    }
}