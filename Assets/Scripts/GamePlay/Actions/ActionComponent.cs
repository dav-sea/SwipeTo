using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ActionComponent : MonoBehaviour
{
    #region EVENTS
    //====================================================================================//
    public event System.Action EventSwipe;
    //====================================================================================//
    #endregion

    #region PRIVATE FIELDS
    [SerializeField]
    protected Appearance Appearance;
    #endregion

    #region PUBLIC FIELDS
    //====================================================================================//
    [HideInInspector]
    public Side.Diraction Diraction;//Направление ActionComponent 
    //====================================================================================//
    public Transform Transform { private set; get; }//Cahce transform (on Initialize)
    public GameObject GameObject { private set; get; }//Cahce gameObject (on Initialize)
    public abstract bool IsRotater();//true - если при вызове(вызовах) метода Swipe ObjectGame повернется
    public abstract bool IsSwipeble();//Возращает результат вызова метода bool Swipe()

    public abstract int ID { get; }
    public abstract int TrainingCount { set; get; }
    public abstract string TranslationTraining { get; }
    [Header("Action Settings")]
    public bool IsActiveAction;
    //====================================================================================//
    #endregion

    #region INITIALIZE & UNITY MESSAGE METHODS
    //====================================================================================//
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        Transform = transform;
        GameObject = gameObject;
        EventSwipe += delegate { };
        OnInitialize();
    }
    protected virtual bool OnInitialize() { return true; }
    //====================================================================================//
    void Awake()
    {
        Initialize();
    }
    //====================================================================================//
    #endregion

    #region PUBLIC METHODS
    //====================================================================================//
    public virtual void Show()
    {
        Appearance.Show();
    }

    public virtual void Hide()
    {
        Appearance.Hide();
        TrainingActive = false;
    }

    public virtual void ResetAction()
    {
        ResetTransform();
        Diraction = null;
        IsActiveAction = true;
        Appearance.Show();
    }
    public virtual bool WillRotate(Side.Diraction diraction)
    {
        return IsActiveAction && IsRotater() && IsSwipeble() && Diraction == diraction;
    }
    // Возвращает true если Action включен и OnSwipe вернет true 
    public bool Swipe(Side.Diraction diraction)
    {
        if (IsActiveAction && OnSwipe(diraction))
        {
            if (Training != null && !TrainingComplete && TrainingActive)
            {
                TrainingCount += 1;
                if (TrainingCount != 0)
                    Training.TextActive = false;
                if (TrainingCount >= 2)
                    TrainingComplete = true;
            }
            TrainingActive = false;

            EventSwipe();
            return true;
        }
        return false;
    }
    //====================================================================================//
    #endregion

    #region  OTHERS
    //====================================================================================//
    protected abstract bool OnSwipe(Side.Diraction diraction);
    //====================================================================================//
    protected void ResetTransform()
    {
        Transform.parent = null;
        Transform.localPosition = Vector3.zero;
        Transform.localRotation = Quaternion.identity;
        Transform.localScale = Vector3.one;
    }
    //====================================================================================//
    #endregion

    #region Training

    protected TrainingContrller Training;

    protected virtual void InitializeTraining() { }

    public virtual bool TrainingActive
    {
        set
        {
            if (Training == null)
            {
                if (!value) return;
                Training = Instantiate(PrefabsHelper.PrefabTrainingObject).GetComponent<TrainingContrller>();
                var tt = Training.transform;
                tt.parent = Transform;
                tt.localPosition = Vector3.zero;
                tt.localRotation = Quaternion.identity;
                tt.localScale = Vector3.one;
                Training.SetNameTranslation(TranslationTraining);
                InitializeTraining();
            }
            Training.ActiveTraining = value;
        }
        get { return Training != null && Training.ActiveTraining; }
    }

    public virtual bool TrainingComplete
    {
        set
        {
            PlayerPrefs.SetInt(ID + "STC", value ? 1 : 0);
        }
        get
        {
            return PlayerPrefs.GetInt(ID + "STC", 0) == 0 ? false : true;
        }
    }

    #endregion

    #region EDITOR HELPERS

    [ContextMenu("Find Appearence")]
    public Appearance Editor_FindAppearence()
    {
        Appearance = GetComponent<Appearance>();
        if (Appearance == null)
            Appearance = gameObject.AddComponent<Appearance>();
        // Appearance.Start
        return Appearance;
    }
    #endregion
}