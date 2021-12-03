using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopPopup : BasePopup
{
    #region serializefield objects
    [BoxGroup("CharacterItem UI")]
    [SerializeField] List<CharactersItemUI> listOfCharacterItemUI = new List<CharactersItemUI>();

    [BoxGroup("RV button component")]
    [SerializeField] Button btnRV = null;
    [BoxGroup("RV button component")]
    [SerializeField] TextMeshProUGUI txtRVCoinReward = null;
    [BoxGroup("RV button component")]
    [SerializeField] TextMeshProUGUI txtTimerCountDown = null;
    [BoxGroup("RV button component")]
    [SerializeField] GameObject rootDisabled = null;
    [BoxGroup("RV button component")]
    [SerializeField] float rvCooldown = 5000f;
    #endregion

    ulong lastRVOpen;

    public override void Init()
    {
        base.Init();
        InitializeAllButtonsListener();
        InitializeCharacterItemUI();
        txtRVCoinReward.text = "<sprite name=\"icon_coin\"> " + GameDataManager.GetInstance().TotalRvRewards.ToString();
        lastRVOpen = ulong.Parse(GameDataManager.GetInstance().LastRVOpen);
        if (IsRVReady())
        {
            EnableBtnRV(true);
        }
        else
        {
            EnableBtnRV(false);
        }
    }

    private void EnableBtnRV(bool _enable)
    {
        btnRV.interactable = _enable;
        rootDisabled.SetActive(!_enable);
        Debug.Log("rv ready? "+_enable);
    }

    private void InitializeAllButtonsListener()
    {
        btnRV.onClick.AddListener(() => OnClickRV());
    }

    private void OnClickRV()
    {
        lastRVOpen = (ulong)DateTime.Now.Ticks;
        GooglePlayController.Instance.adsManager.ShowRewardedAds();
        GooglePlayController.Instance.adsManager.EVENT_HandleUserEarnedReward += OnRewardVideoComplete;
        EnableBtnRV(false);
    }

    private void OnRewardVideoComplete(object sender, EventArgs e)
    {
        Debug.Log("RV Complete");
        GameDataManager.GetInstance().TotalCoins += GameDataManager.GetInstance().TotalRvRewards;
        GameDataManager.GetInstance().IsAlreadyOpenRv = true;
        GameDataManager.GetInstance().LastRVOpen = lastRVOpen.ToString();
        GameDataManager.GetInstance().SaveGameData();
        Main.Instance.HudManager.UpdateData();
    }

    #region initialize
    private void InitializeCharacterItemUI()
    {
        for (int i = 0; i < listOfCharacterItemUI.Count; i++)
        {
            listOfCharacterItemUI[i].Init(i);
            listOfCharacterItemUI[i].EVENT_SELECT_CHARACTER += OnSelectCharacter;
        }
    }

    private void OnSelectCharacter(object sender, EventArgs e)
    {
        for (int i = 0; i < listOfCharacterItemUI.Count; i++)
        {
            listOfCharacterItemUI[i].UpdateUnlockState();
        }
    }
    #endregion

    private void RemoveAllButtonsListener()
    {
        btnRV.onClick.RemoveAllListeners();
        for (int i = 0; i < listOfCharacterItemUI.Count; i++)
        {
            listOfCharacterItemUI[i].RemoveAllButtonsListener();
            listOfCharacterItemUI[i].EVENT_SELECT_CHARACTER -= OnSelectCharacter;
        }
    }

    public override void Hide()
    {
        RemoveAllButtonsListener();
        base.Hide();
    }

    private void Update()
    {
        if (!btnRV.IsInteractable())
        {
            if (IsRVReady())
            {
                EnableBtnRV(true);
                return;
            }
            else
            {
                //set timer
                ulong diff = ((ulong)DateTime.Now.Ticks - lastRVOpen);
                ulong m = diff / TimeSpan.TicksPerMillisecond;
                float secondLeft = (float)(rvCooldown - m) / 1000;

                string textTimer = "";
                //hours
                textTimer += ((int)secondLeft / 3600).ToString() + "h";
                secondLeft -= ((int)secondLeft / 3600) * 3600;
                //minutes
                textTimer += ((int)secondLeft / 60).ToString("00") + "m";
                //seconds
                textTimer += (secondLeft % 60).ToString("00") + "s";
                txtTimerCountDown.text = textTimer;
            }
        }
    }

    private bool IsRVReady()
    {
        if (!GameDataManager.GetInstance().IsAlreadyOpenRv)
        {
            return true;
        }
        else
        {
            ulong diff = ((ulong)DateTime.Now.Ticks - lastRVOpen);
            ulong m = diff / TimeSpan.TicksPerMillisecond;
            float secondLeft = (float)(rvCooldown - m) / 1000;
            txtTimerCountDown.text = secondLeft.ToString();
            if (secondLeft < 0)
            {
                return true;
            }
            //Debug.Log("second left: " + secondLeft);
        }
        return false;
    }
}
