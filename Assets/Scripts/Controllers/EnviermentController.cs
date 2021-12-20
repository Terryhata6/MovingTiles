using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UtilitsSpace;
using UnityEngine;

public class EnviermentController : Singleton<EnviermentController>
{
    [SerializeField] private List<EnvironmentSet> GOSets;
    [SerializeField] private Skybox _customSkybox;
    [SerializeField] private GameObject _arrows;

    void Awake()
    {
        foreach (var set in GOSets)
        {
            set.DeactivateSet();
        }

        _customSkybox = Camera.main.transform.GetComponent<Skybox>();
        
    }

    public void DeactivateAll()
    {
        foreach (var set in GOSets)
        {
            set.DeactivateSet();
        }
        _arrows.SetActive(false);
        
    }

    public void ActivateSet(EnviSetType setTypeType)
    {
        int index;
        for (index = 0; index < GOSets.Count; index++)
        {
            GOSets[index].DeactivateSet();
        }
        _arrows.SetActive(true);
        for (index = 0; index < GOSets.Count; index++)
        {
            if (GOSets[index].setTypeType.Equals(setTypeType))
            {
                GOSets[index].ActivateSet();
                if (GOSets[index].skyboxMaterial != null)
                {
                    _customSkybox.material = GOSets[index].skyboxMaterial;
                }

                return;
            }
        }
        
    }
}

[Serializable]
public struct EnvironmentSet
{
    public EnviSetType setTypeType;
    public List<GameObject> _gos;
    public  Material skyboxMaterial;
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
