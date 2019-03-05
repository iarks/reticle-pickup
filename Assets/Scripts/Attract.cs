using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attract : MonoBehaviour
{
    public float pullRadius = 2;
    public float pullForce = 1;
    public Rigidbody rig;

    public void FixedUpdate()
    {
       
            // calculate direction from target to me
            Vector3 forceDirection = transform.position - rig.gameObject.transform.position;

            // apply force on target towards me
            rig.AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
        
    }

}
