using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour
{
    // Agregar esta variable al inicio de la clase:
    [SerializeField] private string keyID;
    [SerializeField] private Door doorToUnlock;
    [SerializeField] private int bananasRequired = 3;
    [SerializeField] private AudioClip keySound;

    private bool isCollected = false;
    private int currentBananas = 0;

    private void Awake()
    {
        // Generar ID único si no está asignado
        if (string.IsNullOrEmpty(keyID))
        {
            keyID = GenerateUniqueKeyID();
        }

        // Verificar si esta llave ya fue recogida
        if (GameManager.Instance.IsKeyCollected(keyID))
        {
            isCollected = true;
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(false); // Ocultar por defecto
    }
    // AGREGAR estos métodos nuevos:
    private string GenerateUniqueKeyID()
    {
        string sceneName = gameObject.scene.name;
        Vector3 pos = transform.position;
        return $"Key_{sceneName}_{gameObject.name}_{pos.x:F1}_{pos.y:F1}";
    }

    public string GetKeyID()
    {
        return keyID;
    }


    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
    public void AddBanana()
    {
        currentBananas++;
        if (currentBananas >= bananasRequired)
        {
            SetVisible(true);
        }
    }
    public int GetBananasRequired()
    {
        return bananasRequired;
    }

    public int GetCurrentBananas()
    {
        return currentBananas;
    }

    public void ResetBananaCount()
    {
        currentBananas = 0;
        isCollected = false;
        SetVisible(false);
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || isCollected) return;

        SoundManager.instance.PlaySound(keySound);
        isCollected = true;
        
        // Marcar la llave como recogida en el GameManager
        GameManager.Instance.MarkKeyAsCollected(keyID);

        doorToUnlock.UnlockDoor();
        GameManager.Instance.PickUpKey(); // Marca que tienes la llave
        gameObject.SetActive(false);    // Desactivar el coleccionable
        Debug.Log("Llave tomada, ahora puedes usar la puerta");

    }
}
