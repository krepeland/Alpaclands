using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Steamworks;

public class AchievmentsManager : MonoBehaviour
{
    public static AchievmentsManager singleton;
    public static HashSet<string> gettedAchievments;

    public static bool IsSteamInitialized;

    private void Start()
    {
        singleton = this;
        gettedAchievments = new HashSet<string>();
        //if (SteamManager.Initialized)
        //{
        //    IsSteamInitialized = true;
        //
        //    Steamworks.SteamUserStats.RequestCurrentStats();
        //}
    }

    public static void AddAchievment(string name) {
        if (!IsSteamInitialized || gettedAchievments.Contains(name))
            return;
        //Steamworks.SteamUserStats.SetAchievement(name);
        gettedAchievments.Add(name);
        StoreStats();
    }

    private static void StoreStats() {
        //Steamworks.SteamUserStats.StoreStats();
    }

    public static void Achievment_CheckPlantCount()
    {
        var count = KeyManager.GetKey("PlantCount", 0);
        if (count >= 10)
        {
            AddAchievment("FIRST_PLANTS");
        }
        if (count >= 100)
        {
            AddAchievment("MORE_PLANTS");
        }
        if (count >= 1000)
        {
            AddAchievment("EVEN_MORE_PLANTS");
        }
    }

    public static void Achievment_CheckStore()
    {
        if (KeyManager.GetKey("Slot_Plant-6", 0) != 0)
        {
            AddAchievment("PLANT_EXPERT");
        }

        if (KeyManager.GetKey("Slot_Alpacas-Max-4", 0) != 0 &&
            KeyManager.GetKey("Slot_Alpacas-Rarity-4", 0) != 0 &&
            KeyManager.GetKey("Slot_Alpacas-Luck-4", 0) != 0)
        {
            AddAchievment("ALPACAS_EXPERT");
        }
    }

    public static void Achievment_CheckScore()
    {
        var score = KeyManager.GetKey("Score", 0);
        if (score >= 100)
        {
            AddAchievment("BEGINNER");
        }

        if (score >= 250)
        {
            AddAchievment("LEARNER");
        }

        if (score >= 500)
        {
            AddAchievment("ADVANCED");
        }

        if (score >= 1500)
        {
            AddAchievment("PROFESSIONAL");
        }

        if (score >= 2500)
        {
            AddAchievment("EXPERT");
        }
    }

    public static void Achievment_CheckScorePerPlant(int score)
    {
        if (score >= 25)
        {
            AddAchievment("NICE_PLANT");
        }

        if (score >= 45)
        {
            AddAchievment("AMAZING_PLANT");
        }

        if (score >= 65)
        {
            AddAchievment("BEST_PLANT");
        }
    }

    public static void Achievment_CheckCoins()
    {
        var coins = KeyManager.GetKey("Coins", 0);
        if (coins >= 10)
        {
            AddAchievment("FIRST_SAVINGS");
        }

        if (coins >= 50)
        {
            AddAchievment("STACK_OF_COINS");
        }

        if (coins >= 150)
        {
            AddAchievment("MOUNTAINS_OF_GOLD");
        }
    }
}
