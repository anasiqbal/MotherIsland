using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Range(1.0f, 15.0f)] public float TargetRadius;
    [Range(20.0f, 75.0f)] public float LaunchAngle;
    [Range(0.0f, 10.0f)] public float TargetHeightOffsetFromGround;
    public bool RandomizeHeightOffset;

    private bool bTargetReady;
    private bool bTouchingGround;

    public Rigidbody rigid;
    
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 TargetObjectTF;
    
    
    public void configureProjectile(Vector3 _targetPosition)
    {
        TargetObjectTF = _targetPosition;
        bTargetReady = true;
        bTouchingGround = false;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        gameObject.SetActive(true);
    }

	void Update ()
    {
        if (bTargetReady)
        {
            Launch();
        }
         
        if (!bTouchingGround && !bTargetReady)
        {
            transform.rotation = Quaternion.LookRotation(rigid.velocity) * initialRotation;
        }
	}

    void OnCollisionEnter()
    {
        bTouchingGround = true;
        gameObject.SetActive(false);
        Destroy(this.gameObject,1.5f);
    }


    void Launch()
    {
        Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 targetXZPos = new Vector3(TargetObjectTF.x, 0.0f, TargetObjectTF.z);
        
        transform.LookAt(targetXZPos);

        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = (TargetObjectTF.y) - transform.position.y;

        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)) );
        float Vy = tanAlpha * Vz;

        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        rigid.velocity = globalVelocity;
        bTargetReady = false;
    }
    
}
