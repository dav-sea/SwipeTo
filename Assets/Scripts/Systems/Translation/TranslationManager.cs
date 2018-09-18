using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml;

public class TranslationManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset TranslationMeta;
    public static TranslationManager Manager { private set; get; }

    [SerializeField]
    private bool AutoLoadOnInitialize = true;
    [SerializeField]
    private bool AutoDefinitionLanguge = true;
    [SerializeField]
    private bool DebugTranslation = false;

    List<TranslationMap> Maps = new List<TranslationMap>();

    public static string GetText(string name)
    {
        return Manager.Current.GetTextTranslation(name);
    }

    private TranslationMap _current;
    public TranslationMap Current
    {
        set
        {
            _current = value;
            if (_current == null)
                _current = Maps[0];
        }
        get { return _current; }
    }

    public void LoadTranslationMeta(TextAsset text)
    {
        var document = XDocument.Parse(text.text);
        // Debug.Log("" + text.text);
        foreach (XElement meta in document.Root.Elements("TranslationCache"))
            Maps.Add(new TranslationMap(meta.Attribute("Languge").Value, meta.Value));

    }

    public void SetCurrent(string language)
    {
        foreach (TranslationMap map in Maps)
            if (map.Language == language)
            {

                Current = map;
                return;
            }
        //Не вызываю SetCurrent("English") что бы избежать возможной рекурсии
        foreach (TranslationMap map in Maps)
            if (map.Language == "English")
                Current = map;
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Manager != null)
        {
            Destroy(this);
            return;
        }
        Manager = this;
        Debug.Log("System Languge: " + Application.systemLanguage);
        if (AutoLoadOnInitialize)
        {
            LoadTranslationMeta(TranslationMeta);
            if (DebugTranslation)
                SetCurrent("Debug");
            else if (AutoDefinitionLanguge)
                SetCurrent(Application.systemLanguage.ToString());
            // SetCurrent("German");
        }
        Debug.Log("Current Languge: " + Current.Language);
    }
    void Awake()
    {
        Initialize();
    }

#if UNITY_EDITOR
    [ContextMenu("Log")]
    private void Editor_log()
    {
        Debug.Log("System Languge: " + Application.systemLanguage);
        Debug.Log("Current Languge: " + Current.Language);
    }
#endif
}