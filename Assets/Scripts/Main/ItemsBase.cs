using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ItemsBase : MonoBehaviour
{
    [SerializeField] private ItemGameObjectInspector[] ObjectGamesItems;
    // [SerializeField] private ItemGameObjectInspector OG_TringleMask;
    // [SerializeField] private ItemGameObjectInspector OG_Desiderium;
    // [SerializeField] private ItemGameObjectInspector OG_Paralaxx;
    // [SerializeField] private ItemGameObjectInspector OG_Lava;
    //======================================================================================//
    [SerializeField] private ItemGameObjectInspector[] ThemesItems;
    // [SerializeField] private ItemGameObjectInspector T_Paralaxx;
    // [SerializeField] private ItemGameObjectInspector T_BlueBattle;
    // [SerializeField] private ItemGameObjectInspector T_Contrast;

    public static ItemsBase Base { private set; get; }

    public Items<GameObject> ObjectGames { private set; get; }

    public Items<GameObject> Themes { private set; get; }

    //======================================================================================//
    // public Items<GameObject>.Item ObjectGame_TringleMask { get { return OG_TringleMask; } }
    // public Items<GameObject>.Item ObjectGame_Desiderium { get { return OG_Desiderium; } }
    // public Items<GameObject>.Item ObjectGame_Paralaxx { get { return OG_Paralaxx; } }

    // public Items<GameObject>.Item ObjectGame_Lava { get { return OG_Lava; } }

    // public Items<GameObject>.Item Theme_Paralaxx { get { return T_Paralaxx; } }
    // public Items<GameObject>.Item Theme_BlueBattle { get { return T_BlueBattle; } }

    // public Items<GameObject>.Item Theme_Contrast { get { return T_Contrast; } }
    //======================================================================================//

    [ContextMenu("Save")]
    public void Save()
    {
        SaveItems();
        SaveSelected();
    }

    public void Load()
    {
        PlayerPrefs.SetInt("I_" + ObjectGamesItems[0].ID + "h", 1);
        PlayerPrefs.SetInt("I_" + ThemesItems[0].ID + "h", 1);
        ItemsSaver.Load(ObjectGames);
        ItemsSaver.Load(Themes);
        // PlayerPrefs.SetInt("I_" + OG_TringleMask.ID + "h", 1);
        var ogs = PlayerPrefs.GetInt("OGs", ObjectGamesItems[0].ID);
        var ts = PlayerPrefs.GetInt("Ts", ThemesItems[0].ID);
        ObjectGames.SetSelected(ogs, true);
        Themes.SetSelected(ts, true);
    }

    public void SaveSelected()
    {
        PlayerPrefs.SetInt("OGs", ObjectGames.GetFirstSelectedToID());
        PlayerPrefs.SetInt("Ts", Themes.GetFirstSelectedToID());
    }

    public void SaveItems()
    {
        ItemsSaver.Save(ObjectGames);
        ItemsSaver.Save(Themes);
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Base != null) { Destroy(this); return; }
        Base = this;

        ObjectGames = new Items<GameObject>();
        Themes = new Items<GameObject>();

        for (int i = 0; i < ObjectGamesItems.Length; ++i)
            ObjectGames.Add(ObjectGamesItems[i]);

        for (int i = 0; i < ThemesItems.Length; ++i)
            Themes.Add(ThemesItems[i]);

        ObjectGames.CheckIdCollision();
        Themes.CheckIdCollision();

        Load();
    }

    public Items<GameObject>.Item GetNotHaveAndBuy(int coins)
    {
        Items<GameObject>.Item result = null;
        result = Themes.GetNotHaveAndBuy(coins);
        var buffer = ObjectGames.GetNotHaveAndBuy(coins);
        if (result == null || (buffer != null && buffer.Price > result.Price))
            return buffer;
        return result;
    }

    void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        ObjectGameChanger.Manager.SetObjectGamePrefab(ObjectGames.GetFirstSelectedToPrefab());
        WorldEther.ChangePrefabObjectGame.Subscribe(delegate (Ethers.Channel.Info info)
        {
            // Debug.Log("Save selected");
            SaveSelected();
        });
        WorldEther.BuyEvent.Subscribe(delegate (Ethers.Channel.Info info)
        {
            // Debug.Log("Save items");
            SaveItems();
        });
    }

    [ContextMenu("Sort on price")]
    private void Editor_SortOnPrice()
    {
        var list = ObjectGamesItems.ToList();
        list.Sort((a, b) => a.Price.CompareTo(b.Price));
        ObjectGamesItems = list.ToArray();

        var themes = ThemesItems.ToList();
        themes.Sort((a, b) => a.Price.CompareTo(b.Price));
        ThemesItems = themes.ToArray();
    }
}

public static class ItemsSaver
{
    public static void Save<T>(Items<T> items) where T : Object
    {
        foreach (Items<T>.Item item in items.ToArray())
            SaveItem(item);
    }
    public static void Load<T>(Items<T> items) where T : Object
    {
        foreach (Items<T>.Item item in items.ToArray())
            LoadItem(item);
    }
    private static void SaveItem<T>(Items<T>.Item item) where T : Object
    {
        if (item == null) return;

        PlayerPrefs.SetInt("I_" + item.ID + "h", BoolToInt(item.PlayerHave));
    }
    private static void LoadItem<T>(Items<T>.Item item) where T : Object
    {
        if (item == null) return;
        item.PlayerHave = IntToBool(PlayerPrefs.GetInt("I_" + item.ID + "h", 0));
    }
    private static int BoolToInt(bool value)
    {
        return value ? 1 : 0;
    }
    private static bool IntToBool(int value)
    {
        return value > 0;
    }
}
[System.Serializable]
public class Items<T> where T : Object
{
    public int GetFirstSelectedToID()
    {
        foreach (Item item in ListItems)
            if (item.IsSelected)
                return item.ID;
        return 0;
    }
    public T GetFirstSelectedToPrefab()
    {
        foreach (Item item in ListItems)
            if (item.IsSelected)
                return item.ObjectPrefab;
        return null;
    }
    public bool ItemIdToHave(int id)
    {
        foreach (Item item in ListItems)
            if (item.ID == id)
                return item.PlayerHave;
        return false;
    }
    public void SetHave(int id, bool value)
    {
        foreach (Item item in ListItems)
            if (item.ID == id)
            {
                item.PlayerHave = value;
                break;
            }
    }

    public Item GetNotHaveAndBuy(int coins)
    {
        Item result = null;
        foreach (Item item in ListItems)
            if (!item.PlayerHave && item.Price <= coins && (result == null || item.Price > result.Price))
                result = item;
        return result;
    }

    public Item ItemIdToItem(int id)
    {
        foreach (Item item in ListItems)
            if (item.ID == id)
                return item;
        return null;
    }
    public bool ItemIdToIsSelected(int id)
    {
        foreach (Item item in ListItems)
            if (item.ID == id)
                return item.IsSelected;
        return false;
    }
    public T ItemIdToPrefab(int id)
    {
        foreach (Item item in ListItems)
            if (item.ID == id)
                return item.ObjectPrefab;
        return null;
    }
    public void SetSelected(int id, bool value)
    {
        foreach (Item item in ListItems)
            if (item.ID == id)
            {
                item.IsSelected = value;
                foreach (Item e in ListItems)
                    if (item.ID != e.ID)
                    {
                        e.IsSelected = false;
                    }
                break;
            }
    }
    public void CheckIdCollision()
    {
        var leng = ListItems.Count;
        for (int i = 0; i < leng; ++i)
            for (int j = i + 1; j < leng; ++j)
            {
                if (ListItems[i].ID == ListItems[j].ID)
                {
                    Debug.LogWarningFormat("ID Collision!(id: {0}) \n{1} and {2}", ListItems[i].ID, ListItems[i].Name, ListItems[j].Name);
                }
            }
    }
    public void Add(Item item)
    {
        ListItems.Add(item);
    }

    public bool PlayerHave(int id)
    {
        foreach (Item item in ListItems)
            if (item.ID == id)
                if (item.PlayerHave)
                    return true;
                else return false;
        return false;

    }

    List<Item> ListItems = new List<Item>();
    [System.Serializable]
    public class Item
    {
        public virtual T ObjectPrefab { protected set; get; }
        public virtual int ID { protected set; get; }
        public virtual string Name { protected set; get; }
        public virtual int Price { protected set; get; }
        public bool PlayerHave;
        public bool IsSelected;
    }

    public Item[] ToArray()
    {
        return ListItems.ToArray();
    }


}
[System.Serializable]
public class ItemInspector<T> : Items<T>.Item where T : Object
{
    [SerializeField] private T Prefab;
    public override T ObjectPrefab { protected set { Prefab = value; } get { return Prefab; } }
    [SerializeField] private string _Name;
    public override string Name { protected set { _Name = value; } get { return _Name; } }
    [SerializeField] private int _ID;
    public override int ID { protected set { _ID = value; } get { return _ID; } }
    [SerializeField] private int _Price;
    public override int Price { protected set { _Price = value; } get { return _Price; } }


}

[System.Serializable]
public class ItemGameObjectInspector : ItemInspector<GameObject>
{
}