using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [SerializeField] private int bananaValue = 1;
    [SerializeField] private Keys keyToActivate;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private string collectibleID;

    private bool isCollected = false;

    private void Start()
    {
        // Generar ID único si no está asignado
        if (string.IsNullOrEmpty(collectibleID))
        {
            collectibleID = GenerateUniqueID();
        }

        // Verificar si este coleccionable ya fue recogido
        if (GameManager.Instance.IsCollectibleCollected(collectibleID))
        {
            isCollected = true;
            gameObject.SetActive(false);
        }
    }

    public void SetKeyToActivate(Keys key)
    {
        keyToActivate = key;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        if (isCollected) return; // Evitar doble recolección

        isCollected = true;
        
        // Reproducir sonido
        if (coinSound != null)
            SoundManager.instance.PlaySound(coinSound);
        
        // Agregar al contador del GameManager
        GameManager.Instance.CounterBananas(bananaValue);
        
        // Marcar como recogido en el GameManager
        GameManager.Instance.MarkCollectibleAsCollected(collectibleID);
        
        // Notificar a la llave específica
        if (keyToActivate != null)
        {
            keyToActivate.AddBanana();
        }

        // Desactivar el coleccionable
        gameObject.SetActive(false);
        
        Debug.Log($"Coleccionable {collectibleID} recogido. Total bananas: {GameManager.Instance.GetBananaCount()}");
    }

    private string GenerateUniqueID()
    {
        // Generar un ID único basado en la posición y el nombre del objeto
        string sceneName = gameObject.scene.name;
        Vector3 pos = transform.position;
        return $"{sceneName}_{gameObject.name}_{pos.x:F1}_{pos.y:F1}_{pos.z:F1}";
    }

    // Método público para obtener el ID (útil para debugging)
    public string GetCollectibleID()
    {
        return collectibleID;
    }

    // Método público para resetear el estado (útil para testing)
    public void ResetCollectible()
    {
        isCollected = false;
        gameObject.SetActive(true);
    }

    // Método para verificar si fue recogido
    public bool IsCollected()
    {
        return isCollected;
    }
}