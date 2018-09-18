using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace BattleCrow
{
    //[ExecuteInEditMode]
    public class Appearance : Component
    {
        private List<Module> Modules = new List<Module>(1);
        private bool _show;
        private GameObject cache_GameObject;
        public GameObject GameObject
        {
            get
            {
                if (cache_GameObject == null) cache_GameObject = gameObject;
                return cache_GameObject;
            }
        }

        public event Action EventShow = delegate { };
        public event Action EventHide = delegate { };

        public T GetModule<T>() where T : Module
        {
            foreach (var e in Modules)
                if (e is T)
                    return e as T;
            return null;
        }

        public bool ActivateBeforeShow;
        public bool IsAppearance
        {
            set
            {
                if (value) Show();
                else Hide();
            }
            get { return _show; }
        }
        public void Show()
        {
            if (_show == true) return;
            if (ActivateBeforeShow && !GameObject.activeSelf) GameObject.SetActive(true);
            _show = true;
            EventShow();
        }
        public void Hide()
        {
            if (_show == false) return;
            _show = false;
            EventHide();
        }

        public abstract class Module : Script
        {
            private Appearance _attached;
            public Appearance Attached
            {
                set
                {
                    if (_attached == value) return;

                    if (_attached != null)
                    {
                        _attached.Modules.Remove(this);
                        _attached.EventShow -= OnShow;
                        _attached.EventHide -= OnHide;
                    }
                    _attached = value;

                    if (_attached != null)
                    {
                        _attached.Modules.Add(this);
                        _attached.EventShow += OnShow;
                        _attached.EventHide += OnHide;
                    }
                    ChangeAttached();
                }
                get { return _attached; }
            }


            public override void Initialize()
            {
                if (_attached == null)
                {
                    enabled = false;
                    InitializeFailed("Need attach \"Apperance\" in the field \"Attached\"");
                }
            }

            protected abstract void OnShow();
            protected abstract void OnHide();
            protected virtual void ChangeAttached() { }
        }
    }


}