using System.Collections.Generic;
using Core;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private List<BaseMenuView> _menues = new List<BaseMenuView>();


    public void Awake()
    {
        UIEvents.Instance.OnButtonStartGame += StartGame;
        UIEvents.Instance.OnButtonPauseGame += PauseGame;
        UIEvents.Instance.OnButtonResumeGame += StartGame;
        //UIEvents.Instance.OnButtonNextLevel += NextLevel;
        UIEvents.Instance.OnButtonRestartGame += RestartGame;
        GameEvents.Instance.OnLevelVictory += WinGame;
        GameEvents.Instance.OnLevelFailed += LoseGame;
        GameEvents.Instance.OnLoadNewLevelController += OpenGameMenu;
    }

    private void Start()
    {
        
    }

    private void OpenGameMenu(LevelController controller)
    {
        SwitchUI(UIState.MainMenu);
    }

    private void StartGame()
    {
        Time.timeScale = 1.0f;
        SwitchUI(UIState.InGame);
    }

    private void PauseGame()
    {
        Time.timeScale = 0.0f;
        SwitchUI(UIState.Pause);
    }

    private void WinGame()
    {
        Time.timeScale = 0.0f;
        SwitchUI(UIState.WinMenu);
    }

    private void LoseGame()
    {
        Time.timeScale = 0.0f;
        SwitchUI(UIState.LoseMenu);
    }

    private void NextLevel()
    {
        SwitchUI(UIState.MainMenu);
    }

    private void RestartGame()
    {
        GameEvents.Instance.RestartLevel();
        SwitchUI(UIState.MainMenu);
    }

    public void AddView(BaseMenuView view)
    {
        _menues.Add(view);
    }

    #region Switch
    private void SwitchUI(UIState state)
    {
        if (_menues.Count == 0)
        {
            Debug.LogWarning("There is no menues to switch.");
        }
        switch (state)
        {
            case UIState.MainMenu:
                SwitchMenu(typeof(MainMenuView));
                break;
            case UIState.InGame:
                SwitchMenu(typeof(InGameUIView));
                break;
            case UIState.Pause:
                SwitchMenu(typeof(PauseMenuView));
                break;
            case UIState.WinMenu:
                SwitchMenu(typeof(WinMenuView));
                break;
            case UIState.LoseMenu:
                SwitchMenu(typeof(LoseMenuView));
                break;
        }
    }
    private void SwitchMenu(System.Type type)
    {
        bool isFound = false;

        for (int i = 0; i < _menues.Count; i++)
        {
            if (_menues[i].GetType() == type)
            {
                _menues[i].Show();
                isFound = true;
            }
            else
            {
                _menues[i].Hide();
            }

            if (i == _menues.Count - 1f && !isFound)
            {
                Debug.LogWarning($"Oops! Menu {type} not found");
            }
        }
    }
    #endregion
}