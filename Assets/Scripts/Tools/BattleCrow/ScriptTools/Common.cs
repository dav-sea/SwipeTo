using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace BattleCrow
{
    ///<summary>
    /// Класс расширяющий UnityEngine.Behaviour.
    /// Глубокая интеграция с BattleCrow Tools.
    /// 1.0
    ///</summary>
    public abstract class Script : Behaviour, IInitializeble
    {
        private Transform cache_Transform;
        private GameObject cache_GameObject;

        public Transform Transform
        {
            get
            {
                if (cache_Transform == null)
                    cache_Transform = transform;
                return cache_Transform;
            }

        }
        public GameObject GameObject
        {
            get
            {
                if (cache_GameObject == null)
                    cache_GameObject = gameObject;
                return cache_GameObject;
            }
        }
        public bool Active
        {
            set { GameObject.SetActive(value); }
            get { return GameObject.activeSelf; }
        }

        public virtual void Initialize() { }

        protected void Awake()
        {
            Initialize();
            OnAwake();
        }

        protected static void Log(string text, LogLevel level = 0)
        {
            if (level == LogLevel.Error)
                Debug.Log(text);
            else if (level == LogLevel.Warning)
                Debug.LogWarning(text);
            else if (level == LogLevel.Error)
                Debug.LogError(text);

        }
        protected static void Logf(string format, LogLevel level, params object[] args)
        {
            if (level == LogLevel.Error)
                Debug.LogFormat(format, args);
            else if (level == LogLevel.Warning)
                Debug.LogWarningFormat(format, args);
            else if (level == LogLevel.Error)
                Debug.LogErrorFormat(format, args);
        }
        protected static void Logf(string format, params object[] args)
        {
            Logf(format, 0, args);
        }

        protected void InitializeFailed(string comment = "\0")
        {
            Logf("Initialize Failed! (obj name:{0})\n{1}", 1, GameObject.name, comment);
        }

        protected virtual void OnAwake() { }

        protected enum LogLevel { Message = 0, Warning = 1, Error = 2 }
    }

    public interface IInitializeble
    {
        void Initialize();
    }


}