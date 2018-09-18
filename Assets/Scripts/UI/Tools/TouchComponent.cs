using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UIOrganization
{
    public class TouchComponent : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {

        public bool Active = true;
        [SerializeField]
        private bool OnlyClick = true;

        public event System.Action EventClick;

        public UnityEvent OnClick;
        [SerializeField]
        private UnityEvent OnDown;
        [SerializeField]
        private UnityEvent OnUp;


        public void OnPointerClick(PointerEventData data) { if (!Active) return; OnClick.Invoke(); if (EventClick != null) EventClick(); }
        public void OnPointerDown(PointerEventData data) { if (Active && !OnlyClick) OnDown.Invoke(); }
        public void OnPointerUp(PointerEventData data) { if (Active && !OnlyClick) OnUp.Invoke(); }
    }
}