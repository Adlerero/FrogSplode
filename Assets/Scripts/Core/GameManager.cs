using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Stats")]
    public int BananasTot = 0;
    public int KeysTot = 0;
    
    [Header("References")]
    [SerializeField] private GameObject key;
    public GameObject DoorReference;
    
    // Game State (no se puede usar [Header] en propiedades)
    public bool HasKey { get; private set; } = false;
    
    // Sistema de tracking de coleccionables
    private HashSet<string> collectedItems = new HashSet<string>();
    // Sistema de tracking de llaves (agregar junto a collectedItems)
    private HashSet<string> collectedKeys = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Hay más de un GameManager en la escena");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (key != null)
            key.SetActive(false); // Asegúrate de que la llave esté oculta al inicio
    }

    public void CounterBananas(int bananas)
    {
        BananasTot += bananas;
        if (BananasTot >= 3)
        {
            if (key != null)
                key.SetActive(true);
        }
        Debug.Log($"Bananas totales: {BananasTot}");
    }
    
    public void PickUpKey()
    {
        HasKey = true;
        KeysTot++;
        if (DoorReference != null)
            DoorReference.GetComponent<Door>().UnlockDoor();
        Debug.Log("Llave agregada al inventario. Total llaves: " + KeysTot);
    }

    // ===== MÉTODOS NUEVOS PARA SISTEMA DE COLECCIONABLES =====

// Marca una llave como recogida

    public void MarkKeyAsCollected(string keyID)
    {
        if (!string.IsNullOrEmpty(keyID) && !collectedKeys.Contains(keyID))
        {
            collectedKeys.Add(keyID);
            Debug.Log($"Llave marcada como recogida: {keyID}");
        }
    }

    public bool IsKeyCollected(string keyID)
    {
        if (string.IsNullOrEmpty(keyID))
            return false;
            
        return collectedKeys.Contains(keyID);
    }

    /// <summary>
    /// Marca un coleccionable como recogido
    /// </summary>
    /// <param name="collectibleID">ID único del coleccionable</param>
    public void MarkCollectibleAsCollected(string collectibleID)
    {
        if (!string.IsNullOrEmpty(collectibleID) && !collectedItems.Contains(collectibleID))
        {
            collectedItems.Add(collectibleID);
            Debug.Log($"Coleccionable marcado como recogido: {collectibleID}");
        }
    }

    /// <summary>
    /// Verifica si un coleccionable ya fue recogido
    /// </summary>
    /// <param name="collectibleID">ID único del coleccionable</param>
    /// <returns>True si ya fue recogido, false en caso contrario</returns>
    public bool IsCollectibleCollected(string collectibleID)
    {
        if (string.IsNullOrEmpty(collectibleID))
            return false;
            
        return collectedItems.Contains(collectibleID);
    }

    /// <summary>
    /// Obtiene el conteo actual de bananas
    /// </summary>
    /// <returns>Número total de bananas recogidas</returns>
    public int GetBananaCount()
    {
        return BananasTot;
    }

    /// <summary>
    /// Resetea todos los coleccionables y el progreso del juego
    /// </summary>
    public void ResetGame()
    {
        collectedItems.Clear();
        collectedKeys.Clear(); // AGREGAR esta línea
        BananasTot = 0;
        KeysTot = 0;
        HasKey = false;
        
        if (key != null)
            key.SetActive(false);
            
        Debug.Log("Juego reseteado");
    }

    /// <summary>
    /// Obtiene la lista de coleccionables recogidos (para debugging)
    /// </summary>
    /// <returns>HashSet con los IDs de coleccionables recogidos</returns>
    public HashSet<string> GetCollectedItems()
    {
        return new HashSet<string>(collectedItems);
    }

    /// <summary>
    /// Obtiene información de debug sobre el estado del juego
    /// </summary>
    public void PrintGameState()
    {
        Debug.Log($"=== ESTADO DEL JUEGO ===");
        Debug.Log($"Bananas: {BananasTot}");
        Debug.Log($"Llaves: {KeysTot}");
        Debug.Log($"Tiene llave: {HasKey}");
        Debug.Log($"Coleccionables recogidos: {collectedItems.Count}");
        foreach (string item in collectedItems)
        {
            Debug.Log($"- {item}");
        }
        Debug.Log($"========================");
    }
}