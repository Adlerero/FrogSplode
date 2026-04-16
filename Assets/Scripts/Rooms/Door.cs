using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private bool isFinalDoor = false;
    [SerializeField] private GameObject winScreen;
    private bool isUnlocked = false;

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();
    }
    private void Start()
    {
        // Si el jugador aún no tiene la llave, la puerta debe bloquearlo físicamente
        if (!GameManager.Instance.HasKey && doorCollider != null)
            doorCollider.isTrigger = false;
        else if (GameManager.Instance.HasKey && doorCollider != null)
            doorCollider.isTrigger = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!GameManager.Instance.HasKey)
            {
                Debug.Log("¡No tienes la llave para entrar!");
                return;
            }

            if (isFinalDoor)
            {
                WinGame();
                return;
            }


            if (collision.transform.position.x < transform.position.x)
                {
                    cam.MoveToNewRoom(nextRoom);
                    nextRoom.GetComponent<Room>().ActivateRoom(true);
                    previousRoom.GetComponent<Room>().ActivateRoom(false);
                }
                else
                {
                    cam.MoveToNewRoom(previousRoom);
                    previousRoom.GetComponent<Room>().ActivateRoom(true);
                    nextRoom.GetComponent<Room>().ActivateRoom(false);
                }
        }
    }

    public void UnlockDoor()
    {
        isUnlocked = true;
        if (doorCollider != null)
            doorCollider.isTrigger = true;
    }   
    
    private void WinGame()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("¡Has ganado el juego!");
        }
        else
        {
            Debug.LogWarning("No se asignó el GameObject de la pantalla de victoria.");
        }
    }

    
}