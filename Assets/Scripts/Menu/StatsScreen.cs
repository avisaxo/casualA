using UnityEngine;
using Weapons;

namespace Menu
{
    public class StatsScreen : MonoBehaviour
    {
        private static StatsScreen _instance;
        public int coins;
        public int levelNumber;
        public int countEnemiesDead;
        public int countEnemiesBossDead;
        public IWeapon Weapon;
    
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            Weapon = new DefaultWeapon();
            SetInstance();
        }

        private void SetInstance()
        {
            if (_instance == null)
                _instance = this;
        }

        public void SetCoins(int _coins)
        {
            coins = _coins;
        }

        public int GetCoins()
        {
            return coins;
        }
    
        public void SetLevel(int _level)
        {
            levelNumber = _level;
        }

        public int GetLevel()
        {
            return levelNumber;
        }

        public void SetCountEnemiesDead(int _enemies)
        {
            countEnemiesDead = countEnemiesDead + countEnemiesBossDead;
        }

        public void AddEnemyDead()
        {
            countEnemiesDead++;
        }
    
        public void AddEnemyBossDead()
        {
            countEnemiesBossDead++;
        }

        public int GetCountEnemiesDead()
        {
            return countEnemiesDead;
        }
    
        public void SetWeapon(IWeapon newWeapon) => Weapon = newWeapon;

        public IWeapon GetWeapons() => Weapon;
    }
}
