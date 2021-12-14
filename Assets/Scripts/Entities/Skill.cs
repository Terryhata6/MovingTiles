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

    }
}