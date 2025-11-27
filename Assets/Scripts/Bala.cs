using UnityEngine;

public class Bala : MonoBehaviour
{
    public Rigidbody rb;
    private float velocidadDisparo;
    public ParticleSystem explocion;
    public Player player;

    void Start()
    {
        velocidadDisparo = 30f;
        rb.AddForce(transform.forward * velocidadDisparo, ForceMode.Impulse);
        Destroy(gameObject, 3.2f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Bala colisionó con: " + other.name);

        // Si querés afectar al objeto que tocó:
        if (other.CompareTag("EnemigoBoss"))
        {
            other.GetComponent<Enemy>().RecibirDano(0.05f);
            InstantiateExplocion();
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemigo"))
        {
            other.gameObject.GetComponent<Enemy>().InstantiateCoin(player);
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
