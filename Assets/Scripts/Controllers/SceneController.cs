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

        [Header("DraggableUI")] [SerializeField]
        private DragUITilableObject[] draggableUi;

        [SerializeField] private TilableObject[] obj;
        [SerializeField] private Sprite[] objImage;


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
            var j = Random.Range(0, obj.Length);
            for (int i = 0; i < draggableUi.Length; i++)
            {
                if (draggableUi[i].ItsFree)
                {
                    if (obj[j] != null && objImage[j] != null)
                    {
                        draggableUi[i].SetNewObject(objImage[j], obj[j]);
                    }

                    return;
                }
                else if (draggableUi[i].CompareObject(obj[j]))
                {
                    draggableUi[i].AddCharge(1);
                    return;
                }
            }

            if (obj[j] != null && objImage[j] != null)
            {
                draggableUi[0].SetNewObject(objImage[j], obj[j]);
            }
        }

        public void RestartDragableObjects()
        {
            for (int i = 0; i < draggableUi.Length; i++)
            {
                draggableUi[i].Restart();
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            for (int i = 0; i < draggableUi.Length; i++)
            {
                draggableUi[i].gameObject.SetActive(true);
            }
        }

        private void Start()
        {
            UIEvents.Instance.OnButtonNextLevel += LoadNextScene;
            UIEvents.Instance.OnButtonNextLevel += RestartDragableObjects;
            GameEvents.Instance.OnRestartLevel += ReloadScene;
            GameEvents.Instance.OnRestartLevel += RestartDragableObjects;
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
            var currentLevelNumber = PlayerPrefs.GetInt("CurrentZone", defaultValue: 0);

            if (currentLevelNumber >= _scenes.Count)
            {
                currentLevelNumber = 0; //todo change to EndScene
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


        public void LoadLevelScene(int sceneNumber)
        {
            string sceneName;

            if (sceneNumber < _scenes.Count)
            {
                sceneName = _scenes[sceneNumber];
                SetLevelNumber(0);
            }
            else
            {
                sceneName = _endScene;
            }
            _loader.DestinationSceneName = sceneName;
            _MMFeedBacks.PlayFeedbacks();
            if (sceneName == _characterSelectScene)
            {
                return;
            }
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