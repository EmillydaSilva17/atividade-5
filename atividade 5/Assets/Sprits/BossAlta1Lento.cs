// BossAlta1Lento.cs
public class BossAlta1Lento : BossBehaviourBase
{
    protected override void Tick()
    {
        Sine(3f, 1f, 6f);
        if (Cooldown(0.9f)) boss.Fire(180f, 5f);
    }
}