using UnityEngine;

public abstract class BossBehaviourBase : StateMachineBehaviour
{
    public float duration = 6f;   // quanto tempo dura antes de ir ao próximo comportamento

    [Header("Ajuste de dificuldade")]
    public float intervaloEscala = 1f;   // maior = atira mais devagar

    protected BossController boss;
    protected Transform player;
    protected float timer;
    float fireTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss == null) boss = animator.GetComponent<BossController>();
        animator.ResetTrigger("Proximo");
        timer = 0f;
        fireTimer = 0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        Tick();
        if (timer >= duration) animator.SetTrigger("Proximo");
    }

    protected abstract void Tick();

    // true uma vez a cada 'interval' segundos (multiplicado pela escala)
    protected bool Cooldown(float interval)
    {
        fireTimer += Time.deltaTime;
        if (fireTimer < interval * intervaloEscala) return false;
        fireTimer = 0f;
        return true;
    }

    // sobe e desce em volta da posição inicial
    protected void Sine(float amplitude, float frequency, float speed)
        => MoveToY(boss.HomePosition.y + Mathf.Sin(timer * frequency) * amplitude, speed);

    protected void HoldHome(float speed) => MoveToY(boss.HomePosition.y, speed);

    void MoveToY(float y, float speed)
    {
        Vector3 p = boss.transform.position;
        p.y = Mathf.MoveTowards(p.y, y, speed * Time.deltaTime);
        boss.transform.position = p;
    }

    // ângulo (graus) da bala em direção à nave
    protected float AimAngle()
    {
        if (player == null)
        {
            var pc = FindFirstObjectByType<PlayerController>();
            if (pc != null) player = pc.transform;
        }
        if (player == null) return 180f;
        Vector2 d = player.position - boss.firePoint.position;
        return Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
    }

    // leque de 'count' balas centrado em 'center', com 'step' graus entre elas
    protected void Fan(float center, int count, float step, float speed)
    {
        float start = center - step * (count - 1) / 2f;
        for (int i = 0; i < count; i++) boss.Fire(start + step * i, speed);
    }
}