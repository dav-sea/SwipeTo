using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIOrganization
{
    public class ScalePosition : MonoBehaviour
    {
        public Camera Camera;
        [SerializeField]
        [Range(0, 1)]
        float X;
        [SerializeField]
        [Range(0, 1)]
        float Y;
        [SerializeField]
        float OffsetZ;
        [SerializeField]
        Vector2 Offset;

        public bool UseLocalPosition = false;

        public bool UpdateOnInitialize = true;

        Transform _transform;

        private bool _initialized;
        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            //Initialize logic
            _transform = transform;
            if (UpdateOnInitialize) UpdatePosition();
        }
        void Awake()
        {
            Initialize();
        }

        [ContextMenu("Update Position")]
        public void UpdatePosition()
        {
            if (Camera == null) { Debug.Log("Camera is null"); return; }
            if (_transform == null) _transform = transform;
            if (UseLocalPosition)
                _transform.localPosition = PositionScale(Camera, X, Y, OffsetZ, Offset);
            else _transform.position = PositionScale(Camera, X, Y, OffsetZ, Offset);
        }
        public static Vector3 PositionScale(Camera camera, float x, float y, float offsetZ, Vector2 offset)
        {
            return camera.ViewportPointToRay(new Vector3(x, y, 0)).GetPoint(offsetZ) + (Vector3)offset;
        }
        public static Vector3 PositionScale(Camera camera, Vector2 procents, float offsetZ, Vector2 offset)
        {
            return PositionScale(camera, procents.x, procents.y, offsetZ, offset);
        }
        public static Vector3 PositionScale(Camera camera, Vector2 procents, float offsetZ)
        {
            return PositionScale(camera, procents.x, procents.y, offsetZ, Vector2.zero);
        }
        public static Vector3 PositionUIScale(Vector2 procents, float offsetZ)
        {
            return PositionScale(UIContenier.Contenier.GetUICamera(), procents.x, procents.y, offsetZ, Vector2.zero);
        }
        public static Vector3 PositionMainCameraScale(Vector2 procents, float offsetZ)
        {
            return PositionScale(UIContenier.Contenier.GetMainCamera(), procents.x, procents.y, offsetZ, Vector2.zero);
        }
    }
}