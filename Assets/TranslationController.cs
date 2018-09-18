using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationController : MonoBehaviour
{

    [SerializeField]
    public string NameTranslation;
    [SerializeField]
    private bool UpdateOnStart = true;



    [Space(5)]
    [SerializeField]
    private bool SendEvent = false;
    [SerializeField]
    UnityEngine.Events.UnityEvent TranslationEvent;


    [Space(5)]
    [SerializeField]
    private bool SendText = true;
    [SerializeField]
    TextMesh MeshText;
    [SerializeField]
    UnityEngine.UI.Text UIText;




    public void UpdateTranslation()
    {
        if (SendEvent) TranslationEvent.Invoke();
        if (SendText)
        {
            var transl = TranslationManager.GetText(NameTranslation);
            if (MeshText == null && UIText == null)
            {
                Debug.LogWarningFormat("{0} (in {1}) is null", "", "TARGET");
                return;
            }
            if (MeshText != null) MeshText.text = transl;
            if (UIText != null) UIText.text = transl;
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if (UpdateOnStart)
            UpdateTranslation();
    }
}
