// BossBaixa2Espiral.cs
using UnityEngine;

public class BossBaixa2Espiral : BossBehaviourBase
{
    protected override void Tick()
    {
        HoldHome(6f);
        if (Cooldown(0.09f)) boss.Fire(180f + 70f * Mathf.Sin(timer * 4f), 6f);
    }
}