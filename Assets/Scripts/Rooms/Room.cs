using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private GameObject[] enemies;
    private Vector3[] initialPosition;

    [Header("Collectibles System")]
    [SerializeField] private Collectibles[] roomCollectibles;
    [SerializeField] private Keys roomKey;

    private void Awake()
    {
        //Save the initial positions of the enemies
        initialPosition = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                initialPosition[i] = enemies[i].transform.position;
        }

        // Conectar automáticamente los coleccionables con la llave del cuarto
        ConnectCollectiblesWithKey();
    }

    private void ConnectCollectiblesWithKey()
    {
        if (roomKey != null && roomCollectibles != null)
        {
            foreach (Collectibles collectible in roomCollectibles)
            {
                if (collectible != null)
                {
                    collectible.SetKeyToActivate(roomKey);
                }
            }
        }
    }

    public void ActivateRoom(bool _status)
    {
        //Activate/deactivate enemies
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status);
                enemies[i].transform.position = initialPosition[i];

                if (_status)
                {
                    Spikehead spikehead = enemies[i].GetComponent<Spikehead>();
                    if (spikehead != null)
                    {
                        spikehead.ResetSpikeHead();
                    }
                }

            }


        }

        // Resetear el contador de la llave cuando se activa el cuarto
        /*
        if (_status && roomKey != null)
        {
            roomKey.ResetBananaCount();
        }
        */
    }

    // Método específico para respawn que resetea coleccionables
    public void ActivateRoomAfterRespawn(bool _status)
    {
        // Primero hacer la activación normal
        ActivateRoom(_status);

        // Solo resetear coleccionables si se está activando después de respawn
        if (_status)
        {
            // Resetear llave
            if (roomKey != null)
            {
                roomKey.ResetBananaCount();
            }

            // Reactivar todos los coleccionables del cuarto
            if (roomCollectibles != null)
            {
                foreach (Collectibles collectible in roomCollectibles)
                {
                    if (collectible != null)
                    {
                        collectible.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    public void UpdateRoomStateAfterRespawn()
    {
        // Si el jugador ya tiene la llave, ocultar la llave de esta habitación
        if (GameManager.Instance.HasKey && roomKey != null)
        {
            roomKey.gameObject.SetActive(false);
        }
        
        // Verificar cada coleccionable del cuarto si ya fue recogido
        if (roomCollectibles != null)
        {
            foreach (Collectibles collectible in roomCollectibles)
            {
                if (collectible != null)
                {
                    // Verificar si este coleccionable específico ya fue recogido
                    string collectibleID = collectible.GetCollectibleID();
                    if (GameManager.Instance.IsCollectibleCollected(collectibleID))
                    {
                        collectible.gameObject.SetActive(false);
                    }
                }
            }
        }
        
        Debug.Log($"Estado de habitación actualizado después de respawn. Jugador tiene llave: {GameManager.Instance.HasKey}");
    }
}