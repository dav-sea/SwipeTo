// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AppearanceModule : MonoBehaviour
// {
//     #region PRIVATE FIELDS
//     private bool _IsActive;
// 	private bool _initialized;
//     #endregion

//     #region  PUBLIC METHODS
//     public bool ActiveModule
//     {
//         set { _IsActive = value; OnChangeActiveModule(); }
//         get { return _IsActive; }
//     }
//     #endregion

//     protected virtual void OnChangeActiveModule() { }
//     #region Initialize
//     public void Initialize()
//     {
//         if (_initialized) return;
//         _initialized = true;
//         //Initialize logic
//         OnInitialize();
//     }

//     protected virtual void OnInitialize() { }
//     #endregion
// }
