using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyAdsViewer : MonoBehaviour
{
    [SerializeField] Appearance ButtonAd;
    [SerializeField] Appearance TimeBomb;

    [SerializeField] TimeBombDaily TimeBombScript;

    private bool IsButton { set { ButtonAd.IsAppearance = value; TimeBomb.IsAppearance = !value; if (!value) TimeBombScript.UpdateTime(); } get { return ButtonAd.IsAppearance; } }

    public void UpdateViewer()
    {
        IsButton = DailyAds.Manager.IsReady();
    }
}
