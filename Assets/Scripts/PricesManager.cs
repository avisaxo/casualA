using System.Collections.Generic;
using Config;
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
    private LevelConfig currentLevelConfig;

    void Start()
    {
        isStopPrize = false;
        prizes = new List<Prize>();
        LoadLevelConfig("Config/Level1");
        //pocitionPorcentage = (initPosition.position.z - finalPosition.position.z) / prizeCount;
    }
    
    private void LoadLevelConfig(string fileName)
    {
        var jsonText = Resources.Load<TextAsset>(fileName);
        
        if (jsonText != null)
        {
            currentLevelConfig = JsonUtility.FromJson<LevelConfig>(jsonText.text);
            Debug.Log($"Configuración de nivel '{currentLevelConfig.LevelName}' cargada exitosamente.");
            
            if (currentLevelConfig.PrizeDrops.Count != currentLevelConfig.WinCondition.RequiredPrizeCount)
            {
                Debug.LogWarning("La lista de premios no coincide con la cantidad necesaria para ganar.");
            }

            CreatePrices();
        }
        else
        {
            Debug.LogError($"No se encontró el archivo JSON: {fileName}.");
        }
    }
    
    void CreatePrices()
    {
        var prizeConfigList = currentLevelConfig.PrizeDrops;
    
        int realPrizeCount = prizeConfigList.Count;
        pocitionPorcentage = (initPosition.position.z - finalPosition.position.z) / realPrizeCount;
    
        for (var i = 0; i < realPrizeCount; i++)
        {
            var config = prizeConfigList[i];
        
            var pos = new Vector3(0, -0.4f, pocitionPorcentage * i);
            var spawnPosition = initPosition.position - pos; 

            var prizeObject = Instantiate(prizeA, spawnPosition, Quaternion.Euler(90, 180, 0));
    
            prizeObject.transform.parent = gameObject.transform;
            var prizeScript = prizeObject.GetComponent<Prize>();
        
            prizeScript.gameManager = gameManager;
            prizeScript.player = player;
            prizeScript.prizeManager = this;
            prizeScript.managerEnemies = managerEnemmies;
        
            prizeScript.type = config.PrizeType;
            prizeScript.requiredHits = config.RequiredHits;
        
            prizes.Add(prizeScript);
        }
    }

    public void PricesAdvance(bool advance)
    {
        if (advance) return;
        foreach (var prize in prizes) prize.Stop();

        if (isStopPrize) return;
        isStopPrize = true;
        CreatePrizeDestroyed();
    }

    public void DestroyPrize(Prize priceDestroy)
    {
        AudioManager.Instance.Play("Crash");
        Destroy(priceDestroy.gameObject);
        prizes.Remove(priceDestroy);
        foreach (var prize in prizes) prize.Advance();
        //CreatePrizeDestroyed();
    }

    Vector3 CalculateNextPrize()
    {
        var objetoMasAdelante = prizes[0].gameObject;
        var maxZ = prizes[0].gameObject.transform.position.z;
        
        for (var i = 1; i < prizes.Count; i++)
        {
            var zActual = prizes[i].transform.position.z;
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
        var countPrize = 6 - prizes.Count;
        var currentFirstPos = CalculateNextPrize();
        for (var i = 0; i < countPrize; i++)
        {
            var positionCreate = new Vector3(currentFirstPos.x, currentFirstPos.y, currentFirstPos.z + 6);
            var prize = Instantiate(prizeA, positionCreate, Quaternion.Euler(-90, 0, 0));
            prize.GetComponent<Prize>().gameManager = gameManager;
            prize.GetComponent<Prize>().player = player;
            prize.GetComponent<Prize>().prizeManager = this;
            prize.GetComponent<Prize>().managerEnemies = managerEnemmies;
            
            if (i is 1 or 0)
                prize.GetComponent<Prize>().type = PrizesType.BulletSpeed;
            if (i == 2)
                prize.GetComponent<Prize>().type = PrizesType.Tower;
            if (i is 4 or 3)
                prize.GetComponent<Prize>().type = PrizesType.PlayerPoints;
            
            prizes.Add(prize.GetComponent<Prize>());
        }
    }
}
