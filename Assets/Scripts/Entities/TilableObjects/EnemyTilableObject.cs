using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Core.Entities
{
    public class EnemyTilableObject : TilableObject
    {
        [Header("EnemyProps")] [SerializeField]
        private int _health;

        [SerializeField] private MMFeedbacks _mmFeedbacks;
        [SerializeField] private float _baseDamage = 1f;
        [SerializeField] private float _damage = 1f;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject[] _weapons;
        [SerializeField] private bool _needBakeMesh = false;
        [SerializeField] private RealTimeSkinnedMeshBaker _baker;
        
        [SerializeField] private bool _haveRagdall = false;
        [SerializeField] private List<Rigidbody> _rigs;
        [SerializeField] private List<Collider> _colliders;
        private bool _endAttack = false;
        public int Health => _health;
        public float Damage => _damage;

        public override void Awake()
        {
            base.Awake();
            _animator = GetComponentInChildren<Animator>();
            if (_haveRagdall)
            {
                _rigs = GetComponentsInChildren<Rigidbody>().ToList();
                _colliders = GetComponentsInChildren<Collider>().ToList();
                DeactivateRagdoll();
            }
        }

        public void ActivateRagdoll()
        {
            if (!_haveRagdall)
                return;
            _animator.enabled = false;
            for (int i = 0; i < _rigs.Count; i++)
            {
                _rigs[i].isKinematic = false;
                _rigs[i].useGravity = true;
            }
            for (int i = 0; i < _colliders.Count; i++)
            {
                _colliders[i].isTrigger = false;
            }
        }

        public void DeactivateRagdoll()
        {
            if (!_haveRagdall)
                return;
            _animator.enabled = true;
            for (int i = 0; i < _rigs.Count; i++)
            {
                _rigs[i].isKinematic = true;
                _rigs[i].useGravity = false;
            }
            for (int i = 0; i < _colliders.Count; i++)
            {
                _colliders[i].isTrigger = true;
            }
        }

        public void SetWeapon(int damage)
        {
            int result = Random.Range(0, _weapons.Length);
            _weapons[result].SetActive(true);
            
        }

        public void EndAttack()
        {
            _endAttack = true;
        }

        protected override IEnumerator InteractionWithPlayer(TileBox box, TurnState state)
        {
            if (state == TurnState.Enemy)
            {
                _animator.SetTrigger("Attack");
                yield return new WaitUntil(() => _endAttack);
                _endAttack = false;
                (box.TiledObject as PlayerTilableObject).GetDamage(Damage, this);
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
            StartCoroutine(EncauntersHolder.Instance.CreateDropFromkKilledEnemy(transform.position));
            if (_haveRagdall)
            {
                ActivateRagdoll();
                foreach (var rig in _rigs)
                {
                    rig.AddForce(transform.forward * -3f + transform.up * 15f, ForceMode.Impulse);   
                }
                yield return new WaitForSeconds(2f);
                Destroy(this.gameObject);
            }
            else
            {
                transform.DOMoveY(-4f, 2f).OnComplete(() => Destroy(this.gameObject));
            }
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