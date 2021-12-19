using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSkinController : MonoBehaviour
{
    [Header("CoreGO")]
    [SerializeField] private GameObject[] _coreGO;
    [Header("Head")]
    [SerializeField] private GameObject[] _allHeadSkins;
    [SerializeField] private PlayerSkinSet[] _headSets;
    [Header("Body")]
    [SerializeField] private GameObject[] _allBodySkins;
    [SerializeField] private PlayerSkinSet[] _bodySets;
    [Header("Debug")]
    [SerializeField] private int _currentHeadSet;
    [SerializeField] private int _currentBodySet;

    private string _headSetKey = "HeadSet";
    private string _bodySetKey = "BodySet";

    private RealTimeSkinnedMeshBaker _realtimeBaker;
    private List<GameObject> _objectsForBake = new List<GameObject>();


    private void Awake()
    {
        _realtimeBaker = GetComponentInChildren<RealTimeSkinnedMeshBaker>();
        if (SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            LoadSkinPreset(false);
        }
        else
        {
            LoadSkinPreset(true);
        }
    }


    public void SaveSkinPreset()
    {
        PlayerPrefs.SetInt(_headSetKey, _currentHeadSet);
        PlayerPrefs.SetInt(_bodySetKey, _currentBodySet);
    }

    public void LoadSkinPreset(bool isBake)
    {
        _currentHeadSet = PlayerPrefs.GetInt(_headSetKey);
        _currentBodySet = PlayerPrefs.GetInt(_bodySetKey);

        ChangeHeadSkin(_currentHeadSet);
        ChangeBodySkin(_currentBodySet);

        if (isBake)
        {
            _objectsForBake.Clear();
            _objectsForBake.AddRange(_coreGO);
            _objectsForBake.AddRange(_headSets[_currentHeadSet].SkinSet);
            _objectsForBake.AddRange(_bodySets[_currentBodySet].SkinSet);
            StartCoroutine(_realtimeBaker.StartBaking(_objectsForBake.ToArray()));
        }
    }

    private void ChangeHeadSkin(int id)
    {
        for (int i = 0; i < _allHeadSkins.Length; i++)
        {
            _allHeadSkins[i].SetActive(false);
        }

        for (int i = 0; i < _headSets[id].SkinSet.Length; i++)
        {
            _headSets[id].SkinSet[i].SetActive(true);
        }
    }

    private void ChangeBodySkin(int id)
    {
        for (int i = 0; i < _allBodySkins.Length; i++)
        {
            _allBodySkins[i].SetActive(false);
        }

        for (int i = 0; i < _bodySets[id].SkinSet.Length; i++)
        {
            _bodySets[id].SkinSet[i].SetActive(true);
        }
    }

    public void NextHeadSkin()
    {
        _currentHeadSet++;

        if (_currentHeadSet >= _headSets.Length)
        {
            _currentHeadSet = 0;
        }

        ChangeHeadSkin(_currentHeadSet);
    }
    public void PreviousHeadSkin()
    {
        _currentHeadSet--;

        if (_currentHeadSet < 0)
        {
            _currentHeadSet = _headSets.Length - 1;
        }

        ChangeHeadSkin(_currentHeadSet);
    }
    public void NextBodySet()
    {
        _currentBodySet++;

        if (_currentBodySet >= _bodySets.Length)
        {
            _currentBodySet = 0;
        }

        ChangeBodySkin(_currentBodySet);
    }
    public void PreviousBodySet()
    {
        _currentBodySet--;

        if (_currentBodySet < 0)
        {
            _currentBodySet = _bodySets.Length - 1;
        }

        ChangeBodySkin(_currentBodySet);
    }
}