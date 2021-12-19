using System;
using Core.Entities;
using Core.UtilitsSpace;
using UnityEngine;

namespace Core
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        [SerializeField] private int CurrentZone = -1;
        [SerializeField] private float _defaultBaseHp = 10;


        public void Awake()
        {
            PlayerPrefs.GetInt("CurrentZone", 0);

        }

        public void OnDeath()
        {
            SavePlayerData(_defaultBaseHp, _defaultBaseHp, -1, 0, 0);
            SceneController.Instance.SetLevelNumber(0);
        }

        public void SavePlayerData(float hp, float maxHp, int currentWeapon, int charges, int weaponDamage)
        {
            PlayerPrefs.SetFloat("StatsPlayerHp", hp);
            PlayerPrefs.SetFloat("StatsPlayerMaxHp", maxHp);
            PlayerPrefs.SetInt("StatsCurrentWeapon", currentWeapon);
            PlayerPrefs.SetInt("StatsCharges", charges);
            PlayerPrefs.SetInt("StatsCurrentWeaponDamage", weaponDamage);
        }

        public void LoadPlayerData(out float hp, out float maxHp,out int currentWeapon,out int charges, out int weaponDamage)
        {
            hp = PlayerPrefs.GetFloat("StatsPlayerHp", _defaultBaseHp);
            maxHp = PlayerPrefs.GetFloat("StatsPlayerMaxHp", _defaultBaseHp);
            currentWeapon = PlayerPrefs.GetInt("StatsCurrentWeapon", -1);
            charges = PlayerPrefs.GetInt("StatsCharges", 0);
            weaponDamage = PlayerPrefs.GetInt("StatsCurrentWeaponDamage", 1);
        }

        public void SaveCurrentZone(int currentZone)
        {
            CurrentZone = currentZone;
            PlayerPrefs.SetInt("CurrentZone", CurrentZone);
        }

        public void GetCurrentZone(out int currentZone)
        {
            CurrentZone = PlayerPrefs.GetInt("CurrentZone", 0);
            currentZone = CurrentZone;
        }

    }
}