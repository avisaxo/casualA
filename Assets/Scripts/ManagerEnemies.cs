using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ManagerEnemies : MonoBehaviour
{
    public GameObject enemyA;
    public GameObject enemyBossA;
    private float tiempoProximoCreacion = 0.2f;
    private float tiempoProximoCreacionBoss = 10f;
    private float tiempoEntreEnemies = 0.2f;
    private float tiempoEntreEnemiesBoss = 10f;
    public Transform positionEnemy;
    public Player player;
    public List<GameObject> enemies;
    public bool isCreationActive;
    public Hud hud;

    private void Start()
    {
        isCreationActive = true;
        enemies = new List<GameObject>();
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

    void Crear()
    {
        float prandomPos = Random.Range(-4f, 4f);
        Vector3 posDef = new Vector3(prandomPos, positionEnemy.position.y, positionEnemy.position.z);
        GameObject enemy = Instantiate(enemyA, posDef, Quaternion.identity);
        enemy.transform.parent = gameObject.transform;
        enemy.GetComponent<Enemy>().player = player;
        enemy.GetComponent<Enemy>().managerEnemy = this;
        enemy.GetComponent<Enemy>().hud = hud;
        enemies.Add(enemy);
    }

    private void CreateBoos()
    {
        var randomPos = Random.Range(-4f, 4f);
        var posDef = new Vector3(randomPos, positionEnemy.position.y, positionEnemy.position.z);
        //var enemy = Instantiate(enemyBossA, posDef, Quaternion.identity);
        var enemy = Instantiate(enemyBossA, posDef, enemyBossA.transform.rotation);
        enemy.GetComponent<Enemy>().player = player;
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
