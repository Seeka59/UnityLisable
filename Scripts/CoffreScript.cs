using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffreScript : MonoBehaviour
{
    public GameObject _player;

   void OnTriggerEnter(Collider other)
    {
        if( other.tag == "Player" )
        {
            Debug.Log("EnterTrigger");
            Debug.Log(other.gameObject);
           _player.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("ExitTrigger");
        _player.SetActive(false);
    }

}
