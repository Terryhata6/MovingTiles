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

        public virtual void Awake()
        {
            
        }

        public virtual string CompareConfig(BaseTilableObject obj)
        {
            return _config;
        }
        
        protected virtual IEnumerator InteractionWithPlayer(TileBox box, TurnState state)
        {
            yield break;
        }

        public virtual void SetBox(TileBox box)
        {
            if (box == null)
            {
                Debug.Log($"{this.GetType()}was destroyed becouse don't have tile");
                Destroy(this.gameObject);
                return;
            }


            if (_currentTileBox != null)
            {
                if (_currentTileBox.TiledObject == this)
                {
                    _currentTileBox.ChangeTiledObject();
                }
            }
            
            transform.parent = box.transform;
            _currentTileBox = box;
            box.ChangeTiledObject(this);

        }
        
        public virtual void CallbackForPlayerMoves(PlayerCallbackType callbackType, PlayerTilableObject player)
        {
            switch (callbackType)
            {
                case PlayerCallbackType.Pickup:
                    break;
                case PlayerCallbackType.Attack:
                    break;
                case PlayerCallbackType.Exit: //Enter-alt
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

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }

    public enum PlayerCallbackType
    {
        Pickup,
        Attack,
        Exit //Enter-alt
    }
}
