using System;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class Prize : MonoBehaviour
{
    // --- Referencias ---
    public Image amount;
    public PricesManager prizeManager;
    public Player player;
    public GameManager gameManager;
    public Animator controler;

    // --- Configuración de Estado ---
    public PrizesType type;
    public int requiredHits;
    
    // --- Lógica de Movimiento ---
    public float targetY;
    public float stackDropSpeed = 8f;

    private void Start()
    {
        targetY = transform.position.y;
        transform.localScale = new Vector3(1,1,1);
    }

    void Update()
    {
        if (transform.position.y != targetY)
        {
            var targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                stackDropSpeed * Time.deltaTime
            );
        }
    }
    
    public void MoveDownStack(float heightToDrop)
    {
        targetY -= heightToDrop;
    }

    public void RecibirDano()
    {
        var damagePerHit = 1.0f / requiredHits;
    
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
                    gameManager.GameOver(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Tipo de premio no manejado en el switch.");
            }

            prizeManager.DestroyPrize(this);
        }
    }
}