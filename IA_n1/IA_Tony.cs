using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Tony : MonoBehaviour
{       
[SerializeField]private Transform tony;
[SerializeField]private Animator anim;

    

      private void OnTriggerEnter(Collider other)
        {
            
                 if(other.gameObject.tag =="Player")
            {
                anim.SetBool("TriggerOn",true);
               
            }
        }

 

}

