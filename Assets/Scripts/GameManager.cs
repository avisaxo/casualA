using System.Collections.Generic;
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
    [SerializeField] public List<bool> levels;
    public CameraFollowSmooth cameraFollow;
    public PowerUpManager powerUpManager;
    public GameObject brickObstacle;
    [SerializeField] public Configuration configuration;
    private Player auxPlayer;
    private float tiempoCreationFinish = 200f;
    private float particionTimer;
    private float tiempoActual = 0f;

    void Start()
    {
        particionTimer = tiempoCreationFinish / 100;
        if (configuration.numberLevel == 0)
        {
            currentLevel = Instantiate(level0);
        } else if (configuration.numberLevel == 1)
        {
            currentLevel = Instantiate(level1);
        }
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

    private void GameOver(bool isWin)
    {
        managerEnemies.DestroyAllEnemies();
        auxPlayer.WinAnimation();
    }

    private void Update()
    {
        if (Time.time >= tiempoCreationFinish)
        {
            managerEnemies.isCreationActive = false;
            auxPlayer.isPlayerActive = false;
            //hud.SetTimeBar(1f);
        }
        else
        {
            tiempoActual += Time.deltaTime;
            var progress = tiempoActual / tiempoCreationFinish;
            //hud.SetTimeBar(progress);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            CreateTowers();
        }
    }

    void CreateLevels() => levels.Add(false);

    void CreateLevel0()
    {
        auxPlayer = Instantiate(player);
        managerEnemies = Instantiate(managerEnemies);
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

    public void CreatePlayerPoints() => auxPlayer.CreatePlayerpoint();

    public void CreateLaserShot() => powerUpManager.ShowPowerUp();

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
