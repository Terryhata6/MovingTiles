using System;
using System.Collections;
using System.Collections.Generic;
using Core.UtilitsSpace;
using UnityEngine;

public class EnviermentController : Singleton<EnviermentController>
{
    [SerializeField] private List<EnvironmentSet> GOSets;
    
    void Awake()
    {
        foreach (var set in GOSets)
        {
            set.DeactivateSet();
        }
    }

    public void ActivateSet(EnviSetType setTypeType)
    {
        int index;
        for (index = 0; index < GOSets.Count; index++)
        {
            GOSets[index].DeactivateSet();
        }

        for (index = 0; index < GOSets.Count; index++)
        {
            if (GOSets[index].setTypeType.Equals(setTypeType))
            {
                GOSets[index].ActivateSet();
            }
        }
    }
}

[Serializable]
public struct EnvironmentSet
{
    public EnviSetType setTypeType;
    public List<GameObject> _gos;
    
    public void ActivateSet()
    {
        for (int i = 0; i < _gos.Count; i++)
        {
            _gos[i].SetActive(true);
        }
    }

    public void DeactivateSet()
    {
        for (int i = 0; i < _gos.Count; i++)
        {
            _gos[i].SetActive(false);
        }
    }
}

public enum EnviSetType
{
    Grass,
    GrassAndStone,
    Stone
}
