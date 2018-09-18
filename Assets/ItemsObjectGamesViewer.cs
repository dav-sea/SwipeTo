using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsObjectGamesViewer : MonoBehaviour
{
    [SerializeField]
    UIOrganization.Screen TargetScreen;

    [SerializeField]
    Transform Contenier;

    [SerializeField]
    TargetFollowScript TargetFollowScript;

    [SerializeField]
    Appearance IsSelectedElement;
    [SerializeField]
    Appearance ButtonBuy;
    [SerializeField]
    Appearance ButtonSelect;
    [SerializeField]
    Appearance ContenierPrice;
    [SerializeField]
    Appearance LeftArrow;
    [SerializeField]
    Appearance RightArrow;
    [SerializeField]
    TextSetter TextPrice;

    // [SerializeField]
    Leaf[] Objects;

    [SerializeField] private bool BuyToSelected = true;

    int Current = 0;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (TargetScreen == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TargetScreen", name);
            enabled = false;
            return;
        }
        if (Contenier == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Contenier", name);
            enabled = false;
            return;
        }
        if (TargetFollowScript == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TargetFollowScript", name);
            enabled = false;
            return;
        }
        if (IsSelectedElement == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "IsSelectedElement", name);
            enabled = false;
            return;
        }
        if (ButtonBuy == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ButtonBuy", name);
            enabled = false;
            return;
        }
        if (ButtonSelect == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ButtonSelect", name);
            enabled = false;
            return;
        }
        if (TextPrice == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TextPrice", name);
            enabled = false;
            return;
        }
        if (ContenierPrice == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ContenierPrice", name);
            enabled = false;
            return;
        }
        if (LeftArrow == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "LeftArrow", name);
            enabled = false;
            return;
        }
        if (RightArrow == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "RightArrow", name);
            enabled = false;
            return;
        }
        TargetFollowScript.Initialize();
        TargetFollowScript.Accelerate.AccelerateValue = 4;
        // TargetFollowScript.SetTarget(Contenier.position);
    }
    MessageManager.IHideMessage MessageCoin;
    public void FlashCoinMessage()
    {
        if (MessageCoin == null) return;
        MessageCoin.Hide();
        MessageCoin = null;
    }

    public void SwitchLeft()
    {
        if (Current == 0) return;
        TargetFollowScript.SetTarget(TargetFollowScript.GetTargetPosition() + new Vector3(OffsetX, 0, 0));
        Current--;
        UpdateEnvironment();
    }
    public void SwitchRight()
    {
        // Debug.Log("" + Current);
        if (Current + 1 == Objects.Length) return;
        TargetFollowScript.SetTarget(TargetFollowScript.GetTargetPosition() - new Vector3(OffsetX, 0, 0));
        Current++;
        UpdateEnvironment();
    }

    private float OffsetX = 50;

    [ContextMenu("Initialize Viewer")]
    public void InitializeViewer()
    {
        // var objects = new List<GameObject>();
        // var items = Inventory.Manager.GetItems();
        var items = ItemsBase.Base.ObjectGames.ToArray();
        List<Leaf> leafs = new List<Leaf>(items.Length);

        GameObject obj;
        ObjectGame controller;

        for (int i = 0; i < items.Length; ++i)
        {
            obj = Instantiate(items[i].ObjectPrefab, new Vector3(i * OffsetX, 0, 0) + Contenier.position, Quaternion.identity, Contenier);
            leafs.Add(new Leaf(obj, items[i].Price, items[i].ID));

            if (items[i].IsSelected) Current = i;
            // TargetFollowScript.SetTarget(TargetFollowScript.GetTargetPosition() - new Vector3(OffsetX * Current, 0, 0));

            controller = obj.GetComponent<ObjectGame>();
            controller.GetActionManager().UseBalanceGamePlayData = true;
            controller.ActivateDemo();
            controller.SetupFrontSide();
        }

        Objects = leafs.ToArray();
    }

    public void UpdateEnvironment()
    {

        IsSelectedElement.IsAppearance = ItemsBase.Base.ObjectGames.ItemIdToIsSelected(Objects[Current].ID);

        ButtonSelect.IsAppearance = ItemsBase.Base.ObjectGames.ItemIdToHave(Objects[Current].ID) && !IsSelectedElement.IsAppearance;
        ButtonBuy.IsAppearance = Objects[Current].Price <= Coins.Manager.CoinsCount && !(ButtonSelect.IsAppearance || IsSelectedElement.IsAppearance);

        if (ButtonBuy.IsAppearance || ButtonSelect.IsAppearance || IsSelectedElement.IsAppearance)
        {
            FlashCoinMessage();
        }
        else if (MessageCoin == null) MessageCoin = MessageManager.ShowMessage(TranslationManager.GetText("Message_NotEnoughCoins"));
        ContenierPrice.IsAppearance = !(IsSelectedElement.IsAppearance || ButtonSelect.IsAppearance);
        if (ContenierPrice.IsAppearance) TextPrice.Text = Objects[Current].Price.ToString();

        if (Current + 1 == Objects.Length) RightArrow.Hide();
        else RightArrow.Show();

        if (Current == 0) LeftArrow.Hide();
        else LeftArrow.Show();
    }

    public void UpdateViewer()
    {
        var items = ItemsBase.Base.ObjectGames.ToArray();

        for (int i = 0; i < items.Length; ++i)
        {
            if (items[i].IsSelected) { Current = i; break; }
        }

        // Contenier.position = 
        TargetFollowScript.SetForcePosition(new Vector3(0, 0, -255) - new Vector3(OffsetX * Current, 0, 0));
        TargetFollowScript.SetTarget(new Vector3(0, 0, -255) - new Vector3(OffsetX * Current, 0, 0));
    }

    public void SelectCurrent()
    {
        var item = ItemsBase.Base.ObjectGames.ItemIdToItem(Objects[Current].ID);
        if (item.PlayerHave)
        {
            ItemsBase.Base.ObjectGames.SetSelected(Objects[Current].ID, true);
            ObjectGameChanger.Manager.SetObjectGamePrefab(ItemsBase.Base.ObjectGames.ItemIdToPrefab(Objects[Current].ID));
        }
        UpdateEnvironment();
    }

    public void BuyCurrent()
    {
        if (ItemsBase.Base.ObjectGames.ItemIdToHave(Objects[Current].ID)) return;
        if (Coins.Manager.CoinsCount < Objects[Current].Price) return;

        Coins.Manager.CoinsCount -= Objects[Current].Price;
        ItemsBase.Base.ObjectGames.SetHave(Objects[Current].ID, true);
        WorldEther.BuyEvent.Push(null, null);

        if (BuyToSelected) SelectCurrent();

        UpdateEnvironment();
    }

    private class Leaf
    {
        public GameObject ObjectGame;
        public int ID;
        public int Price;

        public Leaf(GameObject obj, int price, int id)
        {
            ObjectGame = obj;
            Price = price;
            ID = id;
        }
    }

    void Awake()
    {
        Initialize();
    }
}
