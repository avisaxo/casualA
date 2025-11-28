using System.Collections.Generic;
using Config;
using Enums;
using UnityEngine;

public class PricesManager : MonoBehaviour
{
    public GameObject prizeA;
    public Transform finalPosition;
    private float pocitionPorcentage;
    public Player player;
    public List<Prize> prizes;
    public GameManager gameManager;
    public ManagerEnemies managerEnemmies;
    private LevelConfig currentLevelConfig;
    public Configuration configuration;
    public List<GameObject> prizesModel;
    
    private const float StackHeight = 2.09f;
    private const float BaseOffset = 0.5f;

    void Start()
    {
        prizes = new List<Prize>();
        LoadLevelConfig(configuration.numberLevel == 0 ? configuration.jsonConfigLevel0 : configuration.jsonConfigLevel1); 
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
        for (var i = 0; i < prizeConfigList.Count; i++)
        {
            var config = prizeConfigList[i];
            var spawnX = finalPosition.position.x;
            var spawnZ = finalPosition.position.z;
            var spawnY = finalPosition.position.y + BaseOffset + StackHeight * i; 
            var spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

            GameObject auxPrize = InstancePrize(config.PrizeType);
            
            var prizeObject = Instantiate(auxPrize, transform);
            prizeObject.transform.position = spawnPosition;

            //prizeObject.transform.parent = gameObject.transform;
            var prizeScript = prizeObject.GetComponent<Prize>();
        
            prizeScript.gameManager = gameManager;
            prizeScript.player = player;
            prizeScript.prizeManager = this;
            //prizeScript.managerEnemies = managerEnemmies;
        
            prizeScript.type = config.PrizeType;
            prizeScript.requiredHits = config.RequiredHits;
        
            prizes.Add(prizeScript);
        }
    }

    public GameObject InstancePrize(PrizesType type)
    {
        switch (type)
        {
            case PrizesType.Tower:
                return prizesModel[0];
                break;
            case PrizesType.BulletSpeed:
                return prizesModel[1];
                break;
            case PrizesType.PlayerPoints:
                return prizesModel[2];
                break;
            case PrizesType.LaserShot:
                return prizesModel[3];
                break;
            case PrizesType.WinCondition:
                return prizesModel[4];
                break;
        }
        return prizesModel[0];
    }

    public void DestroyPrize(Prize priceDestroy)
    {
        var removedIndex = prizes.IndexOf(priceDestroy);

        AudioManager.Instance.Play("Crash");
        Destroy(priceDestroy.gameObject);
        prizes.Remove(priceDestroy);

        Debug.Log("prizes.Count = " + prizes.Count);
        for (var i = removedIndex; i < prizes.Count; i++)
        {
            var prizeToMove = prizes[i];
            prizeToMove.MoveDownStack(StackHeight);
        }
        
        //foreach (var prize in prizes) prize.Advance();
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
            var prize = Instantiate(prizesModel[i], positionCreate, Quaternion.Euler(-90, 0, 0));
            prize.GetComponent<Prize>().gameManager = gameManager;
            prize.GetComponent<Prize>().player = player;
            prize.GetComponent<Prize>().prizeManager = this;
            //prize.GetComponent<Prize>().managerEnemies = managerEnemmies;
            
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
