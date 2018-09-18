using UnityEngine;
using Ethers;

public class WorldEther : MonoBehaviour
{
    #region  STATIC
    //===============================================//
    public static Channel ObjectGameLose
    {
        get
        {
            return instance.ObjectLose;
        }
    }

    public static Channel EventSwipe
    {
        get
        {
            return instance.SwipeEvent;
        }
    }

    public static Channel CoinsChange
    {
        get
        {
            return instance.ChangeCoins;
        }
    }
    public static Channel ChangeLevel
    {
        get
        {
            return instance.LevelChange;
        }
    }
    public static Channel ChangePalette
    {
        get
        {
            return instance.PaletteChange;
        }
    }

    public static Channel RestartGame
    {
        get
        {
            return instance.Restart;
        }
    }

    public static Channel PauseGame
    {
        get { return instance.Pause; }
    }

    public static Channel ResumeGame
    {
        get { return instance.Resume; }
    }

    public static Channel ChangeScores
    {
        get { return instance.Scores; }
    }

    public static Channel ChangeLifes
    {
        get { return instance.Lifes; }
    }
    public static Channel LoseGame
    {
        get { return instance.Lose; }
    }
    public static Channel ChangeScreen
    {
        get { return instance.ScreenChange; }
    }
    public static Channel ChangePrefabObjectGame
    {
        get
        {
            return instance.ChangeObjectGamePrefab;
        }
    }

    public static Channel ChangeInventoryObjectGame
    {
        get
        {
            return instance.ChangeObjectGameInventory;
        }
    }

    public static Channel BuyEvent
    {
        get { return instance.EventBuy; }

    }
    // public static Channel ChangePrefabActions
    // {
    //     get
    //     {
    //         return instance.ChangeActionsPrefab;
    //     }
    // }
    public static Channel ChangePrefabOthers
    {
        get
        {
            return instance.ChangeOthersPrefab;
        }
    }
    public static Channel ApplyAllPrefabs
    {
        get
        {
            return instance.ApplyPrefabs;
        }
    }
    public static Ether EtherWorld
    {
        get { return instance.Ether; }
    }
    public static Channel ChangeGameSettings
    {
        get { return instance.ChangeSettings; }
    }
    public static Channel ChnageVolume
    {
        get { return instance.ChangeBaseVolume; }
    }
    public static Channel ProgressScoreChagne
    {
        get { return instance.ChangeProgrssScore; }
    }
    private static WorldEther instance;
    //===============================================//
    #endregion

    private Ether Ether;
    private bool _initialized;

    private Channel ObjectLose;
    private Channel Restart;
    private Channel Pause;
    private Channel Resume;
    private Channel Lifes;
    private Channel Scores;
    private Channel Lose;

    private Channel LevelChange;
    private Channel PaletteChange;
    private Channel ScreenChange;
    private Channel ChangeCoins;
    private Channel ChangeObjectGameInventory;
    private Channel SwipeEvent;
    private Channel ChangeProgrssScore;
    //======================================//
    private Channel ChangeObjectGamePrefab;
    private Channel ChangeOthersPrefab;
    private Channel ApplyPrefabs;
    //======================================//
    private Channel EventBuy;
    private Channel ChangeSettings;

    private Channel ChangeBaseVolume;


    [ContextMenu("LOG")]
    public void Log()
    {
        foreach (Channel channel in Ether.Channels)
            Debug.LogFormat("{0} have {1} Subscribers", channel.Name, channel.CountSubscribers);
    }

    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        Ether = new Ether();
        //

        ObjectLose = new Channel("ObjectGameLose");
        Restart = new Channel("Restart");
        Resume = new Channel("Resume");
        Pause = new Channel("Pause");
        Lifes = new Channel("ChangeLifes");
        Scores = new Channel("ChangeScores");
        Lose = new Channel("Lose");
        PaletteChange = new Channel("PaletteChange");
        ScreenChange = new Channel("ScreenChange");
        ChangeCoins = new Channel("ChangeCoins");
        ChangeObjectGamePrefab = new Channel("ChangeObjectGamePrefab");
        // ChangeActionsPrefab = new Channel("ChangeActionsPrefab");
        ChangeOthersPrefab = new Channel("ChangeOthersPrefab");
        ApplyPrefabs = new Channel("ApplyPrefabs");
        ChangeObjectGameInventory = new Channel("ChangeObjectGameInventory");
        EventBuy = new Channel("EventBuy");
        ChangeSettings = new Channel("ChangeSettings");
        SwipeEvent = new Channel("SwipeEvent");
        ChangeBaseVolume = new Channel("ChangeBaseVolume");
        ChangeProgrssScore = new Channel("ChangeProgrssScore");
        LevelChange = new Channel("LevelChange");

        Ether.Channels.Add(ObjectLose);
        Ether.Channels.Add(Restart);
        Ether.Channels.Add(Pause);
        Ether.Channels.Add(Resume);
        Ether.Channels.Add(Lifes);
        Ether.Channels.Add(Scores);
        Ether.Channels.Add(Lose);
        Ether.Channels.Add(PaletteChange);
        Ether.Channels.Add(ScreenChange);
        Ether.Channels.Add(ChangeCoins);
        Ether.Channels.Add(ChangeObjectGamePrefab);
        // Ether.Channels.Add(ChangeActionsPrefab);
        Ether.Channels.Add(ChangeOthersPrefab);
        Ether.Channels.Add(ApplyPrefabs);
        Ether.Channels.Add(ChangeObjectGameInventory);
        Ether.Channels.Add(EventBuy);
        Ether.Channels.Add(ChangeSettings);
        Ether.Channels.Add(SwipeEvent);
        Ether.Channels.Add(ChangeBaseVolume);
        Ether.Channels.Add(ChangeProgrssScore);
        Ether.Channels.Add(LevelChange);
        instance = this;
        // ChangeCoins.Subscribe(delegate (Ethers.Channel.Info info) { Debug.Log(Coins.Manager.CoinsCount.ToString()); });
    }

    void Awake()
    {
        Initialize();
    }
}