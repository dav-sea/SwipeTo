using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIOrganization
{
    public class ActionScaler : MonoBehaviour
    {
        [SerializeField]
        private TargetScaleScript ScaleScript;

        // private Transform _transform;
        public float NormalValue = 1;
        public float ScaleValue = 1.1f;

        public void Scale()
        {
            SetScale(ScaleValue);
        }
        public void UnScale()
        {
            SetScale(NormalValue);
        }


        private bool _initialized;
        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            //Initialize logic
            ScaleScript.Initialize();
            ScaleScript.DisableForFinish = true;
            ScaleScript.Accelerate.AccelerateValue = 5;
            // _transform = ScaleScript.transform;
            SetScale(NormalValue);
        }
        private void SetScale(float scale)
        {
            ScaleScript.SetTarget(new Vector3(scale, scale, scale));
        }
        public void SetForceNormal()
        {
            ScaleScript.GetTransform().localScale = new Vector3(NormalValue, NormalValue, NormalValue);
        }
        public void SetForceScale()
        {
            ScaleScript.GetTransform().localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);
        }
        // private float GetScale()
        // {
        //     return Vector3Avg(_transform.localScale);
        // }
        // private float Vector3Avg(Vector3 vector)
        // {
        //     return (vector.x + vector.y + vector.z) / 3f;
        // }
        void Awake()
        {
            Initialize();
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
    }
}