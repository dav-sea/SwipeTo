using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsThemeViewer : MonoBehaviour
{
    Items<GameObject>.Item[] ThemePrefabs;

    [SerializeField]
    Appearance IsSelectedElement;
    [SerializeField]
    Appearance ButtonBuy;
    [SerializeField]
    Appearance ButtonSelect;

    [SerializeField]

    Appearance ContenierPrice;

    [SerializeField]
    TextSetter TextPrice;

    [SerializeField]
    Transform BlockContenier;
    [SerializeField]
    Appearance LeftArrow;
    [SerializeField]
    Appearance RightArrow;
    [SerializeField] private bool BuyToSelected = true;
    int Current;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
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
        if (BlockContenier == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", " BlockContenier", name);
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
    }
    void Awake()
    {
        Initialize();
    }

    private GameObject ObjectGame;

    public void FlashTheme()
    {
        ThemeController.Manager.SelectTheme(ItemsBase.Base.Themes.GetFirstSelectedToPrefab());
    }

    public void UpdateViewer()
    {
        if (ObjectGame != null)
        {
            ObjectGame.SetActive(false);
            Destroy(ObjectGame, 1);
        }
        for (int i = 0; i < ThemePrefabs.Length; i++)
        {
            if (ThemePrefabs[i].IsSelected)
            {
                Current = i;
                break;
            }
        }
        ObjectGame = Instantiate(PrefabsHelper.PrefabObjectGame, Vector3.zero, Quaternion.identity, transform);
        ObjectGame controller = ObjectGame.GetComponent<ObjectGame>();
        var transf = ObjectGame.transform;
        transf.parent = BlockContenier;
        transf.localPosition = new Vector3(0, 0, 0);
        transf.localScale = Vector3.one;
        controller.GetActionManager().UseBalanceGamePlayData = true;
        controller.ActivateDemo();
        controller.SetupFrontSide();
    }

    [ContextMenu("uptd")]
    public void InitializeViewer()
    {
        ThemePrefabs = ItemsBase.Base.Themes.ToArray();
    }

    public void UpdateEnvironment()
    {
        IsSelectedElement.IsAppearance = ItemsBase.Base.Themes.ItemIdToIsSelected(ThemePrefabs[Current].ID);

        ButtonSelect.IsAppearance = ItemsBase.Base.Themes.ItemIdToHave(ThemePrefabs[Current].ID) && !IsSelectedElement.IsAppearance;
        ButtonBuy.IsAppearance = ThemePrefabs[Current].Price <= Coins.Manager.CoinsCount && !(ButtonSelect.IsAppearance || IsSelectedElement.IsAppearance);
        if (ButtonBuy.IsAppearance || ButtonSelect.IsAppearance || IsSelectedElement.IsAppearance)
        {
            FlashCoinMessage();
        }
        else if (MessageCoin == null) MessageCoin = MessageManager.ShowMessage(TranslationManager.GetText("Message_NotEnoughCoins"));

        ContenierPrice.IsAppearance = !(IsSelectedElement.IsAppearance || ButtonSelect.IsAppearance);
        if (ContenierPrice.IsAppearance) TextPrice.Text = ThemePrefabs[Current].Price.ToString();

        if (Current + 1 == ThemePrefabs.Length) RightArrow.Hide();
        else RightArrow.Show();

        if (Current == 0) LeftArrow.Hide();
        else LeftArrow.Show();
    }

    public void FlashCoinMessage()
    {
        if (MessageCoin == null) return;
        MessageCoin.Hide();
        MessageCoin = null;
    }

    public void SelectCurrent()
    {
        var item = ItemsBase.Base.Themes.ItemIdToItem(ThemePrefabs[Current].ID);
        if (item.PlayerHave)
        {
            ItemsBase.Base.Themes.SetSelected(ThemePrefabs[Current].ID, true);
        }
        ItemsBase.Base.SaveSelected();
        UpdateEnvironment();
    }
    public void SwitchLeft()
    {
        if (Current == 0) return;
        Current--;
        ThemeController.Manager.SelectTheme(ThemePrefabs[Current].ObjectPrefab);
        UpdateEnvironment();
    }
    public void SwitchRight()
    {
        // Debug.Log("" + Current);
        if (Current + 1 == ThemePrefabs.Length) return;
        Current++;
        ThemeController.Manager.SelectTheme(ThemePrefabs[Current].ObjectPrefab);
        UpdateEnvironment();
    }
    MessageManager.IHideMessage MessageCoin;
    public void BuyCurrent()
    {
        if (ItemsBase.Base.Themes.ItemIdToHave(ThemePrefabs[Current].ID)) return;
        if (Coins.Manager.CoinsCount < ThemePrefabs[Current].Price) return;

        Coins.Manager.CoinsCount -= ThemePrefabs[Current].Price;
        ItemsBase.Base.Themes.SetHave(ThemePrefabs[Current].ID, true);
        WorldEther.BuyEvent.Push(null, null);

        if (BuyToSelected) SelectCurrent();

        UpdateEnvironment();
    }
}
