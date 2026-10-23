using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Personagens")]
    public PlayerController player;
    public BossController boss;

    [Header("Telas")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    [Header("Botões")]
    public Button victoryRetryButton;
    public Button defeatRetryButton;

    bool ended;

    void Awake()
    {
        Time.timeScale = 1f;
    }

    void OnEnable()
    {
        player.OnDied += HandlePlayerDied;
        boss.OnDefeated += HandleBossDefeated;

        if (victoryRetryButton) victoryRetryButton.onClick.AddListener(Retry);
        if (defeatRetryButton) defeatRetryButton.onClick.AddListener(Retry);
    }

    void OnDisable()
    {
        player.OnDied -= HandlePlayerDied;
        boss.OnDefeated -= HandleBossDefeated;

        if (victoryRetryButton) victoryRetryButton.onClick.RemoveListener(Retry);
        if (defeatRetryButton) defeatRetryButton.onClick.RemoveListener(Retry);
    }

    void HandleBossDefeated()
    {
        if (ended) return;
        ended = true;

        player.enabled = false;          // impede de atirar e mover durante a tela de vitória
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void HandlePlayerDied()
    {
        if (ended) return;
        ended = true;

        defeatPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}