#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceSignHelper : MonoBehaviour
{
    [ContextMenu("Solidation")]
    private void Ediotr_solidation()
    {
        var action = gameObject.GetComponent<ActionComponent>();

        var appearance = action.Editor_FindAppearence();
        appearance.ActivateOnShow = true;

        var scalescript = GetComponent<TargetScaleScript>();
        if (scalescript == null) scalescript = gameObject.AddComponent<TargetScaleScript>();
        scalescript.DisableForFinish = true;
        scalescript.enabled = false;

        var scaleappereance = GetComponent<AppearanceScale>();
        if (scaleappereance == null) scaleappereance = gameObject.AddComponent<AppearanceScale>();
        scaleappereance.FindAll();
    }
}

#endif