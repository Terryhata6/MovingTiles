using UnityEngine;
using UnityEngine.UI;


public class UICharacterSelectMenuController : MonoBehaviour
{
    [SerializeField] private Button _buttonStartGame;
    [SerializeField] private Button _buttonHeadRight;
    [SerializeField] private Button _buttonHeadLeft;
    [SerializeField] private Button _buttonBodyRight;
    [SerializeField] private Button _buttonBodyLeft;


    private void Start()
    {
        _buttonStartGame.onClick.AddListener(UIEvents.Instance.ButtonSaveAndContinue);
        _buttonHeadRight.onClick.AddListener(UIEvents.Instance.ButtonHeadRight);
        _buttonHeadLeft.onClick.AddListener(UIEvents.Instance.ButtonHeadLeft);
        _buttonBodyRight.onClick.AddListener(UIEvents.Instance.ButtonBodyRight);
        _buttonBodyLeft.onClick.AddListener(UIEvents.Instance.ButtonBodyLeft);

        UIEvents.Instance.OnButtonSaveAndContinue += SaveAndContinue;
        UIEvents.Instance.OnButtonHeadRight += HeadRight;
        UIEvents.Instance.OnButtonHeadLeft += HeadLeft;
        UIEvents.Instance.OnButtonBodyRight += BodyRight;
        UIEvents.Instance.OnButtonBodyLeft += BodyLeft;
    }

    private void SaveAndContinue()
    {
        //TODO
    }

    private void HeadRight()
    {
        //TODO
    }
    private void HeadLeft()
    {
        //TODO
    }
    private void BodyRight()
    {
        //TODO
    }
    private void BodyLeft()
    {
        //TODO
    }

    private void OnDestroy()
    {
        _buttonStartGame.onClick.RemoveAllListeners();
        _buttonHeadRight.onClick.RemoveAllListeners();
        _buttonHeadLeft.onClick.RemoveAllListeners();
        _buttonBodyRight.onClick.RemoveAllListeners();
        _buttonBodyLeft.onClick.RemoveAllListeners();

        UIEvents.Instance.OnButtonSaveAndContinue -= SaveAndContinue;
        UIEvents.Instance.OnButtonHeadRight -= HeadRight;
        UIEvents.Instance.OnButtonHeadLeft -= HeadLeft;
        UIEvents.Instance.OnButtonBodyRight -= BodyRight;
        UIEvents.Instance.OnButtonBodyLeft -= BodyLeft;
    }
}