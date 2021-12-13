using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Entities
{
    public class BaseTilableObject : MonoBehaviour
    {
        [SerializeField][Tooltip("INDEVELOPMENT")]private string _config;
        [SerializeField] private bool _canMove;
        [SerializeField] private bool _haveSkills = false;
        [SerializeField] private List<Skill> _skills = new List<Skill>();
        [SerializeField] private TileBox _currentTileBox;
        public bool HaveSkills => _haveSkills;

        public void Awake()
        {
            if (_skills.Count > 0)
            {
                _haveSkills = true;
            }
        }

        public void Move()
        {
            if (!_canMove)
            {
                return;
            }
            else
            {
                
            }
        }

        public void MoveToBox(TileBox box)
        {
            //MoveToBoxWithAnimation
        }

        public void SetBox(TileBox box)
        {
            //MovetoBoxinstantly
        }

        public void ExecuteSkill()
        {
            if (_haveSkills)
            {
                for (int i = 0; i < _skills.Count; i++)
                {
                    if (!_skills[i].OnCooldown)
                    {
                        _skills[i].Execute();
                        break;
                    }
                }
            }
        }
    }

    
}