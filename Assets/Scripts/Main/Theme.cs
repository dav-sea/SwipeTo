using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class Theme : MonoBehaviour
{
    [SerializeField] Palette.PaletteClaster PaletteConfiguration;
    public Palette.PaletteClaster Palette { get { return PaletteConfiguration; } }

    [SerializeField] GameObject BackgroundPrefab;
    public GameObject PrefabBackground { get { return BackgroundPrefab; } }
}