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
            GameEvents.Instance.OnSwipeFromArrow += SwipeFromArrow;
        }

        private void SwipeFromArrow(Vector3 direction)
        {
            _startPosition = Vector3.zero;
            _endPosition = direction;
            _startTime = 0f;
            _endTime = _startTime + _maximumTime * 0.5f;
            DetectSwipe();
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
            if (Vector3.Distance(_startPosition, _endPosition) >= _minimumDistance &&
                 (_endTime - _startTime) <= _maximumTime)
            {
                _swipeDirection = _endPosition - _startPosition;
                _swipeDirection2d.x = _swipeDirection.x;
                _swipeDirection2d.y = _swipeDirection.z;
                
                _swipeDirection2d.Normalize();
                SwipeDirection(_swipeDirection2d);
            }
        }

        private void SwipeDirection(Vector2 direction)
        {
            if (Vector3.Dot(direction, Vector2.up) >= _directionTreshhold)
            {
                //Debug.Log("UP");
                _inputController.GetSwipe(SwipeDirections.Up);
            }
            else if (Vector3.Dot(direction, Vector2.down) >= _directionTreshhold)
            {
                //Debug.Log("Down");
                _inputController.GetSwipe(SwipeDirections.Down);
            }
            else if (Vector3.Dot(direction, Vector2.left) >= _directionTreshhold)
            {
                //Debug.Log("Left");
                _inputController.GetSwipe(SwipeDirections.Left);
            }
            else if (Vector3.Dot(direction, Vector2.right) >= _directionTreshhold)
            {
                //Debug.Log("Right");
                _inputController.GetSwipe(SwipeDirections.Right);
            }
        }

        private void OnDisable()
        {
            _inputController.OnStartTouch -= SwipeStart;
            _inputController.OnEndTouch -= SwipeEnd;
            GameEvents.Instance.OnSwipeFromArrow -= SwipeFromArrow;
        }
    }

    public enum SwipeDirections
    {
        Left,
        Right,
        Up,
        Down
    }
}