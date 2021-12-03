using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : BasePopup
{
    [BoxGroup("Settings buttons")]
    [SerializeField] Button btnSound = null;
    [BoxGroup("Settings buttons")]
    [SerializeField] Button btnRateUs = null;
    [BoxGroup("Settings buttons")]
    [SerializeField] Button btnCredits = null;

    [BoxGroup("Sound icon ")]
    [SerializeField] Image ImgIconSound = null;
    [BoxGroup("Sound icon ")]
    [SerializeField] Sprite ImgIconSoundOn = null;
    [BoxGroup("Sound icon ")]
    [SerializeField] Sprite ImgIconSoundOff = null;


    [BoxGroup("Settings views")]
    [SerializeField] BasePopup creditsPopup = null;

    private bool isMuteSound = false;

    #region events
    public event EventHandler EVENT_OPEN_CREDITS;
    #endregion

    public override void Init()
    {
        base.Init();
        UpdateSoundStateIcon();
        InitializeButtons();
    }

    #region Initialize buttons
    private void InitializeButtons()
    {
        btnSound.onClick.AddListener(() => OnClickSoundButton());
        btnRateUs.onClick.AddListener(() => OnClickRateUsButton());
        btnCredits.onClick.AddListener(() => OnClickCreditsButton());
    }

    private void OnClickCreditsButton()
    {
        InitializeCredits();
    }

    private void OnClickRateUsButton()
    {
        //Application.OpenURL("https://play.google.com/store/apps/details?id=com.siakew.WonderWord");
    }

    private void OnClickSoundButton()
    {
        OnToggleSound();
    }

    private void OnToggleSound()
    {
        //SoundManager.Instance.PlayMusic();
        if (isMuteSound)
        {
            isMuteSound = false;
        }
        else
        {
            isMuteSound = true;
        }
        SoundManager.Instance.SetSoundState(isMuteSound);
        UpdateSoundStateIcon();
        Debug.Log("Sound " + isMuteSound);
    }


    private void UpdateSoundStateIcon()
    {
        if (isMuteSound)
        {
            ImgIconSound.sprite = ImgIconSoundOff;
        }
        else
        {
            ImgIconSound.sprite = ImgIconSoundOn;
        }
    }
    #endregion

    #region initialize
    private void InitializeCredits()
    {
        creditsPopup.EVENT_HIDE += OnHideCredits;
        creditsPopup.Init();
    }

    private void OnHideCredits(object sender, EventArgs e)
    {
        creditsPopup.EVENT_HIDE -= OnHideCredits;
    }

    #endregion

    public override void Hide()
    {
        RemoveAllButtonsListener();
        base.Hide();
    }

    private void RemoveAllButtonsListener()
    {
        btnSound.onClick.RemoveAllListeners();
        btnRateUs.onClick.RemoveAllListeners();
        btnCredits.onClick.RemoveAllListeners();
    }
}
