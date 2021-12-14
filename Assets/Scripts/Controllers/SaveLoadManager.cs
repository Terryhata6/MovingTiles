using System;
using Core.UtilitsSpace;
using UnityEngine;

namespace Core
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        [SerializeField] private int CurrentLevel;


        public void Awake()
        {
            PlayerPrefs.GetInt("CurrentLevel", 0);

        }
    }
}