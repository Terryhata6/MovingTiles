using System;
using System.Collections;
using Core.Interfaces;
using UnityEngine;

namespace Core.Entities
{
    public class PlayerTilableObject : BaseTilableObject, ITilable
    {

        [SerializeField] private bool _canCounterAttack = false;
        [SerializeField] private int _baseDamage;

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

        public void SetBox(TileBox box)
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

        public void GetDamage()
        {
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
                for (float i = 0; i < 0.5f; i += 0.01f * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, obj.Tile.transform.position,
                        i);
                    TempVector3.y = Mathf.Sin(i * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }
                obj.PlayerCallBack(PlayerCallbackType.Attack, this);
                for (float i = 0; i < 0.5f; i += 0.01f * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, obj.Tile.transform.position,
                        (0.5f - i));
                    TempVector3.y = Mathf.Sin((0.5f - i) * Mathf.PI) * _jumpHeight;
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