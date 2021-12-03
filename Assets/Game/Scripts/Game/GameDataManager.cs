using System;
using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private static GameDataManager gameplayDataManager = new GameDataManager();
    public static GameDataManager GetInstance()
    {
        return gameplayDataManager;
    }

    //region global data
    string SepayGameSaveIDFlappyBird = "___";
    int totalCoins;
    int highScore;
    int idSelectedCharacter;
    int totalRvRewards;
    int deathCounterToShowAds;
    bool isAlreadyOpenRv;
    string lastRVOpen;
    List<bool> listOfUnlockedCharacter;

    public int TotalCoins { get => totalCoins; set => totalCoins = value; }
    public int HighScore { get => highScore; set => highScore = value; }
    public List<bool> ListOfUnlockedCharacter { get => listOfUnlockedCharacter; set => listOfUnlockedCharacter = value; }
    public int IdSelectedCharacter { get => idSelectedCharacter; set => idSelectedCharacter = value; }
    public string LastRVOpen { get => lastRVOpen; set => lastRVOpen = value; }
    public bool IsAlreadyOpenRv { get => isAlreadyOpenRv; set => isAlreadyOpenRv = value; }
    public int TotalRvRewards { get => totalRvRewards; set => totalRvRewards = value; }
    public int DeathCounterToShowAds { get => deathCounterToShowAds; set => deathCounterToShowAds = value; }

    //List<List<bool>> listOfCompeltedChallengeInCategory;

    //public List<List<bool>> ListOfCompeltedChallengeInCategory { get => listOfCompeltedChallengeInCategory; set => listOfCompeltedChallengeInCategory = value; }

    public void Init()
    {
        Reset();
        bool exists = SaveGame.Exists("SepayGameSaveIDFlappyBird");
        Debug.Log("is save game exist ? : " + exists);
        if (exists)
        {

            LoadGame();
        }
        else
        {
            SaveGameData();
        }
    }

    public void ClearSaveData()
    {
        SaveGame.Clear();
        Reset();
        SaveGameData();
    }


    public void LoadGame()
    {
        Debug.Log("Load Save Game");
        SepayGameSaveIDFlappyBird = SaveGame.Load<string>("SepayGameSaveIDFlappyBird");
        totalCoins = SaveGame.Load<int>("TotalCoins");
        highScore = SaveGame.Load<int>("HighScore");
        idSelectedCharacter = SaveGame.Load<int>("IdSelectedCharacter");
        isAlreadyOpenRv = SaveGame.Load<bool>("IsAlreadyOpenRv");
        lastRVOpen = SaveGame.Load<string>("LastRVOpen");
        listOfUnlockedCharacter = SaveGame.Load<List<bool>>("listOfUnlockedCharacter");
        //listOfCompeltedChallengeInCategory = SaveGame.Load<List<List<bool>>>("listOfCompeltedChallengeInCategory");
        //CheckData();
    }

    /*private void CheckData()
    {
        Debug.Log(ListOfCategoryUnlockStatus.Count);
        foreach (var item in ListOfCategoryUnlockStatus)
        {
            Debug.Log(item);
        }

    }*/

    public void SaveGameData()
    {
        Debug.Log("Save Game");
        SaveGame.Save("SepayGameSaveIDFlappyBird", SepayGameSaveIDFlappyBird);
        SaveGame.Save<int>("TotalCoins", totalCoins);
        SaveGame.Save<int>("HighScore", highScore);
        SaveGame.Save<int>("IdSelectedCharacter", idSelectedCharacter);
        SaveGame.Save<bool>("IsAlreadyOpenRv", isAlreadyOpenRv);
        SaveGame.Save<string>("LastRVOpen", lastRVOpen);
        SaveGame.Save<List<bool>>("listOfUnlockedCharacter", listOfUnlockedCharacter);
    }

    public void Reset()
    {
        //reset data here
        SepayGameSaveIDFlappyBird = "SepayGameSaveIDFlappyBird";
        totalCoins = 100;
        highScore = 0;
        idSelectedCharacter = 0;
        TotalRvRewards = 100;
        deathCounterToShowAds = 0;
        IsAlreadyOpenRv = false;
        lastRVOpen = DateTime.Now.Ticks.ToString();
        ResetUnlockedCharacters();
    }

    private void ResetUnlockedCharacters()
    {
        ListOfUnlockedCharacter = new List<bool>();
        for (int i = 0; i < CharacterDatabase.Instance.GetTotalCharacter(); i++)
        {
            ListOfUnlockedCharacter.Add(false);
        }
        ListOfUnlockedCharacter[0] = true;
    }

    public bool IsCharacterUnlocked(int _idCharacter)
    {
        return ListOfUnlockedCharacter[_idCharacter];
    }

    public void UnlockCharacter(int _idCharacter)
    {
        ListOfUnlockedCharacter[_idCharacter] = true;
    }
}