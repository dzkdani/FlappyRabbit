using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Character : CustomMonobehavior
{
    [BoxGroup("Character parts sprite")]
    [SerializeField] SpriteRenderer characterHead = null;
    [BoxGroup("Character parts sprite")]
    [SerializeField] SpriteRenderer characterBody = null;
    [BoxGroup("Character parts sprite")]
    [SerializeField] SpriteRenderer characterWingFront = null;
    [BoxGroup("Character parts sprite")]
    [SerializeField] SpriteRenderer characterWingBack = null;

    Rigidbody2D characterRigidbody2D;
    private int idCharacter;
    private float jumpForceValue;
    private float gravityScale;
    private bool isDeath;
    private bool isReady;
    public float JumpForceValue { get => jumpForceValue; set => jumpForceValue = value; }
    public bool IsReady { get => isReady; set => isReady = value; }
    #region events
    public event EventHandler EVENT_DEATH;
    #endregion

    public void Init(int _idCharacter, float _jumpForceValue)
    {
        this.GetComponent<Animator>().enabled = true;
        this.GetComponent<CapsuleCollider2D>().isTrigger = true;
        characterRigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        gravityScale = characterRigidbody2D.gravityScale;
        characterRigidbody2D.gravityScale = 0;
        idCharacter = _idCharacter;
        jumpForceValue = _jumpForceValue;
        isDeath = false;
        IsReady = false;
        SetupCharacterSprites();
    }

    private void SetupCharacterSprites()
    {
        characterHead.sprite = CharacterDatabase.Instance.GetCharacterData(idCharacter).SpriteCharacterHead;
        characterBody.sprite = CharacterDatabase.Instance.GetCharacterData(idCharacter).SpriteCharacterBody;
        characterWingFront.sprite = CharacterDatabase.Instance.GetCharacterData(idCharacter).SpriteCharacterWing;
        characterWingBack.sprite = CharacterDatabase.Instance.GetCharacterData(idCharacter).SpriteCharacterWing;
    }

    public void SetCharacterReady()
    {
        characterRigidbody2D.gravityScale = gravityScale;
    }

    public void Jump()
    {
        characterRigidbody2D.velocity = Vector2.up * jumpForceValue;
        //characterRigidbody2D.AddTorque(2, ForceMode2D.Impulse);
        //this.transform.rotation = Quaternion.Euler(0, 0, 10);
    }

    public void Pause(bool _isPause)
    {
        if (_isPause)
        {
            characterRigidbody2D.gravityScale = 0;
            this.GetComponent<Animator>().enabled = false;
        }
        else
        {
            characterRigidbody2D.gravityScale = gravityScale;
            this.GetComponent<Animator>().enabled = true;
        }
    }

    private void Death()
    {
        isDeath = true;
        Jump();
        this.GetComponent<Animator>().enabled = false;
        //characterRigidbody2D.gravityScale = 0;
        //characterRigidbody2D.velocity = Vector2.zero;
        this.GetComponent<CapsuleCollider2D>().isTrigger = false;
        DispatchEvent(EVENT_DEATH, this.gameObject, EventArgs.Empty);
    }
    // Update is called once per frame
    public void UpdateMethod()
    {
        this.transform.eulerAngles = new Vector3(0, 0, characterRigidbody2D.velocity.y * 2.5f);

        /*
        if (!isDeath)
        {
            this.transform.eulerAngles = new Vector3(0, 0, characterRigidbody2D.velocity.y * 2.5f);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Border" && !isDeath && IsReady)
        {
            Debug.Log("death");
            Death();
        }
    }
}
