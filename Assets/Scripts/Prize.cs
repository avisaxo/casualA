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
        amount.fillAmount += damage;
        controler.SetTrigger("Anim");

        //Debug.Log("damage = " + damage);

        if (amount.fillAmount >= 1)
        {
            if (type == PrizesType.BulletSpeed)
                player.IncrementCreationBala();
            if(type == PrizesType.Tower)
                gameManager.CreateTowers();
            if (type == PrizesType.PlayerPoints) 
                gameManager.CreatePlayerPoints();
            if (type == PrizesType.LaserShot) 
                gameManager.CreateLaserShot();

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