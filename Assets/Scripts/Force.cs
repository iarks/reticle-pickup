using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{

    public Rigidbody rigidbodyCmp;
    private float weightScale;
    private float controlTension = 100f;
    private GameObject grabber;

    private void Awake()
    {
        grabber = this.gameObject;
        grabber.transform.SetPositionAndRotation(rigidbodyCmp.gameObject.transform.position, rigidbodyCmp.gameObject.transform.rotation);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 controlTransformPosition = rigidbodyCmp.gameObject.transform.position;

        Vector3 targetToControl = this.transform.position - controlTransformPosition;

        //controlTension = Mathf.Clamp01((10.0f - 0.5f) / (targetToControl.magnitude - 0.5f + 0.0001f));


        weightScale = Mathf.Clamp((15f / rigidbodyCmp.mass), 1f, 15f);

        //RotateRigidbody();
        MoveRigidbody();
    }


    // Sets the angular velocity of the rigidbody.
    private void RotateRigidbody()
    {
        float angle;
        Vector3 axis;
        // Get the delta between the control transform rotation and the rigidbody.
        Quaternion rigidbodyRotationDelta = this.transform.rotation *
                                            Quaternion.Inverse(rigidbodyCmp.rotation);
        // Convert this rotation delta to values that can be assigned to rigidbody
        // angular velocity.
        rigidbodyRotationDelta.ToAngleAxis(out angle, out axis);
        // Set the angular velocity of the rigidbody so it rotates towards the
        // control transform.
        float timeStep = Mathf.Clamp01(Time.fixedDeltaTime * weightScale * controlTension);
        rigidbodyCmp.angularVelocity = timeStep * angle * axis;
    }

    private void MoveRigidbody()
    {

        // Get the vector from the control transform to the rigidbody.
        Vector3 forceDirection = this.transform.position - rigidbodyCmp.position;
        Vector3 normalizedForce = forceDirection.normalized;
        float distanceFromControlTransform = forceDirection.magnitude;

        // Normalize the rigidbody velocity when it is more than one unit from the
        // target.
        if (distanceFromControlTransform > 1.0f)
        {
            forceDirection = normalizedForce;
            // Otherwise, scale it by the distance to the target.
        }
        else
        {
            forceDirection = forceDirection * distanceFromControlTransform;
        }

        // Set the desired max velocity for the rigidbody.
        Vector3 targetVelocity = forceDirection * weightScale;

        // Have the rigidbody accelerate until it reaches target velocity.
        float timeStep = Mathf.Clamp01(Time.fixedDeltaTime * controlTension * 8.0f);
        rigidbodyCmp.velocity += timeStep * (targetVelocity - rigidbodyCmp.velocity);
    }
}
