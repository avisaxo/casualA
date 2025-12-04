namespace Weapons
{
    public class WeaponTwo : IWeapon
    {
        private const int BulletType = 2;
        private static float _reloadTime = 0.7f;
        
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