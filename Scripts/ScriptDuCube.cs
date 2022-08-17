using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDuCube : MonoBehaviour
{
  public Transform explosionPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        if(collision.gameObject.name == "Player")
        {
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;
        Instantiate(explosionPrefab, position, rotation);
        Destroy(gameObject);

        }
        
    }
}
