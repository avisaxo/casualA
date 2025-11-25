using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Transform objetivo;
    public float duracion;  // Cu�nto tarda en llegar (en segundos)
    private Vector3 inicio;
    private float tiempo;
    public Player player;
    public Hud hud;

    private void Start()
    {
        duracion = 1f;
        inicio = transform.position;
        tiempo = 0f;
        objetivo = player.gameObject.transform;
        transform.eulerAngles = new Vector3(90, 0, 0);
    }

    void Update()
    {
        if (objetivo == null) return;

        tiempo += Time.deltaTime;

        float t = tiempo / duracion;

        t = t * t;

        transform.position = Vector3.Lerp(inicio, objetivo.position, t);

        if (t >= 1f)
        {
            hud.AddCoin();
            AudioManager.Instance.Play("Moneda1");
            Destroy(gameObject);
        }
    }
}
