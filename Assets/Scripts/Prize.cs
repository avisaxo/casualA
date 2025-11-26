using System;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class Prize : MonoBehaviour
{
    public Image amount;
    public float velocidad;
    public PricesManager prizeManager;
    public Player player;
    public GameManager gameManager;
    public ManagerEnemies managerEnemies;
    public PrizesType type;
    public Animator controler;
    public int requiredHits;

    private void Start()
    {
        velocidad = 0.5f;
    }
    
    void Update()
    {
        transform.Translate(-Vector3.down * velocidad * Time.deltaTime);
        if (transform.position.z <= prizeManager.finalPosition.position.z)
        {
            prizeManager.PricesAdvance(false);
            //Debug.Log("Llego al final del avance");
        }
    }

    public void RecibirDano(float damage)
    {
        // Calculamos el valor de salud que representa un golpe
        // 1.0f / 5 hits = 0.2 de fillAmount por golpe.
        var damagePerHit = 1.0f / requiredHits;
    
        // Sumamos la porción de un golpe
        amount.fillAmount += damagePerHit;
    
        controler.SetTrigger("Anim");

        if (amount.fillAmount >= 1)
        {
            switch (type)
            {
                case PrizesType.BulletSpeed:
                    player.IncrementCreationBala();
                    break;
                case PrizesType.Tower:
                    gameManager.CreateTowers();
                    break;
                case PrizesType.PlayerPoints:
                    gameManager.CreatePlayerPoints();
                    break;
                case PrizesType.LaserShot:
                    gameManager.CreateLaserShot();
                    break;
                case PrizesType.WinCondition:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            prizeManager.DestroyPrize(this);
        }
    }

    public void Stop()
    {
        velocidad = 0.0f;
    }
    
    public void Advance()
    {
        prizeManager.isStopPrize = false;
        velocidad = 0.5f;
    }
}