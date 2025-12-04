namespace Weapons
{
    public class WeaponThree : IWeapon
    {
        private const int BulletType = 3;
        private static float _reloadTime = 1f;
        
        public float GetReloadTime() => _reloadTime;

        public float GetFireRate()
        {
            throw new System.NotImplementedException();
        }

        public float GetDamage()
        {
            throw new System.NotImplementedException();
        }

        public void SetReloadTime(float time) => _reloadTime = time;

        public int GetBulletType() => BulletType;
    }
}