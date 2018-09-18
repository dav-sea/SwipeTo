using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIOrganization;

public class LockTouch : MonoBehaviour
{
    [SerializeField] TouchComponent TouchComponent;
    public event System.Action<bool> EventChangeLock;

    public bool Lock
    {
        set
        {
            TouchComponent.Active = !value;
            // Debug.Log("" + TouchComponent.Active);
            if (EventChangeLock != null) EventChangeLock(value);
        }
        get { return !TouchComponent.Active; }
    }

}
