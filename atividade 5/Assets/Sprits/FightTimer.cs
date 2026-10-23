using UnityEngine;

public class FightTimer : MonoBehaviour
{
    public BossController boss;

    float startTime;

    void OnEnable()  => boss.OnDefeated += Report;
    void OnDisable() => boss.OnDefeated -= Report;

    void Start() => startTime = Time.time;

    void Report()
    {
        Debug.Log($"A luta durou {Time.time - startTime:F1} segundos");
    }
}