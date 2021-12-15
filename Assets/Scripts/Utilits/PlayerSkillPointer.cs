using Core.Entities;
using UnityEngine;

namespace Core.UtilitsSpace
{
    public class PlayerSkillPointer: MonoBehaviour
    {
        [SerializeField] private GameObject _playerSkillInstance;
        private Vector3 _tempPos;
        private RaycastHit _hit;
        private Collider _lastGameObj;
        
        

        private void FixedUpdate()
        {
            if (_playerSkillInstance.Equals(null))
            {
                return;
            }

            _tempPos = InputController.Instance.TouchPosition(out _hit, ~(1<<7));
            if (!_hit.Equals(null))
            {
                if (_hit.collider.gameObject.layer.Equals(6) )
                {
                    if (_hit.collider.Equals(_lastGameObj))
                    {
                    }
                    else
                    {
                        CheckTile(_hit.collider.gameObject.GetComponent<TileBox>());
                        _tempPos = _hit.collider.transform.position;
                        _lastGameObj = _hit.collider;
                        _playerSkillInstance.transform.position = _tempPos;
                    }
                    
                }
                else
                {
                    _tempPos.y = 1f;
                    _playerSkillInstance.transform.position = _tempPos;
                }

            }
            else
            {
                _tempPos.y = 1f;
                _playerSkillInstance.transform.position = _tempPos;
            }

            
        }

        public void CheckTile(TileBox tile)
        {
            // if (tile)
            // {
            //     if (tile!.TileBusy)
            //     {
            //         Debug.Log("свободно");
            //     }
            //     else
            //     {
            //         Debug.Log("несвободно");
            //     }
            // }
        }
        
        public void SetSkill(GameObject skill)
        {
            _playerSkillInstance = skill;
        }

        public void DropSkill()
        {
            _playerSkillInstance = null;
        }
    }
}