using Mirror;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
   [SerializeField]Behaviour[]componentsToDisable;
   [SerializeField]Behaviour componentsToEnable;
    [SerializeField]private GameObject playerUiPrefab;
    public GameObject playerUiInstance;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
        }
        else
        {
            DebugInput();
            // Création du UI du joueur local
            playerUiInstance = Instantiate(playerUiPrefab);
            // Configuration du UI
            PlayerUiKevin ui = playerUiInstance.GetComponent<PlayerUiKevin>();
            if(ui == null)
            {
                Debug.LogError("Pas de component PlayerUI sur playerUIInstance");
            }
            else
            {
                ui.SetPlayer(GetComponent<Player>());
            }

            GetComponent<Player>().Setup();
        } 
    }

    private void  SetLayerRecursively(GameObject obj,int newLayer)
    {
        obj.layer = newLayer;
        foreach(Transform child in obj.transform)
        { SetLayerRecursively(child.gameObject, newLayer);}
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netId, player);   
    }
    //public override void OnStopClient(NetworkConnection conn)
    
    private void DebugInput()
    {
        componentsToEnable.enabled = false;
        componentsToEnable.enabled = true; 
    }
   
    private void DisableComponents()
    {
         for(int i = 0;i<componentsToDisable.Length;i++)
          {
              componentsToDisable[i].enabled = false;
          }
    }

}
