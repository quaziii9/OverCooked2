using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Puff : MonoBehaviour
{
    private IObjectPool<Puff> _ManagedPool;
    private Vector3 _Direction;
    public ParticleSystem Pts;

    private void OnEnable()
    {
        Pts = GetComponent<ParticleSystem>();
        Pts.Play();
        Enable();
    }
    public void SetManagedPool(IObjectPool<Puff> pool)
    {
        _ManagedPool = pool;
    }

    public void Enable()
    {Invoke("DestroyPuff", 1f);
    }

    public void DestroyPuff()
    {
        _ManagedPool.Release(this);
    }
}
