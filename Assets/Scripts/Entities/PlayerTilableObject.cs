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
        [SerializeField] private int _currentWeapon = -1;
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
            SaveLoadManager.Instance.LoadPlayerData(out _health, out _healthMax, out _currentWeapon, out _currentWeaponCharges, out _damage);
            if (_currentWeapon >= 0 && _currentWeapon < _weapons.Length)
            {
                SetWeapon((WeaponType) _currentWeapon, _damage, _currentWeaponCharges);
            }

            yield return null;
            SetBox(TileController.Instance.Center);
            UpdateHealthUi();
            
        }

        public void ActivateOneHandedWeapon(int charges, int damage)
        {
            _animator.SetBool("ThoHand", false);
            _currentWeaponCharges = charges;
            _damage = damage;
        }

        public void ActivateTwoHandedWeapon(int charges, int damage)
        {
            _animator.SetBool("ThoHand", true);
            _currentWeaponCharges = charges;
            _damage = damage;
        }

        public void DeactivateWeapon()
        {
            for (int i = 0; i < _weapons.Length; i++)
            {
                _weapons[i].SetActive(false);
            }
            //TODO DeactivateWeaponFeedBack
            _animator.SetBool("ThoHand", false);
            _damage = _baseDamage;
            _currentWeapon = -1;
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
            //
        }

        public void GetDamage(float Damage, BaseTilableObject enemy)
        {
            GetDamage(Damage);
            if (_canCounterAttack)
            {
                enemy.CallbackForPlayerMoves(PlayerCallbackType.Attack, this);
            }
        }
        
        public void GetHeal(float heal)
        {
            _health += heal;
            if (_health > _healthMax)
            {
                _health = MaxHP;
            }

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
            SaveLoadManager.Instance.SavePlayerData(_health, _healthMax, _currentWeapon, _currentWeaponCharges, _damage);
            LevelController.Instance.LevelVictory();
        }

        public void SetWeapon(WeaponType type, int damage, int charges)
        {
            for (int i = 0; i < _weapons.Length; i++)
            {
                _weapons[i].SetActive(false);
            }
            switch (type)
            {
                case WeaponType.Axe:
                {
                    if (_currentWeapon == 0)
                    {
                        ActivateTwoHandedWeapon(charges + _currentWeaponCharges, damage);
                    }
                    else
                    {
                        ActivateTwoHandedWeapon(charges, damage);
                    }
                    _currentWeapon = 0;
                    _weapons[0].SetActive(true);
                    break;
                }
                case WeaponType.BigSword:
                {
                    if (_currentWeapon == 1)
                    {
                        ActivateTwoHandedWeapon(charges + _currentWeaponCharges, damage);
                    }
                    else
                    {
                        ActivateTwoHandedWeapon(charges, damage);
                    }
                    _currentWeapon = 1;
                    _weapons[1].SetActive(true);
                    break;
                }
                case WeaponType.Katana:
                {
                    ActivateOneHandedWeapon(charges,damage);
                    _currentWeapon = 2;
                    _weapons[2].SetActive(true);
                    break;
                }
                case WeaponType.Mace:
                {
                    if (_currentWeapon == 3)
                    {
                        ActivateTwoHandedWeapon(charges + _currentWeaponCharges, damage);
                    }
                    else
                    {
                        ActivateTwoHandedWeapon(charges, damage);
                    }
                    _currentWeapon = 3;
                    _weapons[3].SetActive(true);
                    break;
                }
                case WeaponType.Pickaxe:
                {
                    if (_currentWeapon == 4)
                    {
                        ActivateTwoHandedWeapon(charges + _currentWeaponCharges, damage);
                    }
                    else
                    {
                        ActivateTwoHandedWeapon(charges, damage);
                    }
                    _currentWeapon = 4;
                    _weapons[4].SetActive(true);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
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

        private bool _attackStarted = false;
        private bool _endAttack = false;
        public void EndAttack()
        {
            if (_attackStarted)
            {
                _endAttack = true;
                //Debug.LogWarning("PlayerEndAttack");
                
            }
        }

        public IEnumerator Attack(BaseTilableObject obj)
        {
            if (LevelController.Instance.TurnState == TurnState.Player/* || _canCounterAttack*/)
            {
                transform.DORotateQuaternion(Quaternion.LookRotation(obj.transform.position - transform.position, Vector3.up) , 0.05f);
                _animator.SetTrigger("Attack");
                _attackStarted = true;
                _currentWeaponCharges--;
                yield return new WaitUntil(() => _endAttack);
                _endAttack = false;
                _attackStarted = false;
                obj.CallbackForPlayerMoves(PlayerCallbackType.Attack, this);
                if (_currentWeaponCharges <= 0)
                {
                    DeactivateWeapon();
                }
                
                
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