using System;
using System.Collections;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Entities
{
    public class EnemyTilableObject : TilableObject
    {
        [Header("EnemyProps")] [SerializeField]
        private int _health;

        [SerializeField] private MMFeedbacks _mmFeedbacks;
        [SerializeField] private float BaseDamage = 1f;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject[] _weapons;
        [SerializeField] private bool _needBakeMesh = false;
        [SerializeField] private RealTimeSkinnedMeshBaker _baker;
        public int Health => _health;
        private bool _endAttack = false;

        public void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void SetWeapon(int charges, int damage)
        {
            
        }

        public void EndAttack()
        {
            _endAttack = true;
            Debug.LogWarning("PlayerEndAttack");
        }

        protected override IEnumerator InteractionWithPlayer(TileBox box, TurnState state)
        {
            if (state == TurnState.Enemy)
            {
                _animator.SetTrigger("Attack");
                yield return new WaitUntil(() => _endAttack);
                _endAttack = false;
                (box.TiledObject as PlayerTilableObject).GetDamage(BaseDamage);
                /*
                for (float i = 0; i < 0.5f; i += 0.01f * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position,
                        i);
                    TempVector3.y = Mathf.Sin(Mathf.Clamp(i, 0f, 0.5f) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }

                for (float i = 0; i < 0.5f; i += 0.01f * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position,
                        (0.5f - i));
                    TempVector3.y = Mathf.Sin(Mathf.Clamp(0.5f - i, 0f, 0.5f) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }*/
            }
            else
            {
                yield return (box.TiledObject as PlayerTilableObject).Attack(this);

                if (_health < 0)
                {
                    StartCoroutine(DestroyAnimation());
                }
            }
        }

        public override void CallbackForPlayerMoves(PlayerCallbackType callbackType, PlayerTilableObject player)
        {
            //base.CallbackForPlayerMoves(callbackType, player);
            switch (callbackType)
            {
                case PlayerCallbackType.Pickup:
                {
                    break;
                }
                case PlayerCallbackType.Exit:
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
            _animator.SetTrigger("Hit");
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
            yield return null;
            Destroy(this.gameObject);
            yield break;
        }

        public UnityAction End;

        public override IEnumerator SpawnAnimation(Action<BaseTilableObject> OnEndSpawn)
        {
            if (_needBakeMesh)
            {
                _baker = GetComponentInChildren<RealTimeSkinnedMeshBaker>();
                yield return _baker.StartBaking();
            }
            transform.DOMoveY(_currentTileBox.transform.position.y + 0.5f, 0.2f)
                .OnComplete(() => OnEndSpawn.Invoke(this));
            yield break;
        }
    }
}