using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Entities
{
    public class EnemyTilableObject : TilableObject
    {

        [Header("EnemyProps")]
        [SerializeField] private int _health;
        [SerializeField] private MMFeedbacks _mmFeedbacks;

        public int Health => _health;
        
        protected override IEnumerator PlayerInteraction(TileBox box, TurnState state)
        {
            //base.PlayerInteraction(box, state);
            if (state == TurnState.Enemy)
            {
                for (float i = 0; i < 0.5f; i += 0.01f * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position,
                        i);
                    TempVector3.y = Mathf.Sin(i * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }

                for (float i = 0; i < 0.5f; i += 0.01f * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position,
                        (0.5f - i));
                    TempVector3.y = Mathf.Sin((0.5f - i) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }
            }
            else
            {
                //TODO Animation blockAttack
            }
        }
        
        public override void PlayerCallBack(PlayerCallbackType callbackType, PlayerTilableObject player)
        {
            //base.PlayerCallBack(callbackType, player);
            switch (callbackType)
            {
                case PlayerCallbackType.Pickup:
                {
                    break;
                }
                case PlayerCallbackType.Attack:
                {
                    GetDamage(player.CurrentDamage);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(callbackType), callbackType, null);
            }
        }

        public void GetDamage(int damage)
        {
            _health -= damage;
            if (Health <= 0)
            {
                _health = 0;
                StartCoroutine(DestroyAnimation());
            }
        }

        public override IEnumerator DestroyAnimation()
        {
            TilableObjectsController.Instance.RemoveObjectFromList(this);
            //TODO Destroy Death Animation
            Destroy(this.gameObject);
            yield break;
            
        }

        public UnityAction End;
        public override IEnumerator SpawnAnimation(Action<BaseTilableObject> OnEndSpawn)
        {
            _mmFeedbacks.Initialization();
            var deltaPosition = _mmFeedbacks.GetComponent<MMFeedbackPosition>();
            //deltaPosition.InitialPosition = _currentTileBox.transform.position + Vector3.up * 3f;
            /*deltaPosition.DestinationPositionTransform = _currentTileBox.transform;
            deltaPosition.DestinationPosition = deltaPosition.DestinationPositionTransform.position;*/
            End = () =>
            {
                _mmFeedbacks.GetComponent<MMFeedbackEvents>().PlayEvents.RemoveListener(End);
                OnEndSpawn?.Invoke(this);
            };
            _mmFeedbacks.GetComponent<MMFeedbackEvents>().PlayEvents.AddListener(End);
            _mmFeedbacks.PlayFeedbacks();
            yield break;
        }
    }
}