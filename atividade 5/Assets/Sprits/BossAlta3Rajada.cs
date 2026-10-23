// BossAlta3Rajada.cs
public class BossAlta3Rajada : BossBehaviourBase
{
    protected override void Tick()
    {
        HoldHome(6f);
        if (Cooldown(0.25f)) boss.Fire(180f, 7f);
    }
}