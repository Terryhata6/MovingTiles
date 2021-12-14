using Core.Interfaces;
using UnityEngine;

namespace Core.Entities
{
    public class PlayerTilableObject : MonoBehaviour, ITilable
    {
        [SerializeField] private TileBox _box;
        public TileBox Box => _box;
        
        public void SetBox(TileBox box)
        {
            if (_box != null)
            {
                _box.ChangeTiledObject();
            }
            _box = box;
            box.ChangeTiledObject(this);
            transform.position = box.transform.position;
            PlayStartAnimation();
        }

        public void PlayStartAnimation()
        {
            
        }


        public void LoadStatsFormPrefs()
        {
            
        }

        public void SavePrefs()
        {
            
        }
    }
}