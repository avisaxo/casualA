using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsScreen : MonoBehaviour
{
    public static StatsScreen instance;
    public int coins;
    public int levelNumber;
    public int countEnemiesDead;
    
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SetInstance();
    }

    private void SetInstance()
    {
        if (instance == null)
            instance = this;
    }

    public void SetCoins(int _coins) => coins = _coins;

    public int GetCoins()
    {
        return coins;
    }
    
    public void SetLevel(int _level) => levelNumber = _level;

    public int GetLevel()
    {
        return levelNumber;
    }
    
    public void SetCountEnemiesDead(int _enemies) => countEnemiesDead = _enemies;
    
    public int GetCountEnemiesDead()
    {
        return countEnemiesDead;
    }
}
