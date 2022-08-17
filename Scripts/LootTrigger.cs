
using UnityEngine;
using Mirror;
public class LootTrigger : NetworkBehaviour
{
private void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }
private void OnTriggerEnter(Collider other)
  {
      Debug.Log(other.gameObject.tag);
      if(other.gameObject.tag == "Player")
      {
        InventaireKevin Ik = other.gameObject.GetComponent<InventaireKevin>();
          Ik.InventaireIncrem("wood",5);    
          DestroyGameObject();
      }
  }
}
