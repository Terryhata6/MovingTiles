using System;
using UnityEngine;

namespace Core
{
    public class SwipeDetection : MonoBehaviour
    {
        private InputController _inputController;
        [SerializeField] private float _minimumDistance = 0.08f;
        [SerializeField] private float _maximumTime = 1f;
        [SerializeField][Range(0f,1f)] float _directionTreshhold = 0.9f;
        
        private Vector3 _startPosition = Vector2.zero;
        private float _startTime = 0f; 
        private Vector3 _endPosition= Vector2.zero;
        private float _endTime = 0f;
        private Vector3 _swipeDirection;
        private Vector2 _swipeDirection2d;
        private void Awake()
        {
            _inputController = InputController.Instance;
        }

        private void OnEnable()
        {
            _inputController.OnStartTouch += SwipeStart;
            _inputController.OnEndTouch += SwipeEnd;
        }
        
        private void SwipeStart(Vector3 position, float time)
        {
            _startPosition = position;
            _startTime = time;
        }
        
        private void SwipeEnd(Vector3 position, float time)
        {
            _endPosition = position;
            _endTime = time;
            DetectSwipe();
        }

        private void DetectSwipe()
        {
            
            float f = Vector3.Distance(_startPosition, _endPosition);
            if ( f >= _minimumDistance &&
                (_endTime - _startTime) <= _maximumTime)
            {
                
                _swipeDirection = _endPosition - _startPosition;
                _swipeDirection2d.x = _swipeDirection.x;
                _swipeDirection2d.y = _swipeDirection.z;
                
                _swipeDirection2d.Normalize();
                Debug.Log($"SWIPEFUCK {_swipeDirection2d}");
                SwipeDirection(_swipeDirection2d);
            }
        }

        private void SwipeDirection(Vector2 direction)
        {
            if (Vector3.Dot(direction, Vector2.up) >= _directionTreshhold)
            {
                Debug.Log("UP");
            }
            else if (Vector3.Dot(direction, Vector2.down) >= _directionTreshhold)
            {
                Debug.Log("Down");
            }
            else if (Vector3.Dot(direction, Vector2.left) >= _directionTreshhold)
            {
                Debug.Log("Left");
            }
            else if (Vector3.Dot(direction, Vector2.right) >= _directionTreshhold)
            {
                Debug.Log("Right");
            }
        }

        private void OnDisable()
        {
            _inputController.OnStartTouch -= SwipeStart;
            _inputController.OnEndTouch -= SwipeEnd;
        }
    }
}