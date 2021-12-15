using System;
using System.Collections;
using Core.Interfaces;
using UnityEngine;

namespace Core.Entities
{
    public class PlayerTilableObject : BaseTilableObject, ITilable
    {

        [SerializeField] private bool _canCounterAttack = false;
        [SerializeField] private float _health;
        [SerializeField] private int _baseDamage;

        public float HP => _health;
        public int CurrentDamage
        {
            get
            {
                return _baseDamage;
            }
        }


        private IEnumerator Start()
        {
            yield return null;
            SetBox(TileController.Instance.Center);
        }

        public override void SetBox(TileBox box)
        {
            if (_currentTileBox != null)
            {
                _currentTileBox.ChangeTiledObject();
            }

            _currentTileBox = box;
            _currentTileBox.ChangeTiledObject(this);
            transform.position = box.transform.position;
            PlayStartAnimation();
        }

        public void PlayStartAnimation()
        {
        }

        public void GetDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _health = 0;
                NearDeath();
            }
        }

        public void NearDeath()
        {
            //HealthSaves
            LevelController.Instance.LevelFailed();
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
            if (LevelController.Instance.TurnState == TurnState.Player || _canCounterAttack)
            {
                for (float i = 0; i < 0.6f; i += Time.deltaTime * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, obj.Tile.transform.position,
                        i);
                    TempVector3.y = Mathf.Sin(Mathf.Clamp(i,0,0.5f) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }
                obj.PlayerCallBack(PlayerCallbackType.Attack, this);
                for (float i = 0; i < 0.6f; i += Time.deltaTime * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, obj.Tile.transform.position,
                        (0.5f - i));
                    TempVector3.y = Mathf.Sin(Mathf.Clamp(0.5f - i,0f,0.5f) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }
            }
        }

        public void LoadStatsFormPrefs()
        {
        }

        public void SavePrefs()
        {
        }
    }
}