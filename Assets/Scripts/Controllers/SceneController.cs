using System;
using System.Collections;
using System.Collections.Generic;
using Core.Entities;
using Core.UtilitsSpace;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core
{
    public class SceneController : Singleton<SceneController>
    {
        [SerializeField] private string _characterSelectScene;
        [SerializeField] private string _endScene;
        [SerializeField] private List<string> _scenes;
        [SerializeField] private bool LevelDebug = false;
        [SerializeField] private LevelController _currentLevelController;
        
        [SerializeField] private MMFeedbacks _MMFeedBacks;
        [SerializeField] private MMFeedbackLoadScene _loader;
        [SerializeField] private MMFeedbackUnloadScene _unloader;
        [SerializeField] private MMSceneLoadingManager _sceneLoadingManager;

        [SerializeField] private Material _spaceMaterial;
        [SerializeField] private Material _spaceBassMaterial;
        private Vector2 deltaVector = Vector2.one;

        [Header("DraggableUI")][SerializeField] private DragUITilableObject[] draggableUi;
        [SerializeField] private TilableObject[] obj;
        [SerializeField] private Image[] objImage;
        
        
        private void FixedUpdate()
        {
            if (_spaceMaterial == null)
            {
                
            }
            else
            {
                _spaceMaterial.mainTextureOffset += deltaVector * Time.fixedDeltaTime;
            }
        }

        public void GetNewDraggableObject()
        {
            for (int i = 0; i < draggableUi.Length; i++)
            {
                if (draggableUi[i].ItsFree)
                {
                    var j = Random.Range(0, obj.Length);
                    if (obj[j] != null && objImage[j] != null)
                    {
                        draggableUi[i].SetNewObject(objImage[j], obj[j]);
                    }

                    return;
                }
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            UIEvents.Instance.OnButtonNextLevel += LoadNextScene;
            GameEvents.Instance.OnRestartLevel += ReloadScene;
            GameEvents.Instance.OnGetDraggableDrop += GetNewDraggableObject;

            _MMFeedBacks = GetComponent<MMFeedbacks>();
            _loader = _MMFeedBacks.GetComponent<MMFeedbackLoadScene>();
            _unloader = _MMFeedBacks.GetComponent<MMFeedbackUnloadScene>();
            _sceneLoadingManager = GetComponent<MMSceneLoadingManager>();
            _MMFeedBacks.Initialization();
            
            
            if (!LevelDebug)
            {
                LoadLevelScene(GetLevelNumber());
            }
            else
            {
                FindLevelController();
            }
            
        }

        public int GetLevelNumber()
        {
            var currentLevelNumber = PlayerPrefs.GetInt("CurrentZone", defaultValue:-1);

            if (currentLevelNumber >= _scenes.Count)
            {
                currentLevelNumber = -1; //todo change to EndScene
                SetLevelNumber(currentLevelNumber);
            }
            return currentLevelNumber;
        }

        public void SetLevelNumber(int number)
        {
            if (number >= _scenes.Count)
            {
                number = 0;
            }
            PlayerPrefs.SetInt("CurrentZone", number);
        }

        public void ReloadScene()
        {
            if (_currentLevelController != null)
            {
                UIEvents.Instance.OnButtonStartGame -= _currentLevelController.LevelStart;
            }
            LoadLevelScene(GetLevelNumber());
        }
        
        public void LoadNextScene()
        {
            if (_currentLevelController != null)
            {
                UIEvents.Instance.OnButtonStartGame -= _currentLevelController.LevelStart;
            }

            var currentLevelNumber = GetLevelNumber();
            currentLevelNumber += 1;
            SetLevelNumber(currentLevelNumber);
            LoadLevelScene(currentLevelNumber);
        }

        public delegate void method(Scene scene, LoadSceneMode mode); 
        public void LoadLevelScene(int sceneNumber)
        {
            string sceneName;
            if (sceneNumber == -1)
            {
                sceneName = _characterSelectScene;
            }
            else
            {
                if (sceneNumber < _scenes.Count)
                {
                    sceneName = _scenes[sceneNumber];
                }
                else
                {
                    sceneName = _endScene;
                }
            }
            _loader.DestinationSceneName = sceneName;
            _MMFeedBacks.PlayFeedbacks();
            GameEvents.Instance.OnLoadNewLevelController += FindLevelController;
        }

        public void FindLevelController(LevelController controller)
        {
            _currentLevelController = controller;
            //_currentLevelController =  FindObjectOfType<LevelController>();
            if (_currentLevelController != null)
            {
                GameEvents.Instance.OnLoadNewLevelController -= FindLevelController;
                UIEvents.Instance.OnButtonStartGame += _currentLevelController.LevelStart;
                _currentLevelController.Initialize();
            }
            else
            {
                Debug.Log("LevelController not found");
            }
        }
        
        public void FindLevelController()
        {
            FindLevelController(LevelController.Instance);
        }

        
    }
}