// Bullet.cs
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public string targetTag = "Boss";

    Rigidbody2D rb;
    IObjectPool<Bullet> pool;
    bool released;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    public void SetPool(IObjectPool<Bullet> p) => pool = p;

    public void Launch(Vector2 position, Vector2 velocity)
    {
        released = false;
        transform.position = position;
        rb.linearVelocity = velocity; // Unity 2022 ou anterior: rb.velocity
    }

    void Update()
    {
        // Devolve à pool quando sai da tela
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        if (vp.x < -0.1f || vp.x > 1.1f || vp.y < -0.1f || vp.y > 1.1f)
            Release();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag)) return;
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(damage);
            Release();
        }
    }

    void Release()
    {
        if (released) return; // evita devolver duas vezes no mesmo frame
        released = true;
        pool.Release(this);
    }
}