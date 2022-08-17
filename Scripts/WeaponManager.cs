using UnityEngine;
using Mirror;

public class WeaponManager : NetworkBehaviour
{
   [SerializeField]
   private WeaponData primaryWeapon;
    [SerializeField]
   private WeaponData primaryWeaponR;

   private WeaponData currentWeapon;


   private WeaponGraphics currentGraphics;
   
   [SerializeField]private Transform weaponHolderRight;
   [SerializeField]private Transform weaponHolderLeft;

    void Start()
    {   
        EquipWeaponRight(primaryWeapon);
        EquipWeaponLeft(primaryWeaponR);
    }

    public WeaponData GetCurrentWeapon()
    {
        return currentWeapon;
    }

    void EquipWeaponRight(WeaponData weapon)
    {
        currentWeapon = weapon;

        GameObject weaponIns = Instantiate(weapon.graphics, weaponHolderRight.position,weaponHolderRight.rotation);
        weaponIns.transform.SetParent(weaponHolderRight);

    }
     void EquipWeaponLeft(WeaponData weapon)
    {
        currentWeapon = weapon;

        GameObject weaponIns = Instantiate(weapon.graphics, weaponHolderLeft.position,weaponHolderLeft.rotation);
        weaponIns.transform.SetParent(weaponHolderLeft);

    }

}
