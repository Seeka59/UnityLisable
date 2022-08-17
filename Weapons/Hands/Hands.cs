using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    public Animator _animator;
   
    void Start()
    {
        _animator.SetBool("WeaponRange",false);
        _animator.SetBool("WeaponMelee",false);
    }
}
