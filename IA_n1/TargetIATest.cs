using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIATest : MonoBehaviour
{
   public MonstreIA MIA;

  private void OnTriggerEnter(Collider other)
  {
      if(MIA.player!=null){return;}

      if(other.gameObject.tag=="Player")
      {
          MIA.player = other.gameObject;   

           Debug.Log("TriggerEnterCoffreOn    target :" + other.gameObject.name);
      }
  }
  private void OnTriggerExit(Collider other)
  {
      
      if(MIA.player==null){return;}

      if(other.gameObject.tag=="Player")
      {
          if(other.gameObject.name == MIA.player.name)
          {
              Debug.Log("Mia.Player " + MIA.player + "DELETE PAR" + other.gameObject.name);
             MIA.player = null;  
             return;
          }
         
    
          Debug.Log("TriggerExit target : " + other.gameObject.name);
      }
  }
}
