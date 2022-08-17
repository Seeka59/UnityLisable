using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class BloodingTime : NetworkBehaviour
{
    public float reset = 1;
    // Start is called before the first frame update
  private void Start()
  {
      Debug.Log("GGBG");
  }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag=="Untagged")
        {
            Destroy(this.gameObject);
        }
    }
}
