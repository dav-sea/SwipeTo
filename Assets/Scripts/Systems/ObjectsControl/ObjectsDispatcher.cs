using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectsDispatcher<VerfiticationComponent> : MonoBehaviour where VerfiticationComponent : Component
{
    [Header("Dispatcher settings")]
    [Space(5)]
    [Header("-Create settings")]
    [SerializeField]
    private GameObject Prefab;

    [SerializeField]
    private int MaximumCountObjects = 64;

    [Header("-Verfitication settings")]

    [SerializeField]
    private bool VerfiticationOfComponent = true;
    [SerializeField]
    private bool VerfiticationOfTag = false;
    [SerializeField]
    private string Tag = "Untagget";

    [Header("-Cache settings")]
    [SerializeField]
    private bool CachingComponents = false;
    [SerializeField]
    private bool ActiveTriming = false;

    [Header("-Register settings")]
    [SerializeField]
    private Transform Parent;

    [SerializeField]
    public TypesTransformation ModeTransform = TypesTransformation.None;

    public bool StartActive = false;

    [Header("-Find settings")]
    [SerializeField]
    private bool FindBeforeFirstCallGetObjects = true;
    [SerializeField]
    private bool FirstFindInStart = true;
    [SerializeField]
    private bool FindInStart = false;

    //__________________________________________________________________________//

    private List<Box> Objects = new List<Box>(0);
    private bool _WasFind;

    //__________________________________________________________________________//

    public GameObject[] GetGameObjects()
    {
        if (!_WasFind && FindBeforeFirstCallGetObjects)
        {
            FindChilds();
            _WasFind = true;
        }
        GameObject[] objs = new GameObject[Objects.Count];
        for (int i = 0; i < objs.Length; ++i)
            objs[i] = Objects[i].Object;
        return objs;
    }

    public VerfiticationComponent[] GetVerfiticationComponents()
    {
        if (!_WasFind && FindBeforeFirstCallGetObjects)
        {
            FindChilds();
            _WasFind = true;
        }
        VerfiticationComponent[] objs = new VerfiticationComponent[Objects.Count];
        for (int i = 0; i < objs.Length; ++i)
            objs[i] = Objects[i].ObjectComponent;
        return objs;
    }

    public void FindChilds()
    {
        RegisterObjects(FindChilds(Parent));
    }

    public void RegisterObjects(IEnumerable<GameObject> objs)
    {
        foreach (GameObject e in objs)
            RegisterObject(e);
    }

    public bool RegisterObject(GameObject obj)
    {
        if (!Verfitication(obj))
        {
            // Debug.LogFormat("ObjectsDispatcher<{0}>({1}): RegisterObject(GameObject obj) - obj not verfitication", typeof(VerfiticationComponent), gameObject.name);
            return false;
        }

        if (Add(obj) != null)
        {
            DefaultValuesApply(obj);
        }
        else return false;

        return true;
    }

    public bool Verfitication(GameObject obj)
    {
        if (obj == null || (!VerfiticationOfTag && !VerfiticationOfComponent)) return false;
        if (VerfiticationOfTag && obj.tag != Tag) return false;
        if (VerfiticationOfComponent && obj.GetComponent(typeof(VerfiticationComponent)) == null) return false;
        return true;
    }

    public void SetPrefab(GameObject prefab)
    {
        if (prefab != null) Prefab = prefab;
        else Debug.LogFormat("ObjectsDispatcher({0}):SetPrefab(GameObject prefab) - prefab is null");
    }

    public bool Delete(GameObject obj)
    {
        for (int i = 0; i < Objects.Count; ++i)
            if (Objects[i].Object == obj)
            {
                DeleteObject(Objects[i]);
                return true;
            }
        return false;
    }

    public void TrimExcess()
    {
        Objects.TrimExcess();
    }

    public void FillToUp(int up)
    {

        up = Mathf.Clamp(up, 0, MaximumCountObjects) - Objects.Count;

        for (int i = 0; i < up; ++i)
        {
            CreateToGameObject();
        }
    }

    public void Clear()
    {
        bool val = ActiveTriming;
        ActiveTriming = false;
        for (int i = 0; i < Objects.Count; ++i)
            DeleteObject(Objects[i]);
        ActiveTriming = val;
        if (val) Objects.TrimExcess();
    }

    public GameObject CreateToGameObject()
    {
        if (Objects.Count + 1 > MaximumCountObjects) return null;
        return Add(CreateObject()).Object;
    }

    public VerfiticationComponent CreateToComponent()
    {
        if (Objects.Count + 1 > MaximumCountObjects) return null;
        return Add(CreateObject()).ObjectComponent;
    }

    //__________________________________________________________________________//

    private GameObject CreateObject()
    {
        if (Prefab == null) return null;
        GameObject obj = Instantiate(Prefab, Vector3.zero, Quaternion.identity);
        // Debug.Log("Create - " + obj.name);
        DefaultValuesApply(obj);
        return obj;
    }

    private void DefaultValuesApply(GameObject obj)
    {
        if (obj == null) return;
        var objvalues = obj.transform;
        objvalues.parent = Parent;
        obj.SetActive(StartActive);
        switch (ModeTransform)
        {
            case TypesTransformation.LocalZero:
                objvalues.localPosition = Vector3.zero;
                objvalues.localRotation = Quaternion.identity;
                break;

            case TypesTransformation.LocalPrefab:
                objvalues.localPosition = Prefab.transform.localPosition;
                objvalues.localRotation = Prefab.transform.localRotation;
                objvalues.localScale = Prefab.transform.localScale;
                break;
        }
    }

    private Box Add(GameObject obj)
    {
        if (obj == null) return null;
        var box = new Box(obj, CachingComponents);
        AddObject(box);
        return box;
    }

    private void DeleteObject(Box obj)
    {
        Objects.Remove(obj);
        if (ActiveTriming) Objects.TrimExcess();
        if (obj.Object != null)
        {
            // Debug.Log("Destroy - " + obj.Object.name);
            Destroy(obj.Object, 1);
        }
    }

    private bool AddObject(Box obj)
    {
        if (Objects.Count + 1 > MaximumCountObjects) return false;
        Objects.Add(obj);
        return true;
    }

    private GameObject[] FindChilds(Transform parent)
    {
        List<GameObject> list = new List<GameObject>(0);

        GameObject[] now = new GameObject[Objects.Count];
        for (int i = 0; i < now.Length; ++i)
            now[i] = Objects[i].Object;

        GameObject obj;

        bool flag;

        for (int i = 0; i < parent.childCount; ++i)
        {
            flag = false;
            obj = parent.GetChild(i).gameObject;

            for (int j = 0; j < now.Length; ++j)
                if (obj == now[j])
                {
                    flag = true;
                    break;
                }

            if (!flag)
            {
                list.Add(obj);
            }
        }

        return list.ToArray();
    }
    //__________________________________________________________________________//

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if (FindInStart || (FirstFindInStart && !_WasFind))
        {
            FindChilds();
            _WasFind = true;
        }
    }

    /// <summary>
    /// Reset is called when the user hits the Reset button in the Inspector's
    /// context menu or when adding the component the first time.
    /// </summary>
    void Reset()
    {
        _WasFind = false;
    }

    //__________________________________________________________________________//
    private class Box
    {
        public GameObject Object;
        private VerfiticationComponent _Cache;
        public VerfiticationComponent ObjectComponent
        {
            private set { _Cache = value; }
            get
            {
                if (_Cache == null)
                    _Cache = Object.GetComponent<VerfiticationComponent>();
                return _Cache;

            }
        }

        public Box(GameObject obj, bool Caching)
        {
            Object = obj;
            if (Caching) ObjectComponent = obj.GetComponent<VerfiticationComponent>();
        }
    }

    public enum TypesTransformation { None, LocalZero, LocalPrefab }
}
