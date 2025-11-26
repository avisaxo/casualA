using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    public Hud hud;
    
    public Button firstPowerUpButton;
    public Button secondPowerUpButton;
    public GameObject firstPowerUpGameObject;
    public GameObject secondPowerUpGameObject;
    public Text firstPowerUpText;
    public Text secondPowerUpText;
    public GameObject firstDisabledPowerUpGameObject;
    public GameObject secondDisabledPowerUpGameObject;
    public GameObject firstEnablePowerUpGameObject;
    public GameObject secondEnablePowerUpGameObject;
    public GameManager gameManager;
    private const int CoinsNeeded = 40;
    private const int secondCoinsNeeded = 60;
    public GameObject brickObject;

    // Start is called before the first frame update
    void Start()
    {
        firstPowerUpButton.onClick.AddListener(ExecutePowerUp);
        secondPowerUpButton.onClick.AddListener(SecondExecutePowerUp);
    }

    private void ExecutePowerUp()
    {
        Debug.Log("Se ejecuto el power up");
        gameManager.CreateMissile();
        hud.SubstractCoins(CoinsNeeded);
        firstPowerUpGameObject.SetActive(false);
    }
    
    private void SecondExecutePowerUp()
    {
        brickObject = GameObject.Find("ObstacleBrik");
        Debug.Log("Se ejecuto el power up");
        gameManager.BrickActive();
        hud.SubstractCoins(secondCoinsNeeded);
        SecondShowPowerUp();
    }

    public void ShowPowerUp() 
    {
        firstPowerUpGameObject.SetActive(true);
        firstEnablePowerUpGameObject.SetActive(false);
        firstDisabledPowerUpGameObject.SetActive(true);
    }
    
    public void SecondShowPowerUp() 
    {
        secondPowerUpGameObject.SetActive(true);
        secondEnablePowerUpGameObject.SetActive(false);
        secondDisabledPowerUpGameObject.SetActive(true);
    }

    public void FireMissile()
    {
        Debug.Log("Disparo misiles");
    }

    public void UpdatePowerUp(int coins)
    {
        var coinsLeft = CoinsNeeded - coins;
        if (coinsLeft < 0)
        {
            firstDisabledPowerUpGameObject.SetActive(false);
            firstEnablePowerUpGameObject.SetActive(true);
        }
        else
            firstPowerUpText.text = coinsLeft.ToString();
    }
    
    public void SecondUpdatePowerUp(int coins)
    {
        var coinsLeft = secondCoinsNeeded - coins;
        if (coinsLeft < 0)
        {
            secondDisabledPowerUpGameObject.SetActive(false);
            secondEnablePowerUpGameObject.SetActive(true);
        }
        else
            secondPowerUpText.text = coinsLeft.ToString();
    }
}
