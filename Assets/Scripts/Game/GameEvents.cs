using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameEvents

{
    public static Action<Transform> OnFindNewPredator;
    public static Action<GameObject, int> OnPreyDied;
    public static Action<int> OnMoneyChange;
    public static Action<int> OnIncreasePlatformsPrice;
    public static Action<float> OnStartHunt;
    public static Action<bool> OnIncreaseCombo;
    public static Action<int> OnUpdateMeatStatus;
    public static Action<bool> OnWindParticles;
    public static Action<bool> OnSetGhostStatus;

    public static void CallOnFindNewPredator(Transform prey)
    {
        OnFindNewPredator?.Invoke(prey);
    }
    public static void CallOnPreyDied(GameObject prey, int reward)
    {
        OnPreyDied?.Invoke(prey, reward);
    }
    public static void CallOnMoneyChange(int moneyValue)
    {
        OnMoneyChange?.Invoke(moneyValue);
    }
    public static void CallOnIncreasePlatformsPrice(int price)
    {
        OnIncreasePlatformsPrice?.Invoke(price);
    }

    public static void CallOnStartHunt(float levelSpeed)
    {
        OnStartHunt?.Invoke(levelSpeed);
    }

    public static void CallIncreaseCombo(bool status)
    {
        OnIncreaseCombo?.Invoke(status);
    }
    public static void CallOnUpdateMeatStatus(int reward)
    {
        OnUpdateMeatStatus?.Invoke(reward);
    }
    public static void CallWindParticles(bool particlesStatus)
    {
        OnWindParticles?.Invoke(particlesStatus);
    }

    public static void CallOnSetGhostStatus(bool ghostStatus)
    {
        OnSetGhostStatus?.Invoke(ghostStatus);
    }




}
