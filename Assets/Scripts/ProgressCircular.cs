using System;
using System.Collections;
using UnityEngine;

public class ProgressCircular : MonoBehaviour
{
    // --- Configuración de Instanciado ---
    [Header("Instanciado y Posición")]
    public GameObject cubePrefab; // El cubo a instanciar
    public int numberOfCubos = 12; // Cantidad de cubos
    public float radius = 5f; // Distancia del centro
    public Transform centerPosition; // Posición central del círculo
    public GameObject tower;
    public GameObject auxTower;

    // --- Configuración de Materiales ---
    [Header("Materiales")]
    public Material whiteMaterial; // El material inicial (blanco)
    public Material greenMaterial; // El material de relleno (verde)

    // --- Referencia a Cubos ---
    // Array para guardar las referencias a los cubos instanciados
    private GameObject[] instantiatedCubes;
    public ManagerEnemies managerEnemies;
    public GameManager gameManager;

    // --- Métodos de Inicio ---
    void Start()
    {
        // 1. Instancia los cubos
        SpawnCubesInCircle();
        
        // 2. Opcional: Inicia el relleno verde 2 segundos después de empezar
        // La duración total del relleno es de 5.0 segundos.
        //Invoke("StartFill", 2.0f); 
        //StartGreenFill(2.0f);
        StartFill();
    }

    void StartFill()
    {
        // Llama a la función para iniciar el relleno en 5.0 segundos
        StartGreenFill(2.0f);
    }

    // --- Función de Instanciado Circular ---
    void SpawnCubesInCircle()
    {
        // Inicializa el array para almacenar los cubos
        instantiatedCubes = new GameObject[numberOfCubos];

        float angleStep = 360f / numberOfCubos;

        for (int i = 0; i < numberOfCubos; i++)
        {
            float angle = i * angleStep;
            float angleRad = angle * Mathf.Deg2Rad;

            // Cálculo de Posición (X y Z en el plano)
            float x = radius * Mathf.Cos(angleRad);
            float z = radius * Mathf.Sin(angleRad);
            Vector3 spawnPosition = centerPosition.position + new Vector3(x, 0f, z);

            // Cálculo de Rotación (Para mirar al centro)
            Vector3 directionToCenter = centerPosition.position - spawnPosition;
            Quaternion lookAtCenterRotation = Quaternion.LookRotation(directionToCenter);

            // Instancia el cubo
            GameObject newCube = Instantiate(cubePrefab, spawnPosition, lookAtCenterRotation);
            newCube.transform.parent = transform;
            
            // Asigna el material inicial y guarda la referencia
            Renderer cubeRenderer = newCube.GetComponent<Renderer>();
            
            // **IMPORTANTE:** Asegúrate de que el cubo tenga el componente Renderer.
            if (cubeRenderer != null && whiteMaterial != null)
            {
                cubeRenderer.material = whiteMaterial;
            }
            
            instantiatedCubes[i] = newCube; // Guarda la referencia
        }
    }

    // --- Función de Relleno Verde Cronometrado ---
    
    // Función pública que se llama para iniciar la corrutina
    public void StartGreenFill(float duration)
    {
        if (instantiatedCubes == null || instantiatedCubes.Length == 0)
        {
            Debug.LogWarning("No hay cubos instanciados para rellenar. Llama a SpawnCubesInCircle primero.");
            return;
        }
        
        StartCoroutine(FillCubesWithGreen(duration));
    }

    // La corrutina que realiza el cambio secuencial con pausas
    private IEnumerator FillCubesWithGreen(float duration)
    {
        if (numberOfCubos <= 0) yield break; // Evita división por cero
        
        // Calcula la pausa necesaria entre cada cambio
        float delayPerCube = duration / numberOfCubos;

        //Debug.Log($"Iniciando relleno verde. Duración total: {duration}s. Pausa por cubo: {delayPerCube:F3}s.");

        // Itera sobre el array de cubos
        for (int i = 0; i < instantiatedCubes.Length; i++)
        {
            GameObject currentCube = instantiatedCubes[i];

            if (currentCube != null && greenMaterial != null)
            {
                // Cambia el material al verde
                Renderer renderer = currentCube.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = greenMaterial;
                }
            }

            // Espera el tiempo calculado antes de cambiar el siguiente
            yield return new WaitForSeconds(delayPerCube);
        }

        if (auxTower == null)
        {
            auxTower = Instantiate(tower, this.gameObject.transform);
            auxTower.GetComponent<Tower>().gameManager = gameManager;
            auxTower.GetComponent<Tower>().managerEnemies = managerEnemies;
        }

        GetComponent<CapsuleCollider>().enabled = false;
        //Debug.Log("¡Relleno verde completado!");
    }
}