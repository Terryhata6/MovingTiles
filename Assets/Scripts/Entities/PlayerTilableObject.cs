using System;
using System.Collections;
using Core.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Core.Entities
{
    [DefaultExecutionOrder(1)]
    public class PlayerTilableObject : BaseTilableObject, ITilable
    {

        [SerializeField] private bool _canCounterAttack = false;
        [SerializeField] private float _health;
        [SerializeField] private float _healthMax;
        [SerializeField] private int _baseDamage;
        [SerializeField] private Transform _hips;

        public float HP => _health;
        public float MaxHP => _healthMax;
        public int CurrentDamage
        {
            get
            {
                return _baseDamage;
            }
        }


        private IEnumerator Start()
        {
            UpdateHealthUi();
            yield return null;
            SetBox(TileController.Instance.Center);
            UpdateHealthUi();
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
            _hips.DOShakePosition(0.4f, snapping: false, strength: new Vector3(0.3f, 0, 0.3f)).OnComplete(() => _hips.localPosition = Vector3.up * 0.5f);
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
        
        public void Attack()
        {
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
                    StartCoroutine(Attack(obj));
                    break;
                }
                default:
                {
                    break;
                }
            }

            return _config;
        }

        public IEnumerator Attack(BaseTilableObject obj)
        {
            if (LevelController.Instance.TurnState == TurnState.Player/* || _canCounterAttack*/)
            {
                transform.DORotateQuaternion(Quaternion.LookRotation(obj.transform.position - transform.position, Vector3.up) , 0.05f);
                for (float i = 0; i < 0.5f;  i = Mathf.Clamp(i + Time.deltaTime * _jumpSpeed, 0 ,0.5f))
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, obj.Tile.transform.position,
                        i);
                    TempVector3.y = Mathf.Sin(Mathf.Clamp(i,0,0.5f) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }
                obj.CallbackForPlayerMoves(PlayerCallbackType.Attack, this);
                for (float i = 0; i < 0.5f; i = Mathf.Clamp(i + Time.deltaTime * _jumpSpeed, 0 ,0.5f))
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, obj.Tile.transform.position,
                        (0.5f - i));
                    TempVector3.y = Mathf.Sin(Mathf.Clamp(0.5f - i,0f,0.5f) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }
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