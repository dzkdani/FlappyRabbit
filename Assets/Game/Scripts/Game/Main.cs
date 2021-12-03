using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Main : MonoBehaviour
{
    #region serializeField Objects
    [BoxGroup("Game Views")]
    [SerializeField] MainMenu mainMenu;
    [BoxGroup("Game Views")]
    [SerializeField] HUDManager hudManager;
    [BoxGroup("Game Views")]
    [SerializeField] GameplayManager gameplayManager;
    [BoxGroup("Game Views")]
    [SerializeField] SettingsPopup settingsPopup;

    #endregion serializeField Objects
    #region variables
    public HUDManager HudManager { get => hudManager; set => hudManager = value; }
    public MainMenu MainMenu { get => mainMenu; set => mainMenu = value; }
    public GameplayManager GameplayManager { get => gameplayManager; set => gameplayManager = value; }
    #endregion
    #region singleton
    // Singleton instance.
    public static Main Instance = null;
    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    void Start()
    {
        CharacterDatabase db = CharacterDatabase.Instance;
        DOTween.Init();
        IntializeGameDataManager();
        InitalizeHUDManager();
        InitializeMainMenu();
    }

    #region initalize
    #region gameplay data manager
    private void IntializeGameDataManager()
    {
        GameDataManager.GetInstance().Init();
        //GameDataManager.GetInstance().Reset();
        //GameDataManager.GetInstance().ClearSaveData();
    }
    #endregion
    #region mainmenu
    private void InitializeMainMenu()
    {
        GooglePlayController.Instance.adsManager.ShowBannerAds();
        hudManager.Init((int)HUDManager.GameState.mainmenu);
        mainMenu.EVENT_HIDE += OnHideMainmenu;
        mainMenu.EVENT_OPEN_GAMEPLAY += OnOpenGameplay;
        mainMenu.Init();
    }

    private void OnOpenGameplay(object sender, EventArgs e)
    {
        GooglePlayController.Instance.adsManager.HideBannerAds();
        InitializeGameplayManager();
        mainMenu.Hide();
    }

    private void OnHideMainmenu(object sender, EventArgs e)
    {
        mainMenu.EVENT_HIDE -= OnHideMainmenu;
        mainMenu.EVENT_OPEN_GAMEPLAY -= OnOpenGameplay;
    }
    #endregion
    #region gameplay
    private void InitializeGameplayManager()
    {
        hudManager.Init((int)HUDManager.GameState.gameplay);
        gameplayManager.EVENT_HIDE += OnHideGameplay;
        gameplayManager.Init();
    }

    private void OnHideGameplay(object sender, EventArgs e)
    {
        gameplayManager.EVENT_HIDE -= OnHideGameplay;
        InitializeMainMenu();
    }
    #endregion
    #region hud manager
    private void InitalizeHUDManager()
    {
        hudManager.EVENT_SHOW_SETTINGS += OnShowSettings;
        hudManager.EVENT_PAUSE_GAME += OnPauseGame;
        
    }

    private void OnPauseGame(object sender, EventArgs e)
    {
        gameplayManager.OnPauseGame();
    }

    private void OnShowSettings(object sender, EventArgs e)
    {
        InitializeSettings();
    }
    #endregion
    #region settings
    private void InitializeSettings()
    {
        settingsPopup.Init();
    }
    #endregion
    #endregion

    #region initializeViews

    #endregion
}
