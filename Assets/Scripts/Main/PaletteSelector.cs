using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteSelector : MonoBehaviour
{

    public Palette.PaletteClaster Configuration;

    [ContextMenu("Select")]
    public void Select()
    {
        Palette.PaletteManager.SetColors(Configuration);
    }
}
