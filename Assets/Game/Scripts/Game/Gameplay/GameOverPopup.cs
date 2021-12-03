using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameOverPopup : BasePopup
{
    [BoxGroup("Rewards attribute")]
    [SerializeField] TextMeshProUGUI txtScoreValue;
    [BoxGroup("Rewards attribute")]
    [SerializeField] TextMeshProUGUI txtCoinsReward;
    [BoxGroup("Rewards attribute")]
    [SerializeField] GameObject highScoreTag;


    [BoxGroup("Popup buttons")]
    [SerializeField] Button btnShare;
    [BoxGroup("Popup buttons")]
    [SerializeField] Button btnRestart;
    [BoxGroup("Popup buttons")]
    [SerializeField] Button btnMenu;

    #region variables
    private bool isFirstTimeDie = false;
    #endregion

    #region events
    public event EventHandler EVENT_RESTART_GAME;
    public event EventHandler EVENT_QUIT_GAME;
    #endregion
    public override void Init()
    {
        base.Init();
        InitalizeAllButtonsListener();
    }

    private void InitalizeAllButtonsListener()
    {
        btnShare.onClick.AddListener(() => OnClickShare());
        btnRestart.onClick.AddListener(() => OnClickRestart());
        btnMenu.onClick.AddListener(() => OnCLickMenu());
    }

    private void OnCLickMenu()
    {
        Debug.Log("back to menu 1");
        DispatchEvent(EVENT_QUIT_GAME, this.gameObject, EventArgs.Empty);
    }

    private void OnClickRestart()
    {
        DispatchEvent(EVENT_RESTART_GAME, this.gameObject, EventArgs.Empty);
    }

    private void OnClickShare()
    {
        throw new NotImplementedException();
    }

    public void Initialize(int _score, int _coin, bool _isNewHighscore)
    {
        Init();
        txtScoreValue.text = _score.ToString();
        txtCoinsReward.text = _coin.ToString();
        highScoreTag.SetActive(_isNewHighscore);
    }

    private void RemoveAllButtonsListener()
    {
        btnShare.onClick.RemoveAllListeners();
        btnRestart.onClick.RemoveAllListeners();
        btnMenu.onClick.RemoveAllListeners();
    }

    public override void Hide()
    {
        RemoveAllButtonsListener();
        base.Hide();
    }
}
