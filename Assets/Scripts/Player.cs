using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject bala;
    public Transform positionCanon;
    public List<GameObject> points;
    // private int indexPoints = 0;
    // public List<GameObject> playerPoints;
    private PlayerAux[] activeHelpers;
    [Header("Configuración de Victoria")]
    [Tooltip("Velocidad a la que el jugador se 'dispara' hacia adelante al ganar.")]
    public float winSpeed = 100f;
    [Tooltip("Tiempo que pasa yendo hacia atrás antes de salir disparado")]
    public float anticipationTime = 1f; 
    [Tooltip("Velocidad suave hacia atrás")]
    public float backwardSpeed = 10f;

    private bool isWinning = false;

    private float velocidad = 5f;
    public float tiempoEntreBalas = 0.4f;
    private float tiempoProximoDisparo = 0f;
    public Image amount;
    public GameObject auxPlayerPrefabs;
    public bool isPlayerActive;

    private void Start()
    {
        isPlayerActive = true;
        //playerPoints = new List<GameObject>();
        activeHelpers = new PlayerAux[points.Count];
    }

    void Update()
    {
        if (isWinning)
        {
            transform.Translate(Vector3.forward * winSpeed * Time.unscaledDeltaTime);
            return; 
        }
        
        #if UNITY_ANDROID || UNITY_IOS
                if (Input.touchCount > 0)
                {
                    Touch toque = Input.GetTouch(0);
        
                    // Detectar movimiento horizontal y vertical
                    Vector2 delta = toque.deltaPosition;
        
                    if (toque.phase == TouchPhase.Stationary || toque.phase == TouchPhase.Moved)
                    {
                        // Mover izquierda/derecha según lado de pantalla
                        if (toque.position.x < Screen.width / 2f && transform.position.x > -4f)
                        {
                            transform.Translate(Vector3.left * velocidad * Time.deltaTime);
                        }
                        else if (toque.position.x >= Screen.width / 2f && transform.position.x < 4f)
                        {
                            transform.Translate(Vector3.right * velocidad * Time.deltaTime);
                        }
        
                        // 📈 Mover adelante/atrás según desplazamiento del dedo
                        if (Mathf.Abs(delta.y) > 10f) // umbral para evitar ruido de movimiento leve
                        {
                            if (delta.y > 0 && transform.position.z <= 1f)
                            {
                                transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
                            }
                            else if (delta.y < 0 && transform.position.z >= -1f)
                            {
                                transform.Translate(Vector3.back * velocidad * Time.deltaTime);
                            }
                        }
                    }
                }
        #endif
        
        if (isPlayerActive)
        {
            if (Input.GetKey(KeyCode.A) && transform.position.x > -4f)
                transform.Translate(Vector3.left * velocidad * Time.deltaTime);

            if (Input.GetKey(KeyCode.D) && transform.position.x < 4f)
                transform.Translate(Vector3.right * velocidad * Time.deltaTime);

            if (Input.GetKey(KeyCode.W) && transform.position.z <= 1f)
                transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

            if (Input.GetKey(KeyCode.S) && transform.position.z >= -1f)
                transform.Translate(Vector3.forward * -velocidad * Time.deltaTime);

            if (Time.time >= tiempoProximoDisparo)
            {
                Disparar();
                tiempoProximoDisparo = Time.time + tiempoEntreBalas;
            }
        }
        else
        {
            foreach (var playerAux in activeHelpers)
            {
                if(playerAux != null) playerAux.isPlayerActive = false;
            }
        }
    }

    public void IncrementCreationBala()
    {
        if(tiempoEntreBalas >= 0.2f)
            tiempoEntreBalas = tiempoEntreBalas - 0.1f;
    }

    void Disparar()
    {
        var balaAux = Instantiate(bala, positionCanon.position, Quaternion.identity);
        balaAux.GetComponent<Bala>().player = this;
        AudioManager.Instance.Play("Disparo1");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemigoBoss")) 
            RecibirDano(0.5f);

        if (other.CompareTag("Enemigo")) 
            RecibirDano(0.1f);
        
        if (other.CompareTag("Tower")) 
            other.gameObject.GetComponent<ProgressCircular>().StartGreenFill(2.0f);
    }

    public void RecibirDano(float damage)
    {
        amount.fillAmount += damage;

        Debug.Log("fillAmount = " + amount.fillAmount);

        if (amount.fillAmount >= 1) Destroy(gameObject);
    }

    public void CreatePlayerpoint()
    {
        var freeSlotIndex = -1;
        for (var i = 0; i < activeHelpers.Length; i++)
        {
            if (activeHelpers[i] == null)
            {
                freeSlotIndex = i; 
                break;
            }
        }

        if (freeSlotIndex != -1)
        {
            //Debug.Log("Creando player aux en la ranura: " + freeSlotIndex);

            var spawnPosition = points[freeSlotIndex].transform.position;

            var auxPlayer = Instantiate(auxPlayerPrefabs, spawnPosition, Quaternion.identity);
            var auxScript = auxPlayer.GetComponent<PlayerAux>();
            
            auxScript.player = this;
            auxPlayer.transform.parent = transform;
            activeHelpers[freeSlotIndex] = auxScript;
        }
        else
        {
            Debug.LogWarning("¡Máximo de 'PlayerPoints' alcanzado! No hay ranuras libres.");
        }
    }

    public void DestroyerAux(PlayerAux playerAux)
    {
        for (var i = 0; i < activeHelpers.Length; i++)
        {
            if (activeHelpers[i] == playerAux)
            {
                activeHelpers[i] = null;
                Destroy(playerAux.gameObject); 
                return;
            }
        }
    }

    public void WinAnimation()
    {
        AudioManager.Instance.Play("Victoria");
        StartCoroutine(WinSequenceRoutine());
    }
    
    IEnumerator WinSequenceRoutine()
    {
        isPlayerActive = false;

        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        var allHelpers = FindObjectsOfType<PlayerAux>();
        foreach (var h in allHelpers) Destroy(h.gameObject);

        if (Camera.main != null)
        {
            var camScript = Camera.main.GetComponent<CameraFollowSmooth>();
            if (camScript != null) camScript.target = null;
        }

        
        var timer = 0f;
        while (timer < anticipationTime)
        {
            transform.Translate(Vector3.back * backwardSpeed * Time.unscaledDeltaTime);
            timer += Time.unscaledDeltaTime;
            yield return null; 
        }
        
        isWinning = true; 
    }
}
