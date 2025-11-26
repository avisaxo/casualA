using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Image barTimer;
    public TMP_Text pointsText;
    public int totalPoints;
    public int pointsNeeded;
    public PowerUpManager powerUpManager;
    private Action<bool> onGameOver;

    private void Start()
    {
        pointsText.text = "0";
        barTimer.fillAmount = 0f;
    }

    public void AddCoin()
    {
        totalPoints++;
        UpdateCoinsView();
        ValidateGameOver();
        powerUpManager.UpdatePowerUp(totalPoints);
        powerUpManager.SecondUpdatePowerUp(totalPoints);
    }
    

    private void ValidateGameOver()
    {
        if(totalPoints == pointsNeeded)
            onGameOver?.Invoke(true);
    }

    //public void SetTimeBar(float point) => barTimer.fillAmount = point;
    public void SetGameOver(Action<bool> gameOver) => onGameOver = gameOver;

    public void SubstractCoins(int coinsNeeded)
    {
        totalPoints -= coinsNeeded;
        UpdateCoinsView();
    }

    private void UpdateCoinsView()
    {
        pointsText.text = "" + totalPoints;
        var fillPercentage = (float)totalPoints / pointsNeeded;
        barTimer.fillAmount = Mathf.Clamp01(fillPercentage);
    }
}
