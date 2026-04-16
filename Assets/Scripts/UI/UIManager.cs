using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;
    
    [Header("Win")]
    [SerializeField] private GameObject winScreen;

    private void Awake()
    {
        // Asegurarse de que todas las pantallas estén desactivadas al inicio
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        
        if (pauseScreen != null)
            pauseScreen.SetActive(false);
        
        if (winScreen != null)
            winScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Si la pantalla de pausa ya está activa, despausar y viceversa
            PauseGame(!pauseScreen.activeInHierarchy);
        }
    }

    #region Game Over
    // Activar pantalla de game over
    public void GameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            Time.timeScale = 0f; // Pausar el juego cuando aparece game over
        }
    }

    // Reiniciar nivel
    public void Restart()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo vuelva a la normalidad
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Menú principal
    public void MainMenu()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo vuelva a la normalidad
        SceneManager.LoadScene(0);
    }

    // Salir del juego
    public void Quit()
    {
        Application.Quit(); // Salir del juego (solo funciona en build)

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Salir del modo de juego (solo en editor)
#endif
    }
    #endregion

    #region Win
    // Activar pantalla de victoria
    public void Win()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f; // Pausar el juego cuando aparece la pantalla de victoria
        }
    }
    #endregion

    #region Pause
    public void PauseGame(bool status)
    {
        Debug.Log($"PauseGame llamado con status: {status}"); // Debug para verificar si se ejecuta
        
        // Si status == true pausar | si status == false despausar
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(status);
        }

        // Cuando pause status es true cambiar timescale a 0 (el tiempo se detiene)
        // cuando es false cambiarlo de vuelta a 1 (el tiempo pasa normalmente)
        if (status)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    // Método público para pausar desde UI buttons
    public void PauseButton()
    {
        PauseGame(true);
    }

    // Método público para despausar desde UI buttons
    public void UnpauseButton()
    {
        PauseGame(false);
    }

    // Método público para alternar pausa desde UI buttons
    public void TogglePause()
    {
        PauseGame(!pauseScreen.activeInHierarchy);
    }
    #endregion
}