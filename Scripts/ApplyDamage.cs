
using UnityEngine;
using Mirror;
public class ApplyDamage : NetworkBehaviour
{

    [SerializeField]private NetworkAnimator _netAnim;

    public void MyTriggerOn(string iDanim)
    {
        if(isLocalPlayer)
        {
            _netAnim.SetTrigger(iDanim);
        }
    }


}
