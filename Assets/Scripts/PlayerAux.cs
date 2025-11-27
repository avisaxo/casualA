using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAux : MonoBehaviour
{
    public GameObject bala;
    public Transform positionCanon;

    private float velocidad = 5f;
    public float tiempoEntreBalas = 0.4f;
    private float tiempoProximoDisparo = 0f;
    public Image amount;
    public Player player;
    public bool isPlayerActive;
    public GameManager gameManager;

    private void Start()
    {
        isPlayerActive = true;
    }

    void Update()
    {
        /*if (Input.GetKey(KeyCode.A) && transform.position.x > -4f)
            transform.Translate(Vector3.left * velocidad * Time.deltaTime);

        if (Input.GetKey(KeyCode.D) && transform.position.x < 4f)
            transform.Translate(Vector3.right * velocidad * Time.deltaTime);*/

        if (isPlayerActive)
        {
            if (Time.time >= tiempoProximoDisparo)
            {
                Disparar();
                tiempoProximoDisparo = Time.time + tiempoEntreBalas;
            }
        }
    }

    public void IncrementCreationBala()
    {
        tiempoEntreBalas = tiempoEntreBalas - 0.1f;
    }

    void Disparar()
    {
        GameObject balaAux = Instantiate(bala, positionCanon.position, Quaternion.identity);
        balaAux.GetComponent<Bala>().gameManager = gameManager;
        balaAux.GetComponent<Bala>().player = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemigoBoss"))
        {
            player.DestroyerAux(this);
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemigo"))
        {
            player.DestroyerAux(this);
            Destroy(gameObject);
        }
    }
}
