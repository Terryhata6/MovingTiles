using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
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
        [SerializeField] private bool _needAnimator = false;
        [SerializeField] private GameObject[] _weapons;
        [SerializeField] private bool _needBakeMesh = false;
        [SerializeField] private RealTimeSkinnedMeshBaker _baker;

        [SerializeField] private bool _haveRagdall = false;
        [SerializeField] private List<Rigidbody> _rigs;
        [SerializeField] private List<Collider> _colliders;

        [SerializeField] private MMProgressBar _healthBar;

        private int _maxHealth;

        private bool _endAttack = false;

        public int Health => _health;
        public float Damage => _damage;

        public override void Awake()
        {
            base.Awake();
            if (_needAnimator)
            {
                _animator = GetComponentInChildren<Animator>();
            }
            else
            {
            }

            if (_haveRagdall)
            {
                _rigs = GetComponentsInChildren<Rigidbody>().ToList();
                _colliders = GetComponentsInChildren<Collider>().ToList();
                DeactivateRagdoll();
            }

            if (Random.Range(0, 10) > 5)
            {
                if (Random.Range(0, 10) > 8)
                {
                    SetWeapon(2);
                }
                else
                {
                    SetWeapon(1);
                }
            }

            _maxHealth = _health;
        }

        public void ActivateRagdoll()
        {
            if (!_haveRagdall)
                return;
            if (_needAnimator)
            {
                _animator.enabled = false;
            }
            else
            {
            }

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
            if (_needAnimator)
            {
                _animator.enabled = true;
            }
            else
            {
            }

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
            if (_weapons == null)
            {
                return;
            }

            if (_weapons.Length > 0)
            {
                int result = Random.Range(0, _weapons.Length);
                _weapons[result].SetActive(true);
                _damage = damage;
                ActivateWeaponMesh(result);
            }
        }

        public void ActivateWeaponMesh(int result)
        {
            foreach (var go in _weapons)
            {
                go.SetActive(false);
            }

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
                if (haveInteraction)
                {
                    interaction.CallSpecialInteraction();
                    interaction.CallSpecialRareInteraction(EnemySpecialInteraction.stringTypePhrazes.AttackPhrazes);
                }

                if (_needAnimator)
                {
                    _animator.SetTrigger("Attack");
                }
                else
                {
                    transform.DOMove(transform.position + (box.transform.position - transform.position) * 0.5f, 0.2f)
                        .OnComplete(() =>
                        {
                            _endAttack = true;
                            transform.DOMove(_currentTileBox.transform.position, 0.2f);
                        });
                }

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
            if (_needAnimator)
            {
                _animator.SetTrigger("Hit");
            }
            else
            {
            }

            if (Health <= 0)
            {
                _health = 0;
                if (haveInteraction)
                {
                    interaction.CallSpecialRareInteraction(EnemySpecialInteraction.stringTypePhrazes.DiesPhrazes);
                }

                _healthBar?.gameObject.SetActive(false);

                StartCoroutine(DestroyAnimation());
            }
            else
            {
                if (haveInteraction)
                {
                    interaction.CallSpecialRareInteraction(EnemySpecialInteraction.stringTypePhrazes.GetdamagePhrazes);
                }
            }

            _healthBar?.UpdateBar(_health, 0, _maxHealth);
        }

        public override IEnumerator DestroyAnimation()
        {
            TilableObjectsController.Instance.RemoveObjectFromList(this);
            _currentTileBox.ChangeTiledObject();
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

            transform.DOMoveY(_currentTileBox.transform.position.y, 0.2f)
                .OnComplete(() => OnEndSpawn.Invoke(this));
            yield break;
        }
    }
}