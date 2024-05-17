using UnityEngine;
using UnityEngine.Pool;

public class Puff : MonoBehaviour
{
    private IObjectPool<Puff> managedPool;
    public ParticleSystem Pts;

    private void OnEnable()
    {
        Pts = GetComponent<ParticleSystem>();
        Pts.Play();
        Enable();
    }

    public void SetManagedPool(IObjectPool<Puff> pool)
    {
        managedPool = pool;
    }

    public void Enable()
    {
        Invoke("DestroyPuff", 1f);
    }

    public void DestroyPuff()
    {
        managedPool.Release(this);
    }
}
