using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIOrganization
{
    public class AppearanceEvents : MonoBehaviour
    {
        [SerializeField]
        private Appearance TartetScreen;
        public bool ActiveModule = true;


        public bool SendShowMessage = true;
        [SerializeField]
        private UnityEvent OnShow;

        public bool SendHideMessage = true;
        [SerializeField]
        private UnityEvent OnHide;

        private bool _initialized;
        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            //Initialize logic
            if (TartetScreen == null)
            {
                Debug.LogWarningFormat("{0} (in {1}) is null", "TartetScreen", name);
                enabled = false;
                return;
            }
            TartetScreen.Initialize();
            TartetScreen.EventShow += delegate { if (ActiveModule && SendShowMessage) OnShow.Invoke(); };
            TartetScreen.EventHide += delegate { if (ActiveModule && SendHideMessage) OnHide.Invoke(); };
        }

        void Awake()
        {
            Initialize();
        }
    }
}