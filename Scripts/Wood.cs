
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;
public class Wood : NetworkBehaviour
{
    public GameObject _lootInstantiate;

    [SyncVar]private bool _isDead = false;

    public bool isDead{get{return _isDead;}protected set {_isDead = value;}}

    public float maxHealth = 100f;
    [SyncVar]private float currentHealth;

    private AudioSource _audio;
    public AudioClip axeAttackWood;
   bool hitOn;

    public void Start()
    {
        SetDefaults();
        
    }
     
    public void SetDefaults()
    {
        hitOn = true;
        isDead = false;
        currentHealth = maxHealth;
        _audio = GetComponent<AudioSource>();

    }
    
    private void TakeDamage(float amount)
    {
        if(isDead || hitOn == false)
        {
            return ;
        }
        currentHealth -= amount;
        Debug.Log(transform.name + " a mtn" + currentHealth +  " PDV");
        if(currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
       _audio.PlayOneShot(axeAttackWood,1f);
        Debug.Log(transform.name + "eliminer . ");
        Instantiate(_lootInstantiate,new Vector3(gameObject.transform.position.x,
            transform.position.y,gameObject.transform.position.z),
            gameObject.transform.rotation);  
        Destroy(this.gameObject);

    }
    private void OnCollisionEnter(Collision collision)
    {
      
        if(collision.transform.tag == "WeaponMelee");
        {
            _audio.PlayOneShot(axeAttackWood,0.7f);
            TakeDamage(25f);
            StartCoroutine(Refresh());
        }

          if(hitOn = false)
        {
            StartCoroutine(Refresh());
        }
    }
   
   IEnumerator Refresh()
    {
        hitOn = false;
        yield return new WaitForSeconds(1.5f);
        hitOn = true;
    } 
   
}
