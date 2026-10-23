// BossMedia1Senoidal.cs
public class BossMedia1Senoidal : BossBehaviourBase
{
    protected override void Tick()
    {
        Sine(3.2f, 2f, 12f);
        if (Cooldown(0.5f)) boss.Fire(180f, 7f);
    }
}