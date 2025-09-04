namespace Project.Core.Features.Weapon.Core
{
    public interface IWeaponFactory
    {
        public IWeapon CreateWeapon(string name);
    }
}