using Mirror;
using UnityEngine;

using UnityEngine.InputSystem;

   namespace StarterAssets
{
    

	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
public class PlayerProjectile : NetworkBehaviour
{ 
    private StarterAssetsInputs _input;
    private Animator m_animator;
    public GameObject projectile;
    public Camera cam;
    public Transform LHFirePoint, RHFirePoint;
    private Vector3 destination;
    private bool leftHand;
    public float projectileSpeed = 30f;
    void Start()
    {

    }
    void Update()
    {
    if(_input.Fire)
     {ShootProjectile();}
     _input.Fire = false;
    }
    


      private void ShootProjectile()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        destination = hit.point;
        else
        destination = ray.GetPoint(1000);
        if(leftHand){
            leftHand = false;
            InstantiateProjectile(LHFirePoint);
        }
        else{
            leftHand = true;
            InstantiateProjectile(RHFirePoint);

        }
    }
    void InstantiateProjectile(Transform firePoint)
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<Rigidbody>().velocity = (destination = firePoint.position).normalized*projectileSpeed;
    }
}}