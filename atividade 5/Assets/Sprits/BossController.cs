using System;
using UnityEngine;
using UnityEngine.Pool;

public class BossController : MonoBehaviour, IDamageable
{
    [Header("Vida")]
    public int maxHealth = 300;

    [Header("Tiro")]
    public Bullet bulletPrefab;
    public Transform firePoint;

    public int CurrentHealth { get; private set; }
    public int Phase { get; private set; }          // 0 = alta, 1 = média, 2 = baixa
    public bool Invulnerable { get; private set; }

    public event Action<float, int> OnHealthChanged; // (percentual 0-1, fase)
    public event Action OnDefeated;

    Animator animator;
    IObjectPool<Bullet> pool;
    bool defeated;

    void Awake()
    {
        animator = GetComponent<Animator>();
        CurrentHealth = maxHealth;

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
            defaultCapacity: 60,
            maxSize: 300);
    }

    void Start()
    {
        OnHealthChanged?.Invoke(1f, Phase);
        UpdateAnimator();
    }

    public void TakeDamage(int amount)
    {
        if (defeated || Invulnerable) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        float percent = (float)CurrentHealth / maxHealth;

        if (CurrentHealth <= 0)
        {
            defeated = true;
            OnHealthChanged?.Invoke(0f, Phase);
            OnDefeated?.Invoke();
            gameObject.SetActive(false);
            return;
        }

        int newPhase = percent > 0.5f ? 0 : (percent > 0.2f ? 1 : 2);
        if (newPhase != Phase)
        {
            Phase = newPhase;
            Invulnerable = true; // será desligado pelo estado de transição (etapa 3)
            if (animator) animator.SetTrigger("Transicao");
        }

        OnHealthChanged?.Invoke(percent, Phase);
        UpdateAnimator();
    }

    // Chamado pelos StateMachineBehaviours
    public void SetInvulnerable(bool value) => Invulnerable = value;

    // Dispara uma bala. angleDeg: 180 = esquerda (direto na nave), varie para leque/espiral
    public void Fire(float angleDeg, float speed)
    {
        var dir = new Vector2(Mathf.Cos(angleDeg * Mathf.Deg2Rad), Mathf.Sin(angleDeg * Mathf.Deg2Rad));
        var b = pool.Get();
        b.Launch(firePoint.position, dir * speed);
    }

    void UpdateAnimator()
    {
        if (!animator) return;
        animator.SetFloat("vidaPercent", (float)CurrentHealth / maxHealth);
        animator.SetInteger("fase", Phase);
    }
}