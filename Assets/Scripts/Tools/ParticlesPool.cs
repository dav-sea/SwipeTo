using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesPool : MonoBehaviour
{

    [SerializeField]
    private GameObject PrefabParticles;

    [SerializeField]
    private int CountParticles = 4;


    PoolObjects<ParticleManager> Particles;


    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (PrefabParticles == null || PrefabParticles.GetComponent<ParticleManager>() == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null or {0} not have ParticleManager", "PrefabParticles", name);
            enabled = false;
            return;
        }
        Particles = new PoolObjects<ParticleManager>();
        Particles.EnqueueObject += delegate (ParticleManager ps) { ps.EmissonActive = false; };
        Fill(CountParticles);
    }

    private void Fill(int count)
    {
        for (int i = 0; i < count; ++i)
            Particles.AddObject(Create());
    }

    private ParticleManager Create()
    {
        return Instantiate(PrefabParticles, Vector3.zero, Quaternion.identity, transform).GetComponent<ParticleManager>();
    }

    public void Enqueue(ParticleManager obj)
    {
        Particles.Enqueue(obj);
    }

    public ParticleManager Dequeue()
    {
        return Particles.Dequeue();
    }

    void Awake()
    {
        Initialize();
    }



}
