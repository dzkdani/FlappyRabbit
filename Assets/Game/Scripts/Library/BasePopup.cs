using System;
using UnityEngine;
using UnityEngine.UI;



public class BasePopup : CustomMonobehavior
{

    [Header("Base Popup Properties")]
    [SerializeField] Button Btn_Back = null;

    public event EventHandler EVENT_HIDE;

    public virtual void Init()
    {
        this.gameObject.SetActive(true);
        Debug.Log(gameObject.name + " - Intialized");
        IntializeButtons();
    }

    private void IntializeButtons()
    {
        if(Btn_Back != null)
        {
            Btn_Back.GetComponent<Button>().onClick.AddListener(() => Hide());
        }
    }

    private void RemoveButtonsListener()
    {
        if (Btn_Back != null)
        {
            Btn_Back.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    public virtual void Hide()
    {
        RemoveButtonsListener();
        this.gameObject.SetActive(false);
        DispatchEvent(EVENT_HIDE, this.gameObject, EventArgs.Empty);
    }
}
