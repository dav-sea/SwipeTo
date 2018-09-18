using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIOrganization
{
    public class AppearanceAnimation : MonoBehaviour
    {

        public event Appearance.AppearanceAction EventFinishShow;

        public event Appearance.AppearanceAction EventFinishHide;

        [SerializeField]
        private Appearance Target;
        [SerializeField]
        private Animation Animation;
        [SerializeField]
        private AnimationClip ShowClip;
        [SerializeField]
        private AnimationClip HideClip;
        [SerializeField]
        private AnimationClip NormalClip;

        [SerializeField]
        private UnityEvent FinishShow;
        [SerializeField]
        private UnityEvent FinishHide;

        [SerializeField]
        private bool SendFinishMessage = false;

        [SerializeField]
        private bool FromEndToEnd;

        [Range(0, 8)] public float SpeedAnimation = 1f;

        private bool isPlaying;

        public bool ActiveAnimation = true;
        public bool ChangeObjectActive = true;

        private bool _initialized;
        private GameObject GameObject;
        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            //Initialize logic
            GameObject = gameObject;
            if (Target == null)
            {
                Debug.LogWarningFormat("{0} (in {1}) is null", "Target", name);
                enabled = false;
                return;
            }
            Target.Initialize();
            Target.EventHide += Hide;
            Target.EventShow += Show;
            EventFinishHide += delegate { };
            EventFinishShow += delegate { };
            if (Animation == null)
            {
                Debug.LogWarningFormat("{0} (in {1}) is null", "Animation", name);
                enabled = false;
                return;
            }
            if (ShowClip == null)
            {
                Debug.LogWarningFormat("{0} (in {1}) is null", "ShowClip", name);
                enabled = false;
                return;
            }
            if (HideClip == null)
            {
                Debug.LogWarningFormat("{0} (in {1}) is null", "HideClip", name);
                enabled = false;
                return;
            }
            // Animation.cullingType = AnimationCullingType.AlwaysAnimate;
        }
        void Awake()
        {
            Initialize();
        }

        private void Show()
        {
            if (!FromEndToEnd || !isPlaying)//Если FETE - false условие выполнится иначе все зависет от isPlaying, если он - false, то условие выполнится
            {
                if (ChangeObjectActive) GameObject.SetActive(true);

                if (ShowClip != null)
                {
                    SetAndPlay(ShowClip);
                    // Debug.Log("show " + name);
                }
            }
        }

        private void Hide()
        {
            if (!FromEndToEnd || !isPlaying)//Если FETE - false условие выполнится иначе все зависет от isPlaying, если он - false, то условие выполнится
            {
                if (HideClip != null)
                    SetAndPlay(HideClip);
            }
        }
        public void NormalActivate()
        {
            SetAndPlay(NormalClip);
        }


        public Appearance GetTargetAppearance()
        {
            return Target;
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            if (FromEndToEnd && isPlaying)
            {
                isPlaying = false;
                if (Target.IsShow())
                    Show();
                else
                    Hide();
            }
            else isPlaying = false;
        }

        public void OnFinishShow()
        {
            // Debug.Log("" + Target.IsShow());
            isPlaying = false;
            EventFinishShow();
            if (SendFinishMessage)
                FinishShow.Invoke();
            if (FromEndToEnd && !Target.IsShow())
            {
                Hide();
            }
            else
                NormalActivate();
        }

        public void OnFinishHide()
        {
            // Debug.Log("" + Target.IsShow());
            isPlaying = false;
            EventFinishHide();
            if (SendFinishMessage)
                FinishHide.Invoke();
            if (FromEndToEnd && Target.IsShow())
            {
                Show();
            }
            if (ChangeObjectActive)
                GameObject.SetActive(false);

        }

        private void SetAndPlay(AnimationClip clip)
        {
            if (clip == null || !ActiveAnimation) return;
            Animation.clip = clip;
            if (SpeedAnimation != 1 && clip != null) Animation[Animation.clip.name].speed = SpeedAnimation;
            Animation.Play();
            isPlaying = clip != NormalClip;
        }

        [ContextMenu("Find Appearance")]
        private void FindScreen()
        {
            Target = GetComponent<Appearance>();
        }
        [ContextMenu("Find Animation")]
        private void FindAnimation()
        {
            Animation = GetComponent<Animation>();
            if (Animation == null)
                Animation = gameObject.AddComponent<Animation>();
            Animation.playAutomatically = false;
        }
        [ContextMenu("Find All")]
        private void FindAll()
        {
            FindScreen();
            FindAnimation();
        }
    }
}