using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bala colisionó con: " + other.name);

        // Si querés afectar al objeto que tocó:
        if (other.CompareTag("EnemigoBoss") || other.CompareTag("Enemigo"))
        {
            other.GetComponent<Enemy>().player.RecibirDano(0.1f);
            Debug.Log("PERDIO");
        }
    }
}
