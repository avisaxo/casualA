using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    public Hud hud;
    
    public Button firstPowerUpButton;
    public GameObject firstPowerUpGameObject;
    public Text firstPowerUpText;
    public GameObject firstDisabledPowerUpGameObject;
    public GameObject firstEnablePowerUpGameObject;
    public GameManager gameManager;
    private const int CoinsNeeded = 40;

    // Start is called before the first frame update
    void Start()
    {
        firstPowerUpButton.onClick.AddListener(ExecutePowerUp);
    }

    private void ExecutePowerUp()
    {
        Debug.Log("Se ejecuto el power up");
        gameManager.CreateMissile();
        hud.SubstractCoins(CoinsNeeded);
        firstPowerUpGameObject.SetActive(false);
    }

    public void ShowPowerUp() 
    {
        firstPowerUpGameObject.SetActive(true);
        firstEnablePowerUpGameObject.SetActive(false);
        firstDisabledPowerUpGameObject.SetActive(true);
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
}
