using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public const string KEY_COINS = "COINSC";//was : COINS
    #region STATIC
    public static Coins Manager { private set; get; }
    #endregion

    private int _CoinsCount;
    public int CoinsCount
    {
        set
        {
            _CoinsCount = Mathf.Clamp(value, 0, int.MaxValue);
            SaveCountCoins();
            WorldEther.CoinsChange.Push(this, null);
        }
        get
        {
            return _CoinsCount;
        }
    }

    public void UpOne()
    {
        CoinsCount++;
    }

    #region  INITIALIZE
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Manager != null)
        {
            Destroy(this);
            return;
        }
        Manager = this;
        LoadCountCoins();
    }
    void Awake()
    {
        Initialize();
    }

    public void SaveCountCoins()
    {
        PlayerPrefs.SetInt(KEY_COINS, _CoinsCount);
    }
    public void LoadCountCoins()
    {
        _CoinsCount = PlayerPrefs.GetInt(KEY_COINS, _CoinsCount);
    }

    [ContextMenu("Flash Coins")]
    private void FlashCoins()
    {
        CoinsCount = 0;
    }
    #endregion
}
