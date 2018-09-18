using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockContainierController : MonoBehaviour
{
    [SerializeField] Appearance Target;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Target == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Target", name);
            enabled = false;
            return;
        }
        Target.EventShow += ShowContainier;
        Target.EventHide += HideContainier;
    }
    private void ShowContainier()
    {
        var main = UIContenier.Contenier.GetMainMenuController().GetMainObjectGame();
        var other = UIContenier.Contenier.GetMainMenuController().GetOtherObjectGame();

        if (main != null)
            main.Show();
        if (other != null)
            other.Show();

    }
    private void HideContainier()
    {
        var main = UIContenier.Contenier.GetMainMenuController().GetMainObjectGame();
        var other = UIContenier.Contenier.GetMainMenuController().GetOtherObjectGame();

        if (main != null)
            main.Hide();
        if (other != null)
            other.Hide();
    }
    void Awake()
    {
        Initialize();
    }
}
