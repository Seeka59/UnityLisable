using UnityEngine;
using Mirror;
using System.Collections;
public class Player : NetworkBehaviour
{
    // syncro statue attake / def 
    
[SyncVar]public string _States = null;
[SyncVar]public bool _isAttack = false;
[SyncVar]public bool _isParade= false;
[SyncVar]private bool _isDead = false;

public bool isAttack{get{return _isAttack;}protected set{_isAttack = value;}}
public bool isParade{get{return _isParade;}protected set{_isParade= value;}}
public bool isDead{get{return _isDead;}protected set {_isDead = value;}}

private float maxHealth = 100f;

[SyncVar]private float currentHealth;
public float GetHealthPct(){return (float)currentHealth / maxHealth;}

    public int _nbrKills;
    public int _nbrDeaths;

[SerializeField]private Behaviour[] disableOnDeath;

[SerializeField]private GameObject[] disableGameObjectsOnDeath;
private bool[] wasEnabledOnStart;
private bool firstSetup = true;

private void Start()
{
     isDead = false;
    currentHealth = maxHealth;
}
   public void Setup()
    {  
        GetComponent<PlayerSetup>().playerUiInstance.SetActive(true);
        CmdBroadcastNewPlayerSetup();
    }

private void Update()
{
    if(currentHealth<=0)
    {Die();}
}
 public void SetDefaults()
    {
        
        isDead = false;
        currentHealth = maxHealth;

    }

 [Command(requiresAuthority = false)]
     private void CmdBroadcastNewPlayerSetup()
     {
         RpcSetupPlayerOnAllClients();
         }
    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if(firstSetup)
        {
            wasEnabledOnStart = new bool[disableOnDeath.Length];
            for (int i = 0; i < disableOnDeath.Length; i++)
            {wasEnabledOnStart[i] = disableOnDeath[i].enabled;}
            firstSetup = false;
        }
        SetDefaults();
    }

    [Command]
    public void CmdMagic(GameObject target, float damage)
    {
        target.GetComponent<Player>().currentHealth -= damage;
        NetworkIdentity opponentIdentity = target.GetComponent<NetworkIdentity>();
        TargetDoMagic(opponentIdentity.connectionToClient, damage);
    }
    [TargetRpc]
    public void TargetDoMagic(NetworkConnection target, float damage)
    {
        // This will appear on the opponent's client, not the attacking player's
        Debug.Log($"Magic Damage = {damage}");
    }

    // Heal thyself
    [Command]
    public void CmdHealMe()
    {
        currentHealth += 10;
        TargetHealed(10);
    }

    [TargetRpc]
    public void TargetHealed(int amount)
    {
        // No NetworkConnection parameter, so it goes to owner
        Debug.Log($"Health increased by {amount}");
    }


      private void Die()
    {
        isDead = true;
        Debug.Log("DieSvr");
        _nbrDeaths++;
        for(int i =0;i < disableOnDeath.Length;i++)
        {
            disableOnDeath[i].enabled =false;
        } 

        Debug.Log(transform.name + "eliminer . ");
        StartCoroutine(Respawn());

    }

      private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSetting.respawnTimer);
        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;   
    } 
}
