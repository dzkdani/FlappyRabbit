using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GooglePlayController : MonoBehaviour
{
    private static GooglePlayController instance;
    public static GooglePlayController Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }else
        {
            DontDestroyOnLoad(this);
        }
        if (adsManager == null)
        {
            adsManager = gameObject.GetComponent<AdsManager>();
        }
        adsManager.AdsInit();
    }

    string leaderboardID = "CgkIgcflxP4JEAIQAg";
    //string achievementID = "";

    public AdsManager adsManager;

    void Start() {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false)
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        RequestLogin();
    }

    void RequestLogin(){
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Logged in successfully");
            }
            else
            {
                Debug.Log("Login Failed");
            }
        });
    }

    public void AddScoreToLeaderBoard(int _score)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Social.ReportScore(_score, leaderboardID, success => {
                if (success)
                {
                    Debug.Log("Succes to add score");
                }
                else
                {
                    Debug.Log("failed to add score");
                }
            });
        }
    }

    public void ShowLeaderboard()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.Log("Calling Leaderboard UI");
            Social.ShowLeaderboardUI();
        }
        else
        {
            Debug.Log("Cant Show Leaderboard, User not Found");
        }
    }

    //public void ShowAchievements()
    //{
    //    if (Social.Active.localUser.authenticated)
    //    {
    //        platform.ShowAchievementsUI();
    //    }
    //}

    //public void UnlockAchievement()
    //{
    //    if (Social.Active.localUser.authenticated)
    //    {
    //        Social.ReportProgress(achievementID, 100f, success => { });
    //    }
    //}
}
