namespace Weapons
{
    public class DefaultWeapon : IWeapon
    {
        private const int BulletType = 0;
        private static float _reloadTime = 0.4f;
        
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