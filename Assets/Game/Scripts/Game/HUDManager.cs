using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class HUDManager : CustomMonobehavior
{
    public enum GameState
    {
        mainmenu = 1,
        gameplay = 2
    }
    #region serializeField Objects
    [BoxGroup("HUD Data")]
    [SerializeField] TextMeshProUGUI TxtCoins;
    [BoxGroup("HUD Data")]
    [SerializeField] TextMeshProUGUI TxtScore;
    [BoxGroup("HUD Data")]
    [SerializeField] TextMeshProUGUI TxtHighScore;

    [BoxGroup("HUD Object Root")]
    [SerializeField] GameObject highscore;
    [BoxGroup("HUD Object Root")]
    [SerializeField] GameObject score;
    [BoxGroup("HUD Object Root")]
    [SerializeField] GameObject coin;

    [BoxGroup("HUD Buttons")]
    [SerializeField] Button BtnSettings;
    [BoxGroup("HUD Buttons")]
    [SerializeField] Button BtnPause;
    #endregion
    #region events
    public event EventHandler EVENT_SHOW_SETTINGS;
    public event EventHandler EVENT_PAUSE_GAME;
    #endregion
    public void Init(int _gameState)
    {

        this.gameObject.SetActive(true);
        UpdateData();
        UpdateGameState(_gameState);
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        BtnSettings.onClick.RemoveAllListeners();
        BtnPause.onClick.RemoveAllListeners();
        BtnSettings.onClick.AddListener(() => OnClickSettings());
        BtnPause.onClick.AddListener(() => OnClickPause());
    }

    private void UpdateGameState(int _gameState)
    {
        if(_gameState == (int)GameState.mainmenu)
        {
            ShowButtonSettings(true);
            ShowScore(false);
            ShowCoins(true);
        }
        else
        {
            ShowButtonSettings(false);
            ShowScore(true);
            ShowCoins(false);
        }
    }

    #region privateMethod
    private void OnClickSettings()
    {
        DispatchEvent(EVENT_SHOW_SETTINGS, this.gameObject, EventArgs.Empty);
    }

    private void OnClickPause()
    {
        DispatchEvent(EVENT_PAUSE_GAME, this.gameObject, EventArgs.Empty);
    }

    private void UpdateCoins()
    {
        TxtCoins.text = GameDataManager.GetInstance().TotalCoins.ToString();
    }

    private void UpdateHighScore()
    {
        TxtHighScore.text = "Best "+GameDataManager.GetInstance().HighScore.ToString();
    }

    public void UpdateScore(int _score)
    {
        TxtScore.text = _score.ToString();
    }
    #endregion

    #region publicMethod
    public void UpdateData()
    {
        UpdateCoins();
        UpdateHighScore();
    }
    public void ShowButtonSettings(bool _isShow)
    {
        BtnSettings.gameObject.SetActive(_isShow);
        //BtnPause.gameObject.SetActive(!_isShow);
    }
    public void ShowScore(bool _isShow)
    {
        highscore.SetActive(!_isShow);
        score.SetActive(_isShow);
    }
    public void ShowCoins(bool _isShow)
    {
        coin.gameObject.SetActive(_isShow);
    }
    #endregion
}
