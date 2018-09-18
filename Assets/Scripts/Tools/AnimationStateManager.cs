using UnityEngine;

public class AnimationStateManager : MonoBehaviour
{
    [SerializeField]
    Animation Animation;

    [ContextMenu("Find Animation")]
    private void FindAnimation()
    {
        Animation = GetComponent<Animation>();
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Animation == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Animation", name);
            enabled = false;
            return;
        }

    }
    void Awake()
    {
        Initialize();
    }

    // public void Pause()
    // {
    //     if (Animation != null)
    //         // Animation.
    // }

    public void Play()
    {
        if (Animation != null)
            Animation.Play();
    }

    public void SetSpeed(float speed)
    {
        if (Animation != null)
            Animation[Animation.clip.name].speed = speed;
    }

    public void Stop()
    {
        if (Animation != null)
            Animation.Stop();
    }

    public void Set(AnimationClip clip)
    {
        if (Animation != null)
            Animation.clip = clip;
    }

    public void SetAndPlay(AnimationClip clip)
    {
        Set(clip);
        Play();
    }

    public void DisactiveObject()
    {
        gameObject.SetActive(false);
    }
}