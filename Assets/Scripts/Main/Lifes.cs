using UnityEngine;

public class Lifes : MonoBehaviour
{
    public static Lifes LifesManager { private set; get; }

    public int MaxLifes;

    private int _CountLifes;
    public int CountLifes
    {
        set
        {
            value = Mathf.Clamp(value, 0, MaxLifes);
            if (value != _CountLifes)
            {
                _CountLifes = value;
                WorldEther.ChangeLifes.Push(this, null);
            }
        }
        get { return _CountLifes; }
    }
    [ContextMenu("Fill to max")]
    private void EditorMax()
    {
        CountLifes = MaxLifes;
    }

    public void SetCountLifes(int value)
    {
        CountLifes = value;
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (LifesManager == null)
        {
            LifesManager = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    void Awake()
    {
        Initialize();
    }
}