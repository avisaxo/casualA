using System.Collections.Generic;
using Enums;
using UnityEngine;

public class PricesManager : MonoBehaviour
{
    public bool isPrizes;
    public GameObject prizeA;
    public Transform initPosition;
    public Transform finalPosition;
    public int prizeCount;
    private float pocitionPorcentage;
    public Player player;
    public List<Prize> prizes;
    public List<PrizesType> availablePrizes = new();
    public bool isStopPrize;
    public GameManager gameManager;
    public ManagerEnemies managerEnemmies;
    void Start()
    {
        isStopPrize = false;
        prizes = new List<Prize>();
        prizeCount = 50;
        pocitionPorcentage = (initPosition.position.z - finalPosition.position.z) / prizeCount;
        Debug.Log("Porcentaje prize = " + pocitionPorcentage);
        CreatePrize();
    }

    void CreatePrize()
    {
        for (var i = 0; i < prizeCount; i++)
        {
            var pos = new Vector3(0, -0.4f, pocitionPorcentage * i);
            var prize = Instantiate(prizeA, initPosition.position - pos, Quaternion.Euler(90, 180, 0));
            prize.transform.parent = gameObject.transform;
            prize.GetComponent<Prize>().gameManager = gameManager;
            prize.GetComponent<Prize>().player = player;
            prize.GetComponent<Prize>().prizeManager = this;
            prize.GetComponent<Prize>().managerEnemies = managerEnemmies;
            
            // if (i == 1 || i == 0)
            //     prize.GetComponent<Prize>().type = PrizesType.BulletSpeed;
            // if (i == 2)
            //     prize.GetComponent<Prize>().type = PrizesType.Tower;
            // if (i == 4 || i == 3)
            //     prize.GetComponent<Prize>().type = PrizesType.PlayerPoints;
            var randomIndex = Random.Range(0, availablePrizes.Count);
            prize.GetComponent<Prize>().type = availablePrizes[randomIndex];
            
            prizes.Add(prize.GetComponent<Prize>());
        }
    }

    public void PricesAdvance(bool advance)
    {
        if (!advance)
        {
            for (var i = 0; i < prizes.Count; i++)
            {
                prizes[i].Stop();
            }

            if (!isStopPrize)
            {
                isStopPrize = true;
                CreatePrizeDestroyed();
            }
        }
    }

    public void DestroyPrize(Prize priceDestroy)
    {
        AudioManager.Instance.Play("Crash");
        Destroy(priceDestroy.gameObject);
        prizes.Remove(priceDestroy);
        for (var i = 0; i < prizes.Count; i++)
        {
            prizes[i].Advance();
        }
        //CreatePrizeDestroyed();
    }

    Vector3 CalculateNextPrize()
    {
        GameObject objetoMasAdelante = prizes[0].gameObject;
        float maxZ = prizes[0].gameObject.transform.position.z;
        
        for (var i = 1; i < prizes.Count; i++)
        {
            float zActual = prizes[i].transform.position.z;
            if (zActual > maxZ)
            {
                maxZ = zActual;
                objetoMasAdelante = prizes[i].gameObject;
            }
        }
        return objetoMasAdelante.transform.position;
    }

    void CreatePrizeDestroyed()
    {
        int countPrize = 6 - prizes.Count;
        Vector3 currentFirstPos = CalculateNextPrize();
        for (int i = 0; i < countPrize; i++)
        {
            Vector3 positionCreate = new Vector3(currentFirstPos.x, currentFirstPos.y, currentFirstPos.z + 6);
            GameObject prize = Instantiate(prizeA, positionCreate, Quaternion.Euler(-90, 0, 0));
            prize.GetComponent<Prize>().gameManager = gameManager;
            prize.GetComponent<Prize>().player = player;
            prize.GetComponent<Prize>().prizeManager = this;
            prize.GetComponent<Prize>().managerEnemies = managerEnemmies;
            
            if (i == 1 || i == 0)
                prize.GetComponent<Prize>().type = PrizesType.BulletSpeed;
            if (i == 2)
                prize.GetComponent<Prize>().type = PrizesType.Tower;
            if (i == 4 || i == 3)
                prize.GetComponent<Prize>().type = PrizesType.PlayerPoints;
            
            prizes.Add(prize.GetComponent<Prize>());
        }
    }
}
