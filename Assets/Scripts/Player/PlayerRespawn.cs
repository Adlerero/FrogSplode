using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        if (currentCheckpoint == null)
        {
            uiManager.GameOver();
            return;
        }

        playerHealth.Respawn(); // Restaurar salud del jugador y resetear animación
        transform.position = currentCheckpoint.position; // Mover jugador a la posición del checkpoint

        // Mover la cámara a la habitación del checkpoint
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
        
        // Activar la habitación del checkpoint pero respetando el estado actual del juego
        Room checkpointRoom = currentCheckpoint.parent.GetComponent<Room>();
        if (checkpointRoom != null)
        {
            // Usar ActivateRoom normal en lugar de ActivateRoomAfterRespawn
            // para que respete el estado actual de coleccionables y llaves
            checkpointRoom.ActivateRoom(true);
            
            // Actualizar el estado de la habitación basado en el GameManager
            checkpointRoom.UpdateRoomStateAfterRespawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("Appear");
            
            // Opcional: Guardar el estado actual del juego cuando se activa un checkpoint
            // Esto asegura que el progreso se mantenga al respawnear
            SaveCurrentGameState();
        }
    }
    
    private void SaveCurrentGameState()
    {
        // Aquí puedes agregar lógica adicional para guardar el estado del juego
        // si el GameManager no lo hace automáticamente
        Debug.Log($"Checkpoint activado. Estado actual - Tiene llave: {GameManager.Instance.HasKey}");
    }
}