using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIOrganization
{
    public class ScreensDisactivator : MonoBehaviour
    {
        [SerializeField] RootScreenSetuper Setuper;

        [SerializeField]
        private Appearance[] Screens;

        public void Action()
        {
            for (int i = Screens.Length - 1; i >= 0; --i)
            {
                if (Screens[i] == null) Debug.Log("Screen is null in ScreensDisactivator");
                else Screens[i].gameObject.SetActive(false);
            }
            if (Setuper != null) Setuper.Setup();
        }

        private bool _initialized;
        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            //Initialize logic
            if (Setuper == null)
            {
                Debug.LogWarningFormat("{0} (in {1}) is null", "Setuper", name);
                enabled = false;
                return;
            }
        }
        void Start()
        {
            Action();
        }

        [ContextMenu("Trim")]
        private void Trim()
        {
            List<Appearance> NewScreens = new List<Appearance>(Screens.Length / 2);
            foreach (var e in Screens)
                if (e != null)
                    NewScreens.Add(e);
            Screens = NewScreens.ToArray();
        }
    }
}