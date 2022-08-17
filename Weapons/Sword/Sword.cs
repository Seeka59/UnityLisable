using System.Collections;
using UnityEngine;
using Mirror;
public class Sword : MonoBehaviour
{
    // Warning MonoBehaviour . 
    private Collider m_Collider;
    [SerializeField]private Player _player;
    public GameObject WeaponMiss;
    public bool hitPlayer;
    public string namePlayer;
    public float damagePlayer;
    private float _lvlWeapon = 0;
    public float _damage = 10;
    //NetworkIdentity _NI;
    void Start()
    {
        //_NI = transform.root.GetComponent<NetworkIdentity>();
        //if(!isLocalPlayer){return;}
        _player = transform.root.GetComponent<Player>();
        m_Collider = GetComponent<Collider>();
        m_Collider.enabled = false;
       
        gameObject.transform.parent.tag = "WeaponMelee";
        
        Debug.Log(" Collider = " + m_Collider.enabled );

        Debug.Log("Sword Test name root = " + gameObject.transform.root.name);

        Debug.Log("Sword Test _player attack = " + _player.isAttack);

    }   

int[] array = { 1, 2, 3, 4, 5, 6 };
private int _i = 0;

    private void Update()
    {
        //if(!isLocalPlayer){return;}
        TesteA();
        lvlWeapon();
    }

private void TesteA()
{   
    //if(!isLocalPlayer){return;}
    if(_player._States == null){m_Collider.enabled = false;return;}
    if(
    _player._States == "Droite" && transform.parent.name=="SlotWeapon"
    ||
     _player._States=="Gauche" && transform.parent.name=="SlotWeapon1"
     )
    {
        if(_player._isAttack == true && m_Collider.enabled == false)
        {
         m_Collider.enabled = true;
         Debug.Log("Update _player attack = " + _player.isAttack);
        }else if (_player._isAttack == false){
            m_Collider.enabled = false;
        } 
    }   
}

    private void lvlWeapon ()
    {
        if(_lvlWeapon == array[_i])
        {
            _damage = _damage + 1;
            _i++;
        }
    }

   void OnTriggerEnter(Collider other)
    {
        if(other.transform.root == transform.root){return;}
        if(m_Collider.enabled == true)
        {
         if(other.gameObject.tag == "Player")
         {
          _player.CmdMagic(other.gameObject,50);
         }
         if(other.gameObject.tag=="Enemy")
         {
            
         }
        }
        
    }  
}