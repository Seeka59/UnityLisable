using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider damageCollider;
    private void awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger=true;
        damageCollider.enabled=false;
    }
    public void EnableDamageCollider()
    {
        damageCollider.enabled=true;
    }
    public void DisableDamageCollider()
    {
        damageCollider.enabled=false;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag=="Player")
        {
            Debug.Log("yo");
        }
    }
}
