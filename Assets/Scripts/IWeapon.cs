public interface IWeapon
{
    float GetReloadTime();
    float GetFireRate();
    float GetDamage();

    void SetReloadTime(float time);
    int GetBulletType();
}