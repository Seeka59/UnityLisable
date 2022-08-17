using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class CollisionDetection : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if( other.tag == "WeaponAttack" )
        {
            string namePlayer = other.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.name;
        }
    }
        
}
