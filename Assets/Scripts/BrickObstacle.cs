using System.Collections;
using UnityEngine;
using System.Collections; // Importante: Necesitas esto para usar Coroutines

public class BrickObstacle : MonoBehaviour
{
    public float delayTime = 10f;
    public GameManager gameManager;
    public void StartDelayAction()
    {
        StartCoroutine(WaitAndExecute());
    }

    private IEnumerator WaitAndExecute()
    {
        Debug.Log("Iniciando la espera. El juego NO se congelará.");

        yield return new WaitForSeconds(delayTime);
        Debug.Log("¡10 segundos han pasado! Ejecutando la acción después del delay.");

        ExecutePostDelayAction();
    }

    private void ExecutePostDelayAction()
    {
        Debug.Log("TERMINO EL DELAY DEL BRICK");
        gameObject.SetActive(false);
        gameManager.MoveEnemies();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartDelayAction();
        }
    }
}
