using UnityEngine;
using UnityEngine.SceneManagement;

public class BootManager : MonoBehaviour
{
    [Header("Configuração de Cenas")]
    [SerializeField] private string nomeDaProximaFase = "Fase";

    private void Start()
    {
        // Como o BootManager e o SaveSystem estão na mesma cena _Boot,
        // vamos fazer o objeto do BootManager persistir e carregar a fase.
        DontDestroyOnLoad(gameObject);

        // Carrega a cena da fase automaticamente
        SceneManager.LoadScene(nomeDaProximaFase);
    }
}