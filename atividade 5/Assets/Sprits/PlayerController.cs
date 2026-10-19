// PlayerController.cs
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Movimento")]
    public float speed = 6f;
    [Range(0.1f, 1f)] public float maxViewportX = 0.45f; // não deixa invadir o chefão

    [Header("Tiro")]
    public Bullet bulletPrefab;
    public Transform firePoint;
    public float fireInterval = 0.25f;   // entre 0,2 s e 1 s
    public float bulletSpeed = 12f;

    [Header("Vida")]
    public int extraLives = 3;           // 3 danos suportados, o 4º mata
    public float invulnerableTime = 1f;

    public event Action<int> OnLivesChanged;
    public event Action OnDied;

    IObjectPool<Bullet> pool;
    float nextFire;
    bool invulnerable, dead;
    Vector2 min, max;
    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        pool = new ObjectPool<Bullet>(
            createFunc: () =>
            {
                var b = Instantiate(bulletPrefab);
                b.SetPool(pool);
                return b;
            },
            actionOnGet: b => b.gameObject.SetActive(true),
            actionOnRelease: b => b.gameObject.SetActive(false),
            actionOnDestroy: b => Destroy(b.gameObject),
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 60);
    }

    void Start()
    {
        var cam = Camera.main;
        min = cam.ViewportToWorldPoint(new Vector3(0.03f, 0.05f));
        max = cam.ViewportToWorldPoint(new Vector3(maxViewportX, 0.95f));
        OnLivesChanged?.Invoke(extraLives);
    }

    void Update()
    {
        if (dead) return;
        Move();

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            nextFire = Time.time + fireInterval;
        }
        else if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            Shoot();
            nextFire = Time.time + fireInterval;
        }
    }

    void Move()
    {
        var dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (dir.sqrMagnitude > 1f) dir.Normalize();

        Vector2 pos = (Vector2)transform.position + dir * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        transform.position = pos;
    }

    void Shoot()
    {
        var b = pool.Get();
        b.Launch(firePoint.position, Vector2.right * bulletSpeed);
    }

    public void TakeDamage(int amount)
    {
        if (dead || invulnerable) return;

        if (extraLives <= 0)
        {
            dead = true;
            OnDied?.Invoke();
            gameObject.SetActive(false);
            return;
        }

        extraLives--;
        OnLivesChanged?.Invoke(extraLives);
        StartCoroutine(InvulnerabilityRoutine());
    }

    IEnumerator InvulnerabilityRoutine()
    {
        invulnerable = true;
        float t = 0f;
        while (t < invulnerableTime)
        {
            sprite.enabled = !sprite.enabled; // pisca
            yield return new WaitForSeconds(0.1f);
            t += 0.1f;
        }
        sprite.enabled = true;
        invulnerable = false;
    }
}