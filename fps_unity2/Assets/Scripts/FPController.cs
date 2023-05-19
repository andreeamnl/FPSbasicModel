using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{
    public GameObject cam; //public exposes object to inspector, drag an drop camera over there
    float xSensitivity = 2f;
    float ySensitivity = 2f;
    float MinimumX = -90f;
    float MaximumX = 90;
    Rigidbody rb;
    CapsuleCollider capsule;

    Quaternion cameraRot;
    Quaternion charachterRot;


    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        capsule=this.GetComponent<CapsuleCollider>();

        cameraRot=cam.transform.localRotation;
        charachterRot=this.transform.localRotation;   //take rotations


        
        
    }

    // Update is called once per frame
    void Update()
    {
        float yRot= Input.GetAxis("Mouse X")*xSensitivity;   //when mouse moves left to right   around y axis
        float xRot=Input.GetAxis("Mouse Y")*ySensitivity;   //when mouse moves up and down around x axis

        cameraRot *= Quaternion.Euler(-xRot, 0, 0);     //update camera rot
        charachterRot *= Quaternion.Euler(0,yRot,0);

        this.transform.localRotation=charachterRot;     //update actual position using camera rot
        cam.transform.localRotation=cameraRot;

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded()){
            rb.AddForce(0,300,0);
        }
        
        float x = Input.GetAxis("Horizontal")*0.3f;   //input code should stay in update method, not fixedupdate
        float z = Input.GetAxis("Vertical")*0.3f;
        transform.position += cam.transform.forward * z + cam.transform.right * x; //new Vector3(x * speed, 0, z * speed);

    }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    bool isGrounded(){
        RaycastHit hitinfo;
        if(Physics.SphereCast(transform.position,capsule.radius,Vector3.down,out hitinfo,(capsule.height/2f)-capsule.radius+0.1f)){   //spherecast detects whether a collison has occured,in this case the "sphere" and the ground
            return true;
        }
        return false;
    }
}