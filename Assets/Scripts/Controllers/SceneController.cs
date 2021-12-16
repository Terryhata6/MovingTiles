using System;
using System.Collections;
using System.Collections.Generic;
using Core.UtilitsSpace;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneController : Singleton<SceneController>
    {
        [SerializeField] private List<string> _scenes;
        [SerializeField] private bool LevelDebug = false;
        [SerializeField] private LevelController _currentLevelController;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            UIEvents.Instance.OnButtonNextLevel += LoadNextScene;
            GameEvents.Instance.OnRestartLevel += ReloadScene;

            if (!LevelDebug)
            {
                LoadLevelScene(_scenes[PlayerPrefs.GetInt("PLayerLevel", 0)]);
            }
            else
            {
                FindLevelController();
            }
        }

        
        public void LoadNextScene()
        {
            if (_currentLevelController != null)
            {
                UIEvents.Instance.OnButtonStartGame -= _currentLevelController.LevelStart;
            }

            var currentLevelNumber = PlayerPrefs.GetInt("PlayerLevel");
            SceneManager.UnloadSceneAsync(_scenes[currentLevelNumber]);
            PlayerPrefs.SetInt("PlayerLevel", currentLevelNumber + 1);

            LoadLevelScene(_scenes[PlayerPrefs.GetInt("PlayerLevel")]);
        }

        public void ReloadScene()
        {
            if (_currentLevelController != null)
            {
                UIEvents.Instance.OnButtonStartGame -= _currentLevelController.LevelStart;
            }
            var currentLevelNumber = PlayerPrefs.GetInt("PlayerLevel");
            SceneManager.UnloadSceneAsync(_scenes[currentLevelNumber]);
            LoadLevelScene(_scenes[currentLevelNumber]);
        }

        
        public void LoadLevelScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName,LoadSceneMode.Additive);
            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => FindLevelController();
            
        }

        public void FindLevelController()
        {
            _currentLevelController = LevelController.Instance;
            //_currentLevelController =  FindObjectOfType<LevelController>();
            if (_currentLevelController != null)
            {
                UIEvents.Instance.OnButtonStartGame += _currentLevelController.LevelStart;
                _currentLevelController.Initialize();
            }
            else
            {
                Debug.Log("LevelController not found");
            }
        }
    }
}