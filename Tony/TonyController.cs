using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TonyController : NetworkBehaviour
{
[SerializeField]private GameObject _hitParticle;
[SerializeField]private GameObject tonyWin;
[SerializeField]private GameObject _Weapon;
[SerializeField]private Animator anim;
[SerializeField]private NetworkAnimator netanim;
[SyncVar]private bool _isDead = false;


public bool isDead{get{return _isDead;}protected set {_isDead = value;}}

[SerializeField] private float maxHealth = 100f;

[SyncVar]private float currentHealth;

[SerializeField]private Behaviour[] disableOnDeath;
private bool[] wasEnabledOnStart;

public void Setup(){
        wasEnabledOnStart = new bool [disableOnDeath.Length];

        for(int i = 0; i < disableOnDeath.Length;i++)
        {
            wasEnabledOnStart[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    public void SetDefaults(){
        isDead = false;
        anim.SetBool("TonyDie",false);
        currentHealth = maxHealth;

        for(int i = 0; i <disableOnDeath.Length;i++)
        {
            disableOnDeath[i].enabled = wasEnabledOnStart[i];
        }
    }


private IEnumerator Respawn(){
        yield return new WaitForSeconds(GameManager.instance.matchSetting.respawnTimer);
        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    } 
[ClientRpc]public void RpcTakeDamage(float amount,string nameHit){
    if(!isLocalPlayer)
    {
            if(isDead)
        {
            return;
        }
         anim.SetTrigger("Hit"); 
        currentHealth -= amount;
        Debug.Log(nameHit + " a toucher" + transform.name +  "Pour" + amount + "PDV");
        Debug.Log(transform.name + " a mtn" + currentHealth +  " PDV");
        if(currentHealth <=0)
        {
            Debug.Log(transform.name + "Est Mort");
            Die();
        }
    } 
    }
    private void Die(){
        isDead = true;
        anim.SetBool("TonyDie",true);
		Instantiate(tonyWin,new Vector3(gameObject.transform.position.x,
            transform.position.y,gameObject.transform.position.z),
            gameObject.transform.rotation); 
        for(int i =0;i < disableOnDeath.Length;i++)
        {
            disableOnDeath[i].enabled =false;
        } 
        Debug.Log(transform.name + "eliminer . ");
        StartCoroutine(Respawn());
    }

        
        private void OnTriggerEnter(Collider other){
             if(other.gameObject.tag=="WeaponAttack")
                {
                Debug.Log(other.gameObject.tag+"attackGameobject"+ other.gameObject);
                RpcTakeDamage(25f,other.name);
                 Instantiate(_hitParticle,new Vector3(gameObject.transform.position.x,
            transform.position.y,gameObject.transform.position.z),
            gameObject.transform.rotation);
                }   
        }

       
      

}
