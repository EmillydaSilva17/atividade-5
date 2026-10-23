// BossBaixa1Rapido.cs
public class BossBaixa1Rapido : BossBehaviourBase
{
    protected override void Tick()
    {
        Sine(3.3f, 3f, 14f);
        if (Cooldown(0.25f)) boss.Fire(180f, 9f);
    }
}