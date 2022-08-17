using System.Collections;
using UnityEngine;
public class SwordL : MonoBehaviour
{
   public const int _bulletMax = 30;
   public int _bulletCurrent;
    private Player _player;
    private Camera _cam;
    private Vector3 destination;
    public float projectileSpeed;
    public GameObject projectile;
    public Transform Obj_projectile;

    void Start()
    {
        gameObject.transform.parent.tag = "WeaponRange";
        _player = transform.root.GetComponent<Player>();
        _cam = this.transform.root.GetChild(4).GetComponent<Camera>();
    }   
    void Update()
    {
        if(_player._isAttack==true)
        {
            FireClic();
            _player._isAttack = false;
        }
        
    }
    private void FireClic()
    {
        
        if(_player._States == "Shoot" && _bulletCurrent>=1) 
        {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0f,0f,0));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
         destination = hit.point;   
        }
        else
        {
        destination = ray.GetPoint(1000);  
        }
            InstantiateProjectile(Obj_projectile);
    
       } 
       else if(_bulletCurrent<=1)
       {

       }
        
       

    }

  private void InstantiateProjectile(Transform firePoint)
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<Rigidbody>().velocity = (destination = firePoint.position).normalized*projectileSpeed;
    }
   
    
}