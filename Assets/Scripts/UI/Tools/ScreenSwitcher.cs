using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIOrganization
{
    public class ScreenSwitcher : MonoBehaviour
    {
        [SerializeField]
        Screen Target;

        public void Switch()
        {
            UIController.ShowScreen(Target);
        }
    }
}