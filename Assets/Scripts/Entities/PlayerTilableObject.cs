using System;
using System.Collections;
using Core.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Core.Entities
{
    [DefaultExecutionOrder(1)]
    public class PlayerTilableObject : BaseTilableObject, ITilable, IAnimatorEventListner
    {

        [SerializeField] private bool _canCounterAttack = false;
        [SerializeField] private float _health;
        [SerializeField] private float _healthMax;
        [SerializeField] private int _damage;
        [SerializeField] private int _baseDamage;
        [SerializeField] private Transform _hips;
        [SerializeField] private Animator _animator;
        [SerializeField] private int _currentWeaponCharges = 0;
        [SerializeField] private GameObject[] _weapons;

        public float HP => _health;
        public float MaxHP => _healthMax;
        public int CurrentDamage
        {
            get
            {
                return _damage;
            }
        }


        private IEnumerator Start()
        {
            _damage = _baseDamage;
            _animator = GetComponentInChildren<Animator>();
            
            yield return null;
            SetBox(TileController.Instance.Center);
            UpdateHealthUi();
            
        }

        public void ActivateOneHandedWeapon(int charges, int damage)
        {
            _animator.SetBool("ThoHand", false);
            _currentWeaponCharges = charges;
        }

        public void ActivateTwoHandedWeapon(int charges, int damage)
        {
            _animator.SetBool("ThoHand", true);
            _currentWeaponCharges = charges;
        }

        public void DeactivateWeapon()
        {
            _animator.SetBool("ThoHand", false);
            _damage = _baseDamage;
        }


        public override void SetBox(TileBox box)
        {
            if (_currentTileBox != null)
            {
                _currentTileBox.ChangeTiledObject();
            }
            if (box == null)
            {
                return;
            }
            _currentTileBox = box;
            _currentTileBox.ChangeTiledObject(this);
            transform.position = box.transform.position;
            PlayStartAnimation();
        }

        public void PlayStartAnimation()
        {
        }

        public void GetDamage(float Damage, BaseTilableObject enemy)
        {
            GetDamage(Damage);
            if (_canCounterAttack)
            {
                
            }
        }
        
        public void GetHeal(float heal)
        {
            _health += heal;
            UpdateHealthUi();
        }
        
        public void GetDamage(float damage)
        {
            //_hips.DOShakePosition(0.4f, snapping: false, strength: new Vector3(0.3f, 0, 0.3f)).OnComplete(() => _hips.localPosition = Vector3.up * 0.5f);
            _animator.SetTrigger("Hit");
            _health -= damage;
            UpdateHealthUi();
            if (_health <= 0)
            {
                _health = 0;
                NearDeath();
            }
        }

        public void UpdateHealthUi()
        {
            GameEvents.Instance.PlayerHpChange(_health,0,_healthMax);
        }


        public void NearDeath()
        {
            //HealthSaves
            StartCoroutine(LevelController.Instance.LevelFailed());
        }

        public void GoToExitDoor()
        {
            LevelController.Instance.LevelVictory();
        }

        public override string CompareConfig(BaseTilableObject obj)
        {
            base.CompareConfig(obj);
            switch (obj.Config)
            {
                case "":
                {
                    break;
                }
                case "Exit"://Enter-alt
                {
                    StartCoroutine(Exit(obj)); 
                    break;
                }
                case "Collectable"://Enter-alt
                {
                    StartCoroutine(Pickup(obj)); 
                    break;
                }
                case "Enemy":
                {
                    //StartCoroutine(Attack(obj)); This call moved to EnemyTilableObject
                    break;
                }
                default:
                {
                    break;
                }
            }

            return _config;
        }

        private bool _endAttack = false;
        public void EndAttack()
        {
            _endAttack = true;
            Debug.LogWarning("PlayerEndAttack");
        }

        public IEnumerator Attack(BaseTilableObject obj)
        {
            if (LevelController.Instance.TurnState == TurnState.Player/* || _canCounterAttack*/)
            {
                transform.DORotateQuaternion(Quaternion.LookRotation(obj.transform.position - transform.position, Vector3.up) , 0.05f);
                _animator.SetTrigger("Attack");
                yield return new WaitUntil(() => _endAttack);
                _endAttack = false;
                obj.CallbackForPlayerMoves(PlayerCallbackType.Attack, this);
                
                
                /*for (float i = 0; i < 0.5f;  i = Mathf.Clamp(i + Time.deltaTime * _jumpSpeed, 0 ,0.5f))
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, obj.Tile.transform.position,
                        i);
                    TempVector3.y = Mathf.Sin(Mathf.Clamp(i,0,0.5f) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }
                
                for (float i = 0; i < 0.5f; i = Mathf.Clamp(i + Time.deltaTime * _jumpSpeed, 0 ,0.5f))
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, obj.Tile.transform.position,
                        (0.5f - i));
                    TempVector3.y = Mathf.Sin(Mathf.Clamp(0.5f - i,0f,0.5f) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }*/
            }
        }
        public IEnumerator Pickup(BaseTilableObject obj) //Enter-alt
        {
            if (LevelController.Instance.TurnState == TurnState.Player)
            {
                obj.CallbackForPlayerMoves(PlayerCallbackType.Pickup, this);
            }
            yield break;
        }
        public IEnumerator Exit(BaseTilableObject obj) //Enter-alt
        {
            if (LevelController.Instance.TurnState == TurnState.Player)
            {
                obj.CallbackForPlayerMoves(PlayerCallbackType.Exit, this);
            }
            yield break;
        }
        

    public void LoadStatsFormPrefs()
        {
        }

        public void SavePrefs()
        {
        }
    }
}