using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyAdsButtonAction : MonoBehaviour
{
    [SerializeField] int UpCoins = 15;
    [SerializeField] UnityEngine.Events.UnityEvent OnCloseAd;
    public void Action()
    {
        if (DailyAds.Manager.IsReady())
        {
            AdsManager.Manager.ShowNonSkipable(delegate
            {
                DeferredAction.Manager.AddDeferredAction(delegate
                {
                    Coins.Manager.CoinsCount += UpCoins;
                    DailyAds.Manager.Flash();
                    OnCloseAd.Invoke();
                }, 0.67f);
            });
        }
        else
        {
            // MessageManager.ShowMessage("Осталось подождать: " + Mathf.RoundToInt((LaunchTracker.SECONDS_IN_DAY - DailyAds.Manager.GetFullLeftTime()) / (float)60) + " минут", 2);
            Debug.Log("On DailyAdsButton but ads not ready");
        }
    }
}
