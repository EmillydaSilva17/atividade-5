// BossAlta2Leque.cs
public class BossAlta2Leque : BossBehaviourBase
{
    protected override void Tick()
    {
        HoldHome(6f);
        if (Cooldown(1.4f)) Fan(180f, 3, 20f, 5.5f);
    }
}