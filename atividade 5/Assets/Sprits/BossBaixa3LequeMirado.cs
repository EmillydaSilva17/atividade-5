// BossBaixa3LequeMirado.cs
public class BossBaixa3LequeMirado : BossBehaviourBase
{
    protected override void Tick()
    {
        Sine(2.5f, 1.5f, 10f);
        if (Cooldown(0.6f)) Fan(AimAngle(), 5, 15f, 7.5f);
    }
}
