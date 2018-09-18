using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIOrganization
{
    public class UIController : MonoBehaviour
    {
        #region STATICS & CONTROLLER
        //======================================================================================//
        public static UIController Controller { private set; get; }

        public static void BackScreen()
        {
            Controller.Back();
        }
        public static bool BackToScreen(Screen target)
        {
            return Controller.BackTo(target);
        }
        public static void BackToRootScreen()
        {
            Controller.BackToRoot();
        }
        public static void ShowScreen(Screen target)
        {
            Controller.Show(target);
        }
        //======================================================================================//
        #endregion

        #region PRIVATE FIELDS
        //======================================================================================//
        private RootChangebleStack<Screen> Screens;
        //======================================================================================//
        #endregion;

        #region PUBLIC FIELDS
        //======================================================================================//
        public Screen ActiveScreen { get { return Screens.Get(); } }

        public bool Debuging = false;

        public int CountScreens { get { return Screens.Count; } }
        //======================================================================================//
        #endregion

        //======================================================================================//
        public void Back()
        {
            // if(Screens.CurrentIsRoot())
            // {

            // }
            Screen screen = Screens.Pop();
            while (screen != null && screen.IsAttached())
            {
                screen.Hide();
                if (!Screens.CurrentIsRoot())
                    screen = Screens.Pop();
                else break;
            }

            if (screen != null)
            {
                screen.Hide();
                screen.BackScreen();
                ActiveScreen.Show();
                if (Debuging) Log(string.Format("Back screen action.\nPrevious: \"{0}\"; Current: \"{1}\".", screen.name, ActiveScreen.name));
            }
            else if (Debuging) Log("Back screen action was failed.\nActive screen was null");
            WorldEther.ChangeScreen.Push(Controller, null);
        }

        // private void HideActive()
        // {
        //     Screen screen = Screens.Pop();
        //     if (screen != null)
        //         screen.Hide();
        // }

        private void HideActiveScreen()
        {
            Screen screen = Screens.Pop();
            while (screen != null && screen.IsAttached())
            {
                screen.Hide();
                if (!Screens.CurrentIsRoot())
                    screen = Screens.Pop();
                else break;
            }

            if (screen != null)
            {
                screen.Hide();
                // if (Debuging) Log(string.Format("Hide screen action.\nPrevious: \"{0}\"; Current: \"{1}\".", screen.name, ActiveScreen.name));
            }
            // else if (Debuging) Log("Back screen action was failed.\nActive screen was null");
        }

        // public bool ThrowScreen(Screen screen)
        // {
        //     var screens = Screens.GetAllScreens();

        //     int count;
        //     for (count = screens.Length - 1; count >= 0; --count)//Поиск скринна в стеке
        //         if (screens[count] == screen)
        //             break;
        // }



        public void Clear(Screen newScreen)
        {
            if (newScreen == null) return;

            while (Screens.PopCount != 0)
                HideActiveScreen();
            Show(newScreen);
        }



        public bool Contains(Screen screen)
        {
            return Screens.Contains(screen);
        }

        public bool BackTo(Screen screen)
        {
            if (screen == null) return false;
            var screens = Screens.ToArray();
            int count;

            for (count = screens.Length - 1; count >= 0; --count)//Поиск скринна в стеке
                if (screens[count] == screen)
                    break;

            if (count < 0) return false;//Если не скринн не найден в стеке

            for (count = screens.Length - count - 1; count >= 0; --count)
                HideActiveScreen();

            WorldEther.ChangeScreen.Push(Controller, null);

            return true;
        }
        public bool Delete(Screen screen)
        {
            var res = Screens.Delete(screen);
            if (res)
            {
                screen.Hide();
                if (Debuging) Log(string.Format("Delete {0}", screen.name));
            }
            return res;
        }
        public void CreateNewRoot(Screen newScreen)
        {
            if (Screens != null && Screens.Count > 0)
            {
                BackToRoot();
                ActiveScreen.Hide();
            }
            Screens = new RootChangebleStack<Screen>(newScreen);
            newScreen.Show();
        }
        public void BackToRoot()
        {
            while (Screens.PopCount != 0)
                Screens.Pop().Hide();
            ActiveScreen.Show();
        }
        public void Show(Screen screen)
        {
            if (screen == null)
            {
                if (Debuging) Log("Show Screen action was failed.\nTarget screen was null");
                return;
            }

            // screen.Initialize();

            if (Debuging) Log("Show Screen action.");

            if (!screen.IsOnTopScreen())//Если screen не является поверхностным
            {
                Screen currentScreen = ActiveScreen;
                while (currentScreen != null)
                {
                    currentScreen.Hide();
                    // Debug.Log("here");
                    if (currentScreen.IsOnTopScreen())
                    {
                        Screens.Pop();
                    }
                    else
                    {
                        break;
                    }
                    currentScreen = ActiveScreen;
                }
            }

            Screens.Push(screen);
            screen.Show();
            WorldEther.ChangeScreen.Push(Controller, null);
        }
        public void ShowOnce(Screen screen)
        {
            if (!Contains(screen))
                Show(screen);
        }
        //======================================================================================//
        private void Log(string message)
        {
            Debug.Log("UI: " + message);
        }

        #region INITIALIZE & UNITY MESSAGE METHODS
        //======================================================================================//
        private bool _initialized;
        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            //Initialize logic
            if (Controller != null)
            {
                Debug.LogWarning("Attempt to create a secondary UIController");
                Destroy(this);
                return;
            }
            Controller = this;

        }
        void Awake()
        {
            Initialize();
        }
        //======================================================================================//
        #endregion

    }
}