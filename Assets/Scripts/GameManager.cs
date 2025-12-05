using System;
using System.Collections.Generic;
using Menu;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Level0 level0;
    public Level0 level1;
    public Level0 currentLevel;
    public Player player;
    public ManagerEnemies managerEnemies;
    public PricesManager prizesMManager;
    public Hud hud;
    public EndCardView endCardView;
    [SerializeField] public List<bool> levels;
    public CameraFollowSmooth cameraFollow;
    public PowerUpManager powerUpManager;
    public GameObject brickObstacle;
    [SerializeField] public Configuration configuration;
    public StatsScreen statsScreen;
    private Player auxPlayer;
    public float tiempoCreationFinish = 200f;
    private float tiempoActual = 0f;

    private void Awake()
    {
        statsScreen = GameObject.Find("StatsScreen").GetComponent<StatsScreen>();
    }

    void Start()
    {
        switch (configuration.numberLevel)
        {
            case 0:
                currentLevel = Instantiate(level0);
                break;
            case 1:
                currentLevel = Instantiate(level1);
                break;
            default:
                currentLevel = currentLevel;
                break;
        }
        
        currentLevel.gameManager = this;

        statsScreen.SetLevel(configuration.numberLevel);
        
        levels = new List<bool>();
        CreateLevels();
        if (!levels[0])
        {
            levels[0] = true;
            CreateLevel0();
        }
        
        hud.SetGameOver(GameOver);
        brickObstacle = currentLevel.obstacleBrick;
        currentLevel.obstacleBrick.GetComponent<BrickObstacle>().gameManager = this;
    }

    public void GameOver(bool isWin)
    {
        managerEnemies.DestroyAllEnemies();
        auxPlayer.WinAnimation();
        endCardView.ShowEndCard();
        statsScreen.SetCoins(hud.GetCoins());
    }

    private void EndCardOptionSelected(int discountAmount)
    {
        hud.ApplyDiscount(discountAmount);
        RestartLevel();
    }
    
    private void RestartLevel()
    {
        switch (configuration.numberLevel)
        {
            case 0:
                currentLevel = Instantiate(level0);
                break;
            case 1:
                currentLevel = Instantiate(level1);
                break;
            default:
                currentLevel = currentLevel;
                break;
        }

        levels = new List<bool>();
        CreateLevels();
        if (!levels[0])
        {
            levels[0] = true;
            CreateLevel0();
        }
        
        hud.UpdateCoinsView();
        brickObstacle = currentLevel.obstacleBrick;
        currentLevel.obstacleBrick.GetComponent<BrickObstacle>().gameManager = this;
    }

    private void Update()
    {
        Debug.Log("Time = " + Time.time + " Finish = " + tiempoCreationFinish);
        if (Time.time >= tiempoCreationFinish)
        {
            managerEnemies.isCreationActive = false;
            auxPlayer.isPlayerActive = false;
            hud.barTimer.fillAmount = 1;
        }
        else
        {
            tiempoActual += Time.deltaTime;
            var progress = tiempoActual / tiempoCreationFinish;
            hud.barTimer.fillAmount = tiempoActual / tiempoCreationFinish;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            CreateTowers();
        }
    }

    void CreateLevels()
    {
        levels.Add(false);
    }

    void CreateLevel0()
    {
        auxPlayer = Instantiate(player);
        auxPlayer.gameManager = this;
        auxPlayer.GetComponent<Player>().statsScreen = statsScreen;
        managerEnemies = Instantiate(managerEnemies);
        managerEnemies.gameManager = this;
        managerEnemies.configuration = configuration;
        managerEnemies.hud = hud;
        managerEnemies.positionEnemy = currentLevel.positionEnemy;
        currentLevel.managerEnemies = managerEnemies;
        // Price Manager create
        if (configuration.numberLevel != 1)
        {
            var manager = Instantiate(prizesMManager);
            manager.configuration = configuration;
            manager.gameManager = this;
            //manager.initPosition = currentLevel.initPosition;
            manager.finalPosition = currentLevel.finalPosition;
            manager.player = auxPlayer;
            manager.managerEnemmies = managerEnemies;   
        }

        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(auxPlayer.transform);
        }
    }

    public void CreateTowers()
    {
        currentLevel.managerEnemies = managerEnemies;
        currentLevel.CreateProgresBar();
        currentLevel.CreateProgresBar1();
    }

    public void CreatePlayerPoints()
    {
        auxPlayer.CreatePlayerpoint();
    }

    public void CreateLaserShot()
    {
        powerUpManager.ShowPowerUp();
    }

    public void CreateMissile()
    {
        currentLevel.FireMissileToTarguet();
    }

    public void BrickActive()
    {
        brickObstacle.SetActive(true);
        brickObstacle.GetComponent<BrickObstacle>().StartDelayAction();
    }

    public void MoveEnemies()
    {
        managerEnemies.MoveEnemies();
    }
}
