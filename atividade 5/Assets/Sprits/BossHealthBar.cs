using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public BossController boss;
    public Image fill;
    public Color highColor = Color.green;
    public Color midColor = Color.yellow;
    public Color lowColor = Color.red;

    void OnEnable()  => boss.OnHealthChanged += Refresh;
    void OnDisable() => boss.OnHealthChanged -= Refresh;

    void Refresh(float percent, int phase)
    {
        fill.fillAmount = percent;
        fill.color = phase == 0 ? highColor : (phase == 1 ? midColor : lowColor);
    }
}