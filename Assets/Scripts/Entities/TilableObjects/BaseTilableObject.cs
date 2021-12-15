using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Entities
{
    public abstract class BaseTilableObject : MonoBehaviour
    {
        [SerializeField] [Tooltip("INDEVELOPMENT")]
        protected string _config;
        [SerializeField] protected float _jumpHeight = .3f;
        [SerializeField] protected float _jumpSpeed = 2f;
        [SerializeField] protected TileBox _currentTileBox;
        public string Config => _config;
        public TileBox Tile => _currentTileBox;
        protected Vector3 TempVector3;
        
        public virtual string CompareConfig(BaseTilableObject obj)
        {
            return _config;
        }
        
        protected virtual IEnumerator PlayerInteraction(TileBox box, TurnState state)
        {
            yield break;
        }

        public void SetBox(TileBox box)
        {
            if (box == null)
            {
                Debug.Log($"{this.GetType()}was destroyed becouse don't have tile");
                Destroy(this.gameObject);
                return;
            }


            if (_currentTileBox != null)
            {
                _currentTileBox.ChangeTiledObject();
            }

            _currentTileBox = box;
            box.ChangeTiledObject(this);
            
        }
        
        public virtual void PlayerCallBack(PlayerCallbackType callbackType, PlayerTilableObject player)
        {
            switch (callbackType)
            {
                case PlayerCallbackType.Pickup:
                    break;
                case PlayerCallbackType.Attack:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(callbackType), callbackType, null);
            }
        }

        public virtual IEnumerator DestroyAnimation()
        {
            yield break;
        }

        public virtual IEnumerator SpawnAnimation([CanBeNull]Action<BaseTilableObject> OnEndSpawn)
        {
            
            OnEndSpawn?.Invoke(this);
            yield break;
        }
    }

    public enum PlayerCallbackType
    {
        Pickup,
        Attack
    }
}
