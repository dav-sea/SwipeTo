using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIOrganization
{
    public class Screen : Appearance
    {
        public event AppearanceAction EventBack;

        [SerializeField]
        private bool OnTopScreen = false;
        [SerializeField]
        private bool AttachToLastScreen = false;

        public bool IsOnTopScreen()
        {
            return OnTopScreen;
        }

        public bool IsAttached()
        {
            return AttachToLastScreen;
        }

        protected override void OnInitialize()
        {
            EventBack += delegate { };
        }

        public void BackScreen()
        {
            EventBack.Invoke();
        }

    }
}