// LivesUI.cs
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public PlayerController player;
    public Image[] icons;

    void OnEnable()  => player.OnLivesChanged += Refresh;
    void OnDisable() => player.OnLivesChanged -= Refresh;

    void Refresh(int extraLives)
    {
        for (int i = 0; i < icons.Length; i++)
            icons[i].enabled = i < extraLives;
    }
}