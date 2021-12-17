using System;
using System.Collections;
using Core.Entities;
using UnityEngine;

namespace Core.UtilitsSpace
{
    public class PlayerSkillPointer
    {
        private Vector3 _tempPos;
        private RaycastHit _hit;
        private Collider _lastGameObj;
        private bool _skillCanSnap;
        private bool _pointSkill = false;
        private TileBox _tile;



        public IEnumerator PointSkill(TilableObject _playerSkill, Action onEndSpawningCallback) //eNTER-ALT
        {
            _pointSkill = true;
            while (_pointSkill)
            {
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
                            _tile = _hit.collider.gameObject.GetComponent<TileBox>();
                            CheckTile(_tile);
                            _tempPos = _hit.collider.transform.position;
                            _lastGameObj = _hit.collider;
                            _playerSkill.transform.position = _tempPos;
                        }
                    
                    }
                    else
                    {
                        _skillCanSnap = false;
                        _lastGameObj = null;
                        _tempPos.y = 1f;
                        _playerSkill.transform.position = _tempPos;
                    }

                }
                else
                {
                    _skillCanSnap = false;
                    _lastGameObj = null;
                    _tempPos.y = 1f;
                    _playerSkill.transform.position = _tempPos;
                }
                yield return null;
            }
            if (_skillCanSnap)
            {
                _playerSkill.SetBox(_tile);
                _playerSkill.StartCoroutine(_playerSkill.SpawnAnimation((value) =>
                {
                    TilableObjectsController.Instance.AddObjectToList(value as TilableObject);
                }));
            }
            else
            {
                GameObject.Destroy(_playerSkill.gameObject);
            }
            onEndSpawningCallback?.Invoke();
        }

        public void CheckTile(TileBox tile)
        {
            if (tile)
            {
                if (tile!.TileBusy)
                {
                    _skillCanSnap = false;
                    Debug.Log("несвободно");
                }
                else
                {
                    _skillCanSnap = true;
                    Debug.Log("свободно");
                }
            }
        }
        

        public void DropSkill()
        {
            _pointSkill = false;
        }
        
        
    }
}