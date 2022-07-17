using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistol : Weapon
{
    public override void shootIsDone()
    {
        isShooting = false;
        fire.SetActive(false);
        _animator.SetBool("isShooting", false);
    } 
    public override void Shake()
    {
        transform.position -= transform.right * 0.3f;
    }
}
