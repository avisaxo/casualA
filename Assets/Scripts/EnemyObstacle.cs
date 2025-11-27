using System;
using UnityEngine;

public class EnemyObstacle : MonoBehaviour
{
    public Enemy enemy;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detecto el trigger OBSTACLE");
        if ((other.tag == "Enemigo" || other.tag == "Brik") && !enemy.isBoss)
        {
            enemy.isMove = false;
        }
        if (other.tag == "Brik" && enemy.isBoss)
        {
            enemy.obstacle = true;
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Enemigo" || other.tag == "Brik") && !enemy.isBoss)
        {
            enemy.isMove = true;
        }
        if (other.tag == "Brik" && enemy.isBoss)
        {
            enemy.obstacle = false;
        }
    }
}
