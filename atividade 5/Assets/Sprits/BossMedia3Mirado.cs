// BossMedia3Mirado.cs
public class BossMedia3Mirado : BossBehaviourBase
{
    protected override void Tick()
    {
        Sine(2.5f, 1.2f, 8f);
        if (Cooldown(0.8f)) boss.Fire(AimAngle(), 8f);
    }
}