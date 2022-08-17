using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIA : MonoBehaviour
{
    private Collider _coll;
    public MonstreIA _MonstreIA;

void Start()
 {
    _coll = GetComponent<Collider>();
     _MonstreIA = gameObject.transform.root.GetComponent<MonstreIA>();
     _coll.enabled = false;
 }

void Update()
{
    if(_MonstreIA._isAttackIA == true && _coll.enabled == false)
    {
        _coll.enabled =true;
    }
    if(_MonstreIA._isAttackIA == false && _coll.enabled == true)
    {
        _coll.enabled = false;
    }   
}


     private void OnTriggerEnter(Collider other)
    {   
        if(_MonstreIA._isAttackIA == true)
        {
            
            if(other.gameObject.tag == "Player")
            {
                Player _player = other.gameObject.GetComponent<Player>();
                if(_player._isParade == true){return;}
                _MonstreIA._hitName = other.gameObject.name;   
                _MonstreIA._hitCollider = true;
            }
        }
    }
}
