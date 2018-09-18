using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml.Linq;

// namespace Translation
// {
// [Serializable]
public class TranslationMap
{
    public string Language { private set; get; }

    private CacheTextTranslate TextTranslationCache;

    public TranslationMap(string language, string traslationPath)
    {
        Language = language;
        TextTranslationCache = new CacheTextTranslate(language, traslationPath);
    }

    public string GetTextTranslation(string Name)
    {
        foreach (ITranslationElement<string> e in TextTranslationCache.Elements) if (Name == e.Name) return e.Value;
        return null;
    }

    public string GetLanguage() { return Language; }

    private TranslationMap() { }
}

public abstract class CacheTranslate<T> : IDisposable
{
    public bool IsCached { private set; get; }
    public string TargetLanguge { private set; get; }
    public string CachePath { set; get; }

    private ITranslationElement<T>[] _cache;
    public ITranslationElement<T>[] Elements
    {
        private set { _cache = value; }
        get
        {
            if (!IsCached) Cache();
            return _cache;
        }
    }

    public void Cache()
    {
        Dispose();
        Elements = CacheManual(CachePath, TargetLanguge).ToArray();
        IsCached = true;
    }

    protected abstract List<ITranslationElement<T>> CacheManual(string resourcePath, string language);

    public void Dispose()
    {
        Elements = null;
        IsCached = false;
    }

    private CacheTranslate() { }
    public CacheTranslate(string languge, string cachePath)
    {
        TargetLanguge = languge;
        CachePath = cachePath;
    }
}

public class CacheTextTranslate : CacheTranslate<string>
{
    protected override List<ITranslationElement<string>> CacheManual(string resourcePath, string language)
    {
        // Debug.Log("" + resourcePath);
        // Debug.Log(Resources.LoadAll("").Length);
        var document = XDocument.Parse(Resources.Load<TextAsset>(resourcePath).text);

        if (document != null)
        {
            var maps = document.Root.Elements("TranslationMap");
            foreach (XElement map in maps)
            {
                if (map.Attribute("Languge").Value == language)
                    return ReadTranslationMap(map);
            }
        }

        return new List<ITranslationElement<string>>(0);
    }

    protected List<ITranslationElement<string>> ReadTranslationMap(XElement map)
    {
        var traslation = new List<ITranslationElement<string>>();
        foreach (XElement element in map.Elements("Element"))
            traslation.Add(CreateElement(element));
        return traslation;
    }

    protected ITranslationElement<string> CreateElement(XElement element)
    {
        return new TranslationText(element.Attribute("Name").Value, element.Value);
    }

    public CacheTextTranslate(string language, string cachePath)
    : base(language, cachePath) { }
}

public interface ITranslationElement<T>
{
    string Name { get; }
    T Value { get; }
}

// [Serializable]
public class TranslationText : ITranslationElement<string>
{
    public string Name { protected set; get; }
    public string Value { protected set; get; }

    public TranslationText(string name, string value)
    {
        Name = name;
        Value = value;
    }

    private TranslationText() { }
}
// }