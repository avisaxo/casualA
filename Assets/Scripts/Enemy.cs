using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float velocidad;
    public Image amount;
    public bool isBoss;
    public Player player;
    public ManagerEnemies managerEnemy;
    public Animator animator;
    public bool isMove;
    private AnimatorStateInfo stateInfo;
    public GameObject coin;
    public Hud hud;
    public bool obstacle;
    public GameManager gameManager;

    private void Start()
    {
        isMove = true;
        velocidad = isBoss ? 2.5f : 1.5f;
        obstacle = false;
    }

    void Update()
    {
        if(isMove && !isBoss)
            transform.Translate(-Vector3.forward * velocidad * Time.deltaTime);
        else if (isMove)
        {
            if (!obstacle)
            {
                transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
            }
        }

        if (!isMove && isBoss)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Die") && stateInfo.normalizedTime >= 1f)
            {
                gameManager.statsScreen.AddEnemyBossDead();
                AudioManager.Instance.Play("muerte2");
                Destroy(gameObject);
                Debug.Log("🎬 La animación terminó");
            }
        }

        if (!isMove && !isBoss)
        {
            Debug.Log("Se detubo");
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Missile")
        {
            Debug.Log("Colisiono contra el missile Stay");
            DestroyEnemy();
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Missile")
        {
            Debug.Log("Colisiono contra el missile Enter");
            DestroyEnemy();
        }
    }

    public void RecibirDano(float damage)
    {
        if (isMove)
        {
            if (isBoss) amount.fillAmount += damage;

            //Debug.Log("damage = " + damage);

            if (amount.fillAmount >= 1)
            {
                AudioManager.Instance.Play("Muerte1");
                isMove = false;
                animator.SetTrigger("Die");
                managerEnemy.DestroyEnemy(this);
                //Destroy(gameObject);
            }
        }
    }

    public void InstantiateCoin(Player player)
    {
        GameObject auxCoin = Instantiate(coin, transform.position, Quaternion.identity);
        auxCoin.GetComponent<Coin>().player = player;
        auxCoin.GetComponent<Coin>().hud = hud;
    }

    public void DestroyEnemy()
    {
        managerEnemy.DestroyEnemy(this);
        gameManager.statsScreen.AddEnemyDead();
        Destroy(gameObject);
    }

    public void StopAndDestroy()
    {
        isMove = false;
        Destroy(gameObject);
    }
}
