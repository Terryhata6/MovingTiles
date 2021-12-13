using UnityEngine;

namespace Core.Entities
{
    
    public class Skill
    {
        public TileBox _tempTile;
        public bool OnCooldown = false;

        public void Execute()
        {
            OnCooldown = true;
        }

        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer.Equals(6f))
            {
                if (other.gameObject.TryGetComponent(out _tempTile))
                {
                    
                }
            }
        }
    }
}