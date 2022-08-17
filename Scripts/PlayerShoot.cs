using Mirror;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{      
[SerializeField]private LayerMask mask;

    private WeaponData currentWeapon;
    private WeaponManager weaponManager;

    private void Start()
    { 
        weaponManager = GetComponent<WeaponManager>();
       // if (isLocalPlayer)StartCoroutine(Shoot());
    }
    /*
 IEnumerator Shoot ()
 {

     while (true)
     {
         currentWeapon = weaponManager.GetCurrentWeapon();

         if(_isAttack==true&&_hitOn == true)
         {
             Debug.Log("GgKeke");
            _CmdPlayerShot(_hitName);
            _hitOn = false;
             yield return new WaitForSeconds(0.5f);
         }
         yield return null; // this is important
     }
 }

*/
}  

      
