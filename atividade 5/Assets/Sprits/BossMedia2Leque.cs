// BossMedia2Leque.cs
public class BossMedia2Leque : BossBehaviourBase
{
    protected override void Tick()
    {
        HoldHome(6f);
        if (Cooldown(1.1f)) Fan(180f, 5, 15f, 6.5f);
    }
}