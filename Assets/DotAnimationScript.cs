using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotAnimationScript : MonoBehaviour
{

    [SerializeField] private TextMesh Text;

    public void SetText(string text)
    {
        Text.text = text;
    }
}
