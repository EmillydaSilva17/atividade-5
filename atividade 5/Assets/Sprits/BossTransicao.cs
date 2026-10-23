// BossTransicao.cs
using UnityEngine;

public class BossTransicao : StateMachineBehaviour
{
    public float duracao = 2f;

    BossController boss;
    SpriteRenderer sprite;
    Color original;
    float timer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss == null)
        {
            boss = animator.GetComponent<BossController>();
            sprite = animator.GetComponent<SpriteRenderer>();
        }
        original = sprite.color;
        timer = 0f;
        animator.ResetTrigger("Proximo");
        boss.SetInvulnerable(true);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        sprite.color = Color.Lerp(original, Color.white, Mathf.PingPong(timer * 6f, 1f)); // pisca
        if (timer >= duracao) animator.SetTrigger("Proximo");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sprite.color = original;
        boss.SetInvulnerable(false);
    }
}