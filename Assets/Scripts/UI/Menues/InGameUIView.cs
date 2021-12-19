using Core;
using Core.Entities;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InGameUIView : BaseMenuView
{
    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private Button _buttonPause;

    [SerializeField] private MMProgressBar _healthBar;
    private UIController _uiController;


    private void Awake()
    {
        _buttonPause.onClick.AddListener(UIEvents.Instance.ButtonPauseGame);
        FindMyController();
        _healthBar.Initialization();
    }

    private void FindMyController()
    {
        _uiController = transform.parent.GetComponent<UIController>();
        _uiController.AddView(this);
    }

    public override void Hide()
    {
        if (!IsShow) return;
        _panel.gameObject.SetActive(false);
        GameEvents.Instance.OnPlayerHpChange -= SetHp;
        IsShow = false;
    }

    public override void Show()
    {
        if (IsShow) return;

        #region debug
        GameEvents.Instance.OnPlayerHpChange += SetHp;
        #endregion
        _panel.gameObject.SetActive(true);
        IsShow = true;
    }

    public void SetHp(float currentValue, float minValue, float maxValue)
    {
        _healthBar.UpdateBar(currentValue, minValue, maxValue);
    }

    private void OnDestroy()
    {
        _buttonPause.onClick.RemoveAllListeners();
    }
}