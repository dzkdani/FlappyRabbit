using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PausePopup : BasePopup
{
    [BoxGroup("Popup buttons")]
    [SerializeField] Button btnRestart;
    [BoxGroup("Popup buttons")]
    [SerializeField] Button btnMenu;

    #region variables
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

    private void RemoveAllButtonsListener()
    {
        btnRestart.onClick.RemoveAllListeners();
        btnMenu.onClick.RemoveAllListeners();
    }

    public override void Hide()
    {
        RemoveAllButtonsListener();
        base.Hide();
    }
}
