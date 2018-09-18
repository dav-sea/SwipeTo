using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIOrganization
{
    public class SimpleAppearance : MonoBehaviour
    {

        [SerializeField]
        Appearance TargetScreen;

        public bool ActiveModule;

        private bool _initialized;
        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            //Initialize logic
            if (TargetScreen == null)
            {
                Debug.LogWarningFormat("{0} (in {1}) is null", "TargetScreen", name);
                return;
            }

            TargetScreen.Initialize();
            TargetScreen.EventShow += delegate { if (ActiveModule) Show(); };
            TargetScreen.EventHide += delegate { if (ActiveModule) Hide(); };
        }
        void Awake()
        {
            Initialize();
        }

        protected void Show()
        {
            gameObject.SetActive(true);
        }
        protected void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}