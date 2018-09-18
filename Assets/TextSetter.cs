using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class TextSetter : MonoBehaviour
{
    [SerializeField] TextMesh TartetText;
    [SerializeField] UnityEngine.UI.Text TargetTextCanvas;
    public string Text
    {
        set
        {
            if (TargetTextCanvas != null)
                TargetTextCanvas.text = value;
            if (TartetText != null)
                TartetText.text = value;
        }
        get { return TartetText != null ? TartetText.text : TargetTextCanvas != null ? TargetTextCanvas.text : null; }
    }
}
