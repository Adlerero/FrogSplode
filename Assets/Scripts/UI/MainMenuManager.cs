using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pressAnyKeyText; // Opcional: texto que parpadee "Press Any Key"
    
    [Header("Settings")]
    [SerializeField] private float blinkSpeed = 1f; // Velocidad del parpadeo del texto
    [SerializeField] private int gameSceneIndex = 1; // Índice de la escena del juego
    
    private bool gameStarted = false;
    
    private void Start()
    {
        // Asegurar que el tiempo esté normal (por si venimos de una pausa)
        Time.timeScale = 1f;
        
        // Si tienes texto de "Press Any Key", inicia el parpadeo
        if (pressAnyKeyText != null)
        {
            StartCoroutine(BlinkText());
        }
    }
    
    private void Update()
    {
        // Detectar cualquier input y que no se haya iniciado ya el juego
        if (!gameStarted && Input.anyKeyDown)
        {
            // Filtrar teclas que no queremos que inicien el juego (opcional)
            if (!Input.GetKeyDown(KeyCode.LeftAlt) && 
                !Input.GetKeyDown(KeyCode.RightAlt) &&
                !Input.GetKeyDown(KeyCode.LeftControl) &&
                !Input.GetKeyDown(KeyCode.RightControl))
            {
                StartGame();
            }
        }
    }
    
    private void StartGame()
    {
        gameStarted = true;
        
        // Opcional: Detener el parpadeo
        if (pressAnyKeyText != null)
        {
            StopAllCoroutines();
            pressAnyKeyText.SetActive(true); // Asegurarse de que esté visible
        }
        
        // Cargar la escena del juego
        SceneManager.LoadScene(gameSceneIndex);
    }
    
    // Corrutina para hacer parpadear el texto "Press Any Key" (opcional)
    private System.Collections.IEnumerator BlinkText()
    {
        while (true)
        {
            if (pressAnyKeyText != null)
            {
                pressAnyKeyText.SetActive(!pressAnyKeyText.activeInHierarchy);
            }
            yield return new WaitForSeconds(1f / blinkSpeed);
        }
    }
    
}