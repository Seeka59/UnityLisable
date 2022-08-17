using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
public class MonstreIA : NetworkBehaviour
{
    [SyncVar]
    public GameObject player = null;
    public GameObject loot;
    //Agent de Navigation
    
    NavMeshAgent navMeshAgent;
    //Animations
    Animator animator;
    const string STAND_STATE = "Stand";
    const string WALK_STATE = "Walk";
    const string ATTACK_STATE = "Attack";
    const string HIT_STATE = "Hit";
    const string DIE_STATE = "Die";
    
    
    //Action actuelle
    
    public string currentAction;
    // teste // 
    [SyncVar]public string _hitName = null;
    [SyncVar]public bool _hitCollider =  false;
    [SyncVar]public bool _isAttackIA = false;
    [SyncVar]private bool _isDead = false;  

    public bool isDead{get{return _isDead;}protected set {_isDead = value;}}

    [SyncVar]private float currentHealth;
    private float maxHealth = 100f;
    //
    
    private int kills,deaths;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    void Awake()
    {
        currentAction = STAND_STATE;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();      
    }
 
    // Update is called once per frame

    void Update()
    {
        if(!isServer)return;
         if(player == null)
        {
           ResetAnimation();
          
            currentAction = STAND_STATE;
           
        }
        else if(_hitCollider == true && _hitName != null)
        {
        _hitName=null;_isAttackIA=false;_hitCollider=false;
        }

        if (player != null)
        {
            if (MovingToTarget() && _hitCollider==false)
            {  
                //En train de marcher
                return;
            }
            //Sinon c'est qu'elle est à distance d'attaque  
    }
    }
 
    bool MovingToTarget()
    {
        //Assigne la destination : le joueur
        navMeshAgent.SetDestination(player.transform.position);
 
        //navMeshAgent pas prêt ?
        if (navMeshAgent.remainingDistance == 0)
            return false;
 
   
        // navMeshAgent.remainingDistance = distance restante pour atteindre la cible (Player)
        // navMeshAgent.stoppingDistance = à quel distance de la cible l'IA doit s'arrête 
        // (exemple 2 m pour le corps à sorps) 
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
 
            if (currentAction != WALK_STATE)
                Walk();
           
        }
        else
        {
            //Si arrivé à bonne distance, regarde vers le joueur
            RotateToTarget(player.transform);
            return false;
        }
        
 
        return true;
    }
    //Walk = Marcher
    void Walk()
    {
        //Réinitialise les paramètres de l'animator
        ResetAnimation();
        //L'action est maintenant "Walk"
        currentAction = WALK_STATE;
        //Le paramètre "Walk" de l'animator = true
        animator.SetBool(WALK_STATE, true);
    }
 
    //Attack = Attaquer
    void Attack()
    {
        //Réinitialise les paramètres de l'animator
        ResetAnimation();
        _isAttackIA = true;
        //L'action est maintenant "Attack"
        currentAction = ATTACK_STATE;
        //Le paramètre "Attack" de l'animator = true
        animator.SetBool(ATTACK_STATE, true);
         StartCoroutine(resetAction(_isAttackIA)); 
    }
 
    private void ResetAnimation()
    {  
        animator.SetBool(WALK_STATE, false);
        animator.SetBool(ATTACK_STATE, false);
        animator.SetBool(STAND_STATE, false);
    }
 
 
    //Permet de tout le temps regarder en direction de la cible
    private void RotateToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
    }
    IEnumerator resetAction(bool action)
    {
        yield return new WaitForSeconds(2f);
      action = false;
    }

    IEnumerator resetAnim(string State)
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool(State,false);
    }


    public void _CmdMagic(GameObject target, float damage)
    {
        target.GetComponent<Player>().CmdMagic(target,damage);
    }


    public void RpcTakeDamage(float amount,string sourceId)
    {
        if(isDead)
        {
            return;
        }
        currentHealth -= amount;
        Debug.Log(transform.name + " a mtn" + currentHealth +  " PDV");

        if(currentHealth <=0)
        {
            Debug.Log("Mort");
            Die();
        }
    }


IEnumerator Respawn()
{
yield return new WaitForSeconds(5f);
animator.SetBool(DIE_STATE,false);
ResetAnimation();
player = null;
isDead = false;
currentHealth = 100f;
}
[Server]
public void Die()
{
        animator.SetBool(DIE_STATE,true);
        ResetAnimation();
        player = gameObject;
        isDead =true;
        
        Instantiate(loot,
        new Vector3(gameObject.transform.position.x,
        gameObject.transform.position.y,
        gameObject.transform.position.z)
        ,gameObject.transform.rotation);

       StartCoroutine(Respawn());
    
}

void OnCollisionEnter(Collision collision)
{
    if(collision.transform.tag == "Player")
    {
       _CmdMagic(collision.gameObject,50);
    }
}



}
