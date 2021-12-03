using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;



public class MainMenu : BasePopup
{
    [BoxGroup("Mainmenu buttons")]
    [SerializeField] Button btnPlay = null;
    [BoxGroup("Mainmenu buttons")]
    [SerializeField] Button btnShop = null;
    [BoxGroup("Mainmenu buttons")]
    [SerializeField] Button btnHighScore = null;

    [BoxGroup("Mainmenu views")]
    [SerializeField] ShopPopup shopPopup = null;

    #region events
    public event EventHandler EVENT_OPEN_GAMEPLAY;
    #endregion

    public override void Init()
    {
        base.Init();
        InitializeButtons();
        InitCharacterSkin();
    }

    #region Initialize buttons
    private void InitializeButtons()
    {
        btnPlay.onClick.AddListener(() => OnClickPlayButton());
        btnShop.onClick.AddListener(() => OnClickShopButton());
        btnHighScore.onClick.AddListener(() => OnClickHighScoreButton());
    }

    private void OnClickHighScoreButton()
    {
        //GooglePlayController.Instance.ShowLeaderboard();
    }

    private void OnClickShopButton()
    {
        InitializeShopPopup();
    }

    private void OnClickPlayButton()
    {
        DispatchEvent(EVENT_OPEN_GAMEPLAY, this.gameObject, EventArgs.Empty);
    }
    #region initialize views
    private void InitializeShopPopup()
    {
        shopPopup.EVENT_HIDE += OnHideShopPopup;
        shopPopup.Init();
    }

    private void OnHideShopPopup(object sender, EventArgs e)
    {
        shopPopup.EVENT_HIDE -= OnHideShopPopup;
    }
    #endregion
    #endregion
    #region private method
    private void InitCharacterSkin()
    {
        int idSelectedCharacter = GameDataManager.GetInstance().IdSelectedCharacter;
    }
    #endregion

    public override void Hide()
    {
        RemoveAllButtonsListener();
        base.Hide();
    }

    private void RemoveAllButtonsListener()
    {
        btnPlay.onClick.RemoveAllListeners();
        btnShop.onClick.RemoveAllListeners();
        btnHighScore.onClick.RemoveAllListeners();
    }
}
