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
    }
}