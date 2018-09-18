using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ParticlesManager : MonoBehaviour
{

    [SerializeField]
    private GameObject PrefabParticle;
    public PoolObjects<ParticleManager> ParticlesPool { private set; get; }

    [SerializeField]
    private int CountParticleSystems = 4;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (PrefabParticle == null || PrefabParticle.GetComponent<ParticleManager>() == null)
        {
            Debug.LogWarning("!!");
            enabled = false;
            return;
        }

        ParticlesPool = new PoolObjects<ParticleManager>();
        // ParticlesPool.EnqueueObject += delegate (ParticleSystem obj) { var emission = obj.emission; emission.enabled = false; };
        ParticlesPool.EnqueueObject += delegate (ParticleManager obj) { obj.EmissonActive = false; };
        GameObject gObject;
        for (int i = 0; i < CountParticleSystems; ++i)
        {
            gObject = Instantiate(PrefabParticle, PrefabParticle.transform.localPosition, PrefabParticle.transform.localRotation, transform);
            // gObject.SetActive(false);
            ParticlesPool.AddObject(gObject.GetComponent<ParticleManager>());
        }
    }

    public void WaitBeforeEnqueueParticles(float seconds, ParticleManager particle)
    {
        _seconds = seconds;
        _particle = particle;
        StartCoroutine("DoEnqueueParticles");
    }

    private float _seconds = 1;
    private ParticleManager _particle;

    private IEnumerator DoEnqueueParticles()
    {
        var particle = _particle;
        yield return new WaitForSeconds(_seconds);
        // Debug.LogFormat("LOL {0}", _seconds);
        EnqueueParticles(particle);
    }

    private void EnqueueParticles(ParticleManager particle)
    {
        if (particle != null)
            ParticlesPool.Enqueue(particle);
        // Debug.LogFormat("EnqueueParticle");
    }

}

