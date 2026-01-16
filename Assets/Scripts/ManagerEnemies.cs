using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ManagerEnemies : MonoBehaviour
{
    public GameObject enemyA;
    public GameObject enemyBossA;
    private float tiempoProximoCreacion = 0.1f;
    private float tiempoProximoCreacionBoss = 10f;
    private float tiempoEntreEnemies = 0.1f;
    private float tiempoEntreEnemiesBoss = 10f;
    public Transform positionEnemy;
    public Player player;
    public List<GameObject> enemies;
    public bool isCreationActive;
    public Hud hud;
    public Configuration configuration;
    public GameManager gameManager;
    
    public BoxCollider areaDeCreacion; // Arrastra un BoxCollider aquí
    public int cantidadInicial;   // Cuántos habrá al empezar
    
    private void Start()
    {
        cantidadInicial = 150;
        isCreationActive = true;
        enemies = new List<GameObject>();
        PoblarAreaInicial();
    }

    void Update()
    {
        if (isCreationActive)
        {
            if (Time.time >= tiempoProximoCreacion)
            {
                Crear();
                tiempoProximoCreacion = Time.time + tiempoEntreEnemies;
            }

            if (Time.time >= tiempoProximoCreacionBoss)
            {
                CreateBoos();
                tiempoProximoCreacionBoss = Time.time + tiempoEntreEnemiesBoss;
            }
        }
    }
    
    void PoblarAreaInicial()
    {
        // Obtenemos los límites del BoxCollider
        Bounds bounds = areaDeCreacion.bounds;

        for (int i = 0; i < cantidadInicial; i++)
        {
            // Generamos una posición aleatoria dentro del cubo
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = positionEnemy.position.y; // Mantener la altura de tus otros enemigos
            float z = Random.Range(bounds.min.z, bounds.max.z);

            Vector3 posAleatoria = new Vector3(x, y, z);
        
            // Usamos una versión modificada de tu función Crear o llamamos a Crear directamente
            GameObject enemy = Instantiate(enemyA, posAleatoria, Quaternion.identity);
            enemy.transform.parent = gameObject.transform;
            enemy.GetComponent<Enemy>().player = player;
            enemy.GetComponent<Enemy>().gameManager = gameManager;
            enemy.GetComponent<Enemy>().managerEnemy = this;
            enemy.GetComponent<Enemy>().hud = hud;
            enemies.Add(enemy);
        }
        areaDeCreacion.gameObject.SetActive(false);
    }

    void Crear()
    {
        float randomPos = 0.0f;
        if(configuration.numberLevel == 0)
            randomPos = Random.Range(-4f, 6f);
        else if(configuration.numberLevel == 1)
            randomPos = Random.Range(-4f, 4f);
        
        Vector3 posDef = new Vector3(randomPos, positionEnemy.position.y, positionEnemy.position.z);
        GameObject enemy = Instantiate(enemyA, posDef, Quaternion.identity);
        enemy.transform.parent = gameObject.transform;
        enemy.GetComponent<Enemy>().player = player;
        enemy.GetComponent<Enemy>().gameManager = gameManager;
        enemy.GetComponent<Enemy>().managerEnemy = this;
        enemy.GetComponent<Enemy>().hud = hud;
        enemies.Add(enemy);
    }

    private void CreateBoos()
    {
        float randomPos = 0.0f;
        if(configuration.numberLevel == 0)
            randomPos = Random.Range(-4f, 6f);
        else if(configuration.numberLevel == 1)
            randomPos = Random.Range(-4f, 4f);
        
        var posDef = new Vector3(randomPos, positionEnemy.position.y, positionEnemy.position.z);
        //var enemy = Instantiate(enemyBossA, posDef, Quaternion.identity);
        var enemy = Instantiate(enemyBossA, posDef, enemyBossA.transform.rotation);
        enemy.GetComponent<Enemy>().player = player;
        enemy.GetComponent<Enemy>().gameManager = gameManager;
        enemy.GetComponent<Enemy>().managerEnemy = this;
        enemies.Add(enemy);
    }

    public void MoveEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Enemy>().isMove = true;
        }
    }

    public void DestroyEnemy(Enemy enemy)
    {
        enemies.Remove(enemy.gameObject);
    }

    public void DestroyAllEnemies()
    {
        isCreationActive = false;

        // 2. Iterar la lista de enemigos "al revés"
        //    Es la forma más segura de recorrer una lista mientras
        //    potencialmente se eliminan elementos de ella.
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            // 3. Comprobar que el enemigo aún existe (no es nulo)
            if (enemies[i] != null)
            {
                // 4. Obtener su script "Enemy"
                Enemy enemyScript = enemies[i].GetComponent<Enemy>();

                if (enemyScript != null)
                {
                    // 5. ¡Darle la orden de detenerse y autodestruirse!
                    enemyScript.StopAndDestroy();
                }
            }
        }

        // 6. Vaciar la lista de seguimiento del manager
        //    Los objetos en sí se destruirán cuando termine su animación,
        //    pero el manager ya no necesita saber de ellos.
        enemies.Clear();
    }
}
