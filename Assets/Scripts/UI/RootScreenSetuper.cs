using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIOrganization
{
    public class RootScreenSetuper : MonoBehaviour
    {
        [SerializeField]
        private UIController Controller;
        [SerializeField]
        private Screen Screen;

        public void Setup()
        {
            Controller.Initialize();
            Screen.gameObject.SetActive(true);
            Controller.CreateNewRoot(Screen);
        }
        private bool _initialized;
        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            //Initialize logic
            if (Controller == null)
            {
                Debug.LogWarningFormat("{0} (in {1}) is null", "Controller", name);
                return;
            }
            if (Screen == null)
            {
                Debug.LogWarningFormat("{0} (in {1}) is null", "Screen", name);
                return;
            }
        }
        void Awake()
        {
            Initialize();
        }
    }
}