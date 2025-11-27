using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Variables privadas para el movimiento
    private Vector3 moveDirection;
    private float speed;
    public ParticleSystem explocion;

    private void Start()
    {
        AudioManager.Instance.Play("Disparo2");
    }

    // Método que llama el 'Shooter' para inicializar la dirección
    public void SetDirectionAndSpeed(Vector3 direction, float bulletSpeed)
    {
        moveDirection = direction;
        speed = bulletSpeed;
    }

    // El movimiento que ocurre en cada frame
    void Update()
    {
        // Mueve la bala en su dirección asignada
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Bala colisionó con: " + other.name);

        // Si querés afectar al objeto que tocó:
        if (other.CompareTag("EnemigoBoss"))
        {
            other.GetComponent<Enemy>().RecibirDano(0.07f);
            InstantiateExplocion();
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemigo"))
        {
            other.gameObject.GetComponent<Enemy>().DestroyEnemy();
            InstantiateExplocion();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        
        if (other.CompareTag("PrizeA"))
        {
            other.GetComponent<Prize>().RecibirDano();
            InstantiateExplocion();
            Destroy(gameObject);
        }
    }
    
    public void InstantiateExplocion()
    {
        ParticleSystem aux = Instantiate(explocion);
        aux.transform.position = gameObject.transform.position;
    }
}
