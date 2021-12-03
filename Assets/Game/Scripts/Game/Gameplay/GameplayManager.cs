using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameplayManager : CustomMonobehavior
{
    [BoxGroup("Gameplay Manager Object")]
    [SerializeField] GameObject gameplayManagerWorld;

    [BoxGroup("Character attribute")]
    [SerializeField] Character character;
    [BoxGroup("Character attribute")]
    [SerializeField] private float jumpForceValue;

    [BoxGroup("Obstacle attribute")]
    [SerializeField] GameObject obstacle;
    [BoxGroup("Obstacle attribute")]
    [SerializeField] GameObject obstacleContainer;
    [BoxGroup("Obstacle attribute")]
    [SerializeField] private float obstacleMoveSpeed;
    [BoxGroup("Obstacle attribute")]
    [SerializeField] private float obstacleSpawnInverval;
    [BoxGroup("Obstacle attribute")]
    [SerializeField] private float obstacleTopPosition;
    [BoxGroup("Obstacle attribute")]
    [SerializeField] private float obstacleBottomPosition;
    [BoxGroup("Obstacle attribute")]
    [SerializeField] private float obstacleMaximumGap;
    [BoxGroup("Obstacle attribute")]
    [SerializeField] private float obstacleMinimumGap;
    [BoxGroup("Obstacle attribute")]
    [SerializeField] private float obstacleGapDecrement;
    [BoxGroup("Obstacle attribute"), Tooltip("every score value given, the obstacle will be decreased")]
    [SerializeField] private float ScoreValueForDifficultyIncrement;

    [BoxGroup("Environment parallax attribute")]
    [SerializeField] EnvironmentParallax environmentParallax;

    [BoxGroup("Gameplay UI components")]
    [SerializeField] Button btnOverlay;
    [BoxGroup("Gameplay UI components")]
    [SerializeField] TextMeshProUGUI txtTapToStart;

    [BoxGroup("Gameplay UI views")]
    [SerializeField] GameOverPopup gameOverPopup;
    [BoxGroup("Gameplay UI views")]
    [SerializeField] PausePopup pausePopup;

    #region variables
    private List<Obstacle> listOfObstacles = new List<Obstacle>();
    private float gapValue;
    private int score;
    private bool isGameOver;
    private bool isGameStart;
    private bool isFirstTimeDie;
    private bool isPause;
    #endregion

    #region events
    public event EventHandler EVENT_HIDE;
    #endregion

    public void Init()
    {
        this.gameObject.SetActive(true);
        gameplayManagerWorld.SetActive(true);
        InitalizeData();
        InitializeCharacter();
        InitializeParallax();
    }

    private void InitializeParallax()
    {
        environmentParallax.Init(obstacleMoveSpeed);
    }

    private void StartGame()
    {
        isGameStart = true;
        txtTapToStart.gameObject.SetActive(!isGameStart);
        character.Jump();
        character.SetCharacterReady();
        InvokeRepeating("AttachObstacle", 0, obstacleSpawnInverval);
    }

    private void InitalizeData()
    {
        gapValue = obstacleMaximumGap;
        score = 0;
        isGameOver = false;
        isGameStart = false;
        isFirstTimeDie = false;
        isPause = false;
        txtTapToStart.gameObject.SetActive(true);
        UpdateScore(score);
    }

    private void Restart()
    {
        RemoveAllObstacles();
        Init();
    }
    #region buttons
    private void InitalizeAllButtons()
    {
        btnOverlay.onClick.AddListener(() => OnClickButtonOverlay());
    }

    public void OnPauseGame()
    {
        if (isGameStart)
        {
            isPause = true;
            InitializePausePopup();
        }
    }

    private void OnClickButtonOverlay()
    {
        if (isGameStart)
        {
            character.Jump();
        }
        else
        {
            StartGame();
        }
    }
    #endregion

    #region initalize objects
    private void AttachObstacle()
    {
        Debug.Log("attach obstacle");
        GameObject newItem = Instantiate(obstacle, obstacleContainer.transform);
        newItem.GetComponent<Obstacle>().EVENT_DESTROY_OBSTACLE += OnDestroyObstacle;
        listOfObstacles.Add(newItem.GetComponent<Obstacle>());
        GetObstacle(listOfObstacles.Count - 1).Init(gapValue, obstacleMoveSpeed, obstacleTopPosition, obstacleBottomPosition);
    }

    private void OnDestroyObstacle(object _sender, EventArgs e)
    {
        GameObject sender = ((GameObject)_sender);
        sender.GetComponent<Obstacle>().EVENT_DESTROY_OBSTACLE -= OnDestroyObstacle;
        listOfObstacles.Remove(sender.GetComponent<Obstacle>());
        Destroy(sender);
    }

    private Obstacle GetObstacle(int _index)
    {
        return (listOfObstacles[_index]) as Obstacle;
    }

    private void RemoveAllObstacles()
    {
        int totalCurrentObstacle = listOfObstacles.Count;
        for (int i = 0; i < totalCurrentObstacle; i++)
        {
            listOfObstacles[i].EVENT_DESTROY_OBSTACLE -= OnDestroyObstacle;
            Destroy(listOfObstacles[i].gameObject);
        }
        listOfObstacles = new List<Obstacle>();
    }

    private void InitializeCharacter()
    {
        character.transform.position = new Vector3(0, 7, -10);
        character.GetComponent<Character>().EVENT_DEATH += OnCharacterDeath;
        character.GetComponent<Rigidbody2D>().gravityScale = 2.0f;
        character.Init(GameDataManager.GetInstance().IdSelectedCharacter, jumpForceValue);
        character.transform.DOMoveY(0, 1.5f, false)
            .SetEase(Ease.InOutBack)
            .OnComplete(CharacterReady);
    }

    private void CharacterReady()
    {
        character.IsReady = true;
        InitalizeAllButtons();
    }

    private void OnCharacterDeath(object _sender, EventArgs e)
    {
        isGameOver = true;
        GameObject sender = ((GameObject)_sender);
        sender.GetComponent<Character>().EVENT_DEATH -= OnCharacterDeath;
        btnOverlay.onClick.RemoveAllListeners();
        CancelInvoke("AttachObstacle");
        Invoke("InitializeGameOver", 1.25f);
    }

    private void CheckToShowInterstitialAds()
    {
        GameDataManager.GetInstance().DeathCounterToShowAds++;
        if(GameDataManager.GetInstance().DeathCounterToShowAds >= 5)
        {
            GooglePlayController.Instance.adsManager.ShowInterstitialAds();
            GameDataManager.GetInstance().DeathCounterToShowAds = 0;
        }
    }

    #region pause popup
    private void InitializePausePopup()
    {
        character.Pause(isPause);
        pausePopup.EVENT_HIDE += OnHidePausePopup;
        pausePopup.EVENT_QUIT_GAME += OnQuitGame;
        pausePopup.EVENT_RESTART_GAME += OnRestartPopup;
        pausePopup.Init();
    }

    private void OnHidePausePopup(object sender, EventArgs e)
    {
        ResumeGame();
        pausePopup.EVENT_HIDE -= OnHidePausePopup;
        pausePopup.EVENT_QUIT_GAME -= OnQuitGame;
        pausePopup.EVENT_RESTART_GAME -= OnRestartPopup;
    }

    private void ResumeGame()
    {
        isPause = false;
        character.Pause(isPause);
    }
    #endregion

    #region game over popup
    private void InitializeGameOver()
    {
        CheckToShowInterstitialAds();
        gameOverPopup.EVENT_HIDE += OnHideGameOverPopup;
        gameOverPopup.EVENT_QUIT_GAME += OnQuitGame;
        gameOverPopup.EVENT_RESTART_GAME += OnRestartPopup;
        int currentHighScore = GameDataManager.GetInstance().HighScore;
        int coinRewads = score / 2;
        GameDataManager.GetInstance().TotalCoins += coinRewads;
        if (currentHighScore < score)
        {
            GameDataManager.GetInstance().HighScore = score;
            gameOverPopup.Initialize(score, coinRewads, true);
        }
        else
        {
            gameOverPopup.Initialize(score, coinRewads, false);
        }
        
    }

    private void OnQuitGame(object _sender, EventArgs e)
    {
        GameObject sender = ((GameObject)_sender);
        sender.GetComponent<GameOverPopup>().Hide();
        Hide();
    }

    private void OnRestartPopup(object _sender, EventArgs e)
    {
        Debug.Log("restart");
        GameObject sender = ((GameObject)_sender);
        sender.GetComponent<GameOverPopup>().Hide();
        Restart();
    }

    private void OnHideGameOverPopup(object sender, EventArgs e)
    {
        gameOverPopup.EVENT_HIDE -= OnHideGameOverPopup;
        gameOverPopup.EVENT_QUIT_GAME -= OnQuitGame;
        gameOverPopup.EVENT_RESTART_GAME -= OnRestartPopup;
    }
    #endregion
    #endregion

    #region private method
    private void UpdateScore(int _score)
    {
        score = _score;
        Main.Instance.HudManager.UpdateScore(score);
        UpdateDifficulty();
    }

    private void UpdateDifficulty()
    {
        if (score % ScoreValueForDifficultyIncrement == 0)
        {
            if (gapValue >= obstacleMinimumGap)
            {
                gapValue -= obstacleGapDecrement;
            }
            else
            {
                gapValue = obstacleMinimumGap;
            }
        }
    }
    #endregion

    #region update
    void Update()
    {
        if (!isGameOver)
        {
            if (!isPause)
            {
                UpdateObstacleMovement();
                UpdateEnvironmentParallaxMovement();
                UpdateCharacters();
            }
        }
    }

    private void UpdateCharacters()
    {
        character.UpdateMethod();
    }

    private void UpdateEnvironmentParallaxMovement()
    {
        environmentParallax.UpdateMethod();
    }

    private void UpdateObstacleMovement()
    {
        for (int i = 0; i < listOfObstacles.Count; i++)
        {
            listOfObstacles[i].UpdateMethod();
            if (listOfObstacles[i].transform.position.x < character.transform.position.x && !listOfObstacles[i].IsPassed)
            {
                listOfObstacles[i].IsPassed = true;
                score++;
                UpdateScore(score);
            }
        }
    }
    #endregion

    public void Hide()
    {
        RemoveAllObstacles();
        btnOverlay.onClick.RemoveAllListeners();
        DispatchEvent(EVENT_HIDE, this.gameObject, EventArgs.Empty);
        gameplayManagerWorld.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
