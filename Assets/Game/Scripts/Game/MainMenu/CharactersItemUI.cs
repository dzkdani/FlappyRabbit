using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharactersItemUI : CustomMonobehavior
{
    #region serializefield objects
    [BoxGroup("Character Item Properties")]
    [SerializeField] Image imgCharacters = null;
    [BoxGroup("Character Item Properties")]
    [SerializeField] Button btnUnlockCharacters = null;
    [BoxGroup("Character Item Properties")]
    [SerializeField] Button btnSelectCharacters = null;
    [BoxGroup("Character Item Properties")]
    [SerializeField] GameObject rootEquiped = null;
    [BoxGroup("Character Item Properties")]
    [SerializeField] TextMeshProUGUI txtCostToUnlock = null;
    #endregion
    private bool isUnlocked = false;
    int idCharacter;

    #region events
    public event EventHandler EVENT_SELECT_CHARACTER;
    #endregion

    public void Init(int _idCharacter)
    {
        Debug.Log(_idCharacter);
        idCharacter = _idCharacter;
        imgCharacters.sprite = CharacterDatabase.Instance.GetCharacterData(idCharacter).SpriteCharacter;
        Debug.Log("SA : " + txtCostToUnlock.text);
        txtCostToUnlock.text = "<sprite name=\"icon_coin\"> " + CharacterDatabase.Instance.GetCharacterData(idCharacter).CostToUnlock.ToString();
        UpdateUnlockState();
    }

    public void UpdateUnlockState()
    {
        isUnlocked = GameDataManager.GetInstance().IsCharacterUnlocked(idCharacter);
        if (isUnlocked)
        {
            if (idCharacter == GameDataManager.GetInstance().IdSelectedCharacter)
            {
                rootEquiped.SetActive(true);
                btnSelectCharacters.gameObject.SetActive(false);
            }
            else
            {
                rootEquiped.SetActive(false);
                btnSelectCharacters.gameObject.SetActive(true);
                btnSelectCharacters.onClick.AddListener(() => SelectCharacter());
            }
            btnUnlockCharacters.gameObject.SetActive(false);
        }
        else
        {
            rootEquiped.SetActive(false);
            btnSelectCharacters.gameObject.SetActive(false);
            btnUnlockCharacters.gameObject.SetActive(true);
            btnUnlockCharacters.onClick.AddListener(() => OnClickUnlock());
        }
    }

    private void SelectCharacter()
    {
        GameDataManager.GetInstance().IdSelectedCharacter = idCharacter;
        GameDataManager.GetInstance().SaveGameData();
        DispatchEvent(EVENT_SELECT_CHARACTER, this.gameObject, EventArgs.Empty);
    }

    private void OnClickUnlock()
    {
        int cost = CharacterDatabase.Instance.GetCharacterData(idCharacter).CostToUnlock;
        int currentCoins = GameDataManager.GetInstance().TotalCoins;
        if(currentCoins >= cost)
        {
            GameDataManager.GetInstance().TotalCoins -= cost;
            GameDataManager.GetInstance().UnlockCharacter(idCharacter);
            UpdateUnlockState();
            Debug.Log("Unlock character");
            GameDataManager.GetInstance().SaveGameData();
            Main.Instance.HudManager.UpdateData();
        }
        else
        {
            //insuficient coins
        }
    }

    public void RemoveAllButtonsListener()
    {
        btnSelectCharacters.onClick.RemoveAllListeners();
        btnUnlockCharacters.onClick.RemoveAllListeners();
    }

}
