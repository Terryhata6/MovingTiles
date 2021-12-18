using System;
using Core.UtilitsSpace;


public class UIEvents : Singleton<UIEvents>
{
    public event Action OnButtonStartGame;
    public void ButtonStartGame()
    {
        OnButtonStartGame?.Invoke();
    }

    public event Action OnButtonPauseGame;
    public void ButtonPauseGame()
    {
        OnButtonPauseGame?.Invoke();
    }

    public event Action OnButtonResumeGame;
    public void ButtonResumeGame()
    {
        OnButtonResumeGame?.Invoke();
    }

    public event Action OnButtonRestartGame;
    public void ButtonRestartGame()
    {
        OnButtonRestartGame?.Invoke();
    }

    public event Action OnButtonNextLevel;
    public void ButtonNextLevel()
    {
        OnButtonNextLevel?.Invoke();
    }

    #region Character select menu
    public event Action OnButtonSaveAndContinue;
    public void ButtonSaveAndContinue()
    {
        OnButtonSaveAndContinue?.Invoke();
    }

    public event Action OnButtonHeadRight;
    public void ButtonHeadRight()
    {
        OnButtonHeadRight?.Invoke();
    }

    public event Action OnButtonHeadLeft;
    public void ButtonHeadLeft()
    {
        OnButtonHeadLeft?.Invoke();
    }

    public event Action OnButtonBodyRight;
    public void ButtonBodyRight()
    {
        OnButtonBodyRight?.Invoke();
    }

    public event Action OnButtonBodyLeft;
    public void ButtonBodyLeft()
    {
        OnButtonBodyLeft?.Invoke();
    }
    #endregion
}