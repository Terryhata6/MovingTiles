using Core.Entities;
using UnityEngine;

namespace Core.UtilitsSpace
{
    public class SkillPointer: MonoBehaviour
    {

        [SerializeField] private GameObject _skill;
        private Vector3 _tempPos;
        private RaycastHit _hit;
        private Collider _tempCollider;
        [SerializeField] private LayerMask _layerMask;
        private void Start()
        {
        }

        private void FixedUpdate()
        {
            if (_skill.Equals(null))
            {
                return;
            }

            _tempPos = InputController.Instance.TouchPosition(out _hit, ~(1<<7));
            if (!_hit.Equals(null))
            {
                if (_hit.collider.gameObject.layer.Equals(6))
                {
                   // CheckTile(_tempCollider.gameObject.GetComponent<TileBox>());
                    _tempPos = _hit.collider.transform.position + Vector3.up * _hit.collider.bounds.extents.y;
                }
            }
            else
            {
                _tempPos.y = 1f;
            }

            _skill.transform.position = _tempPos;
        }

        public void CheckTile(TileBox tile)
        {
            if (tile)
            {
                if (tile!._tileBusy)
                {
                    Debug.Log("свободно");
                }
                else
                {
                    Debug.Log("несвободно");
                }
            }
        }
        
        public void SetSkill(GameObject skill)
        {
            _skill = skill;
        }

        public void DropSkill()
        {
            _skill = null;
        }
    }
}