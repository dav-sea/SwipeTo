using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifesViewer : MonoBehaviour
{
    public float Width = 10;
    List<LeafLife> LifeObjects;

    public Vector3 Offset;

    private Transform _transform;

    [SerializeField]
    private Transform LifesContenier;

    public bool IsHide { protected set; get; }

    public void Hide()
    {
        IsHide = true;
        foreach (LeafLife lifeObject in LifeObjects)
            lifeObject.Hide();
    }

    public void Show()
    {
        IsHide = false;
        foreach (LeafLife lifeObject in LifeObjects)
            lifeObject.Show();
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        _transform = transform;
        WorldEther.ChangeLifes.Subscribe(ListnerLifeChange);
        LifeObjects = new List<LeafLife>(1);
        if (LifesContenier == null)
            LifesContenier = _transform;
    }
    void Awake()
    {
        Initialize();
    }

    protected void UpdateView()
    {
        int diff = Lifes.LifesManager.CountLifes - LifeObjects.Count;
        ChangeObjectLife(diff);
        UpdatePositions();
    }

    protected void ChangeObjectLife(int change)
    {
        System.Action Step = null;
        if (change > 0) Step = delegate { AddObjectLife(); --change; };
        else Step = delegate { DeleteObjectLife(); ++change; };
        while (change != 0) Step.Invoke();
    }

    protected void AddObjectLife()
    {
        var lifeObject = new LeafLife(Instantiate(PrefabsHelper.PrefabLifeObject));
        var transf = lifeObject.GameObject.transform;
        transf.parent = LifesContenier;
        transf.position = Offset;
        if (IsHide) lifeObject.GameObject.SetActive(false);
        else { lifeObject.Show(); }

        LifeObjects.Add(lifeObject);
    }

    protected void DeleteObjectLife()
    {
        var lifeObject = LifeObjects[0];
        LifeObjects.RemoveAt(0);

        lifeObject.Hide();
        Destroy(lifeObject.GameObject, 3);
    }

    public void UpdatePositions()
    {
        var array = LifeObjects.ToArray();
        var coff = Width / array.Length;
        var offset = new Vector3(Offset.x + LifesContenier.position.x - Width / 2, Offset.y + LifesContenier.position.y, Offset.z + LifesContenier.position.z);
        // var offset = new Vector3(Offset.x - Width / 2, Offset.y, Offset.z);

        for (int num = 0; num < array.Length; ++num)
            array[num].SetTarget(new Vector3(coff * num + coff / 2 + offset.x, offset.y, offset.z));
    }

    private void ListnerLifeChange(Ethers.Channel.Info info)
    {
        UpdateView();
    }

    private class LeafLife
    {
        public TargetFollowScript FollowScript;
        public Appearance Appearance;
        public GameObject GameObject { private set; get; }

        private LeafLife() { }

        public LeafLife(GameObject gameObject)
        {
            GameObject = gameObject;
            FollowScript = gameObject.GetComponent<TargetFollowScript>();
            Appearance = gameObject.GetComponent<Appearance>();
        }

        public void Hide()
        {
            Appearance.Hide();
        }
        public void Show()
        {
            Appearance.Show();
        }
        public void SetTarget(Vector3 target)
        {
            FollowScript.SetTarget(target);
        }
    }

}