using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    float speed = 0.5f;
    Rigidbody rb;
    CapsuleCollider capsule;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        capsule=this.GetComponent<CapsuleCollider>();

        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded()){
            rb.AddForce(0,300,0);
        }
        
        float x = Input.GetAxis("Horizontal");   //input code should stay in update method, not fixedupdate
        float z = Input.GetAxis("Vertical");
        transform.position += new Vector3(x*speed,0,z*speed);   //these lines i've seen being used in fixedupdate()

    }

    bool isGrounded(){
        RaycastHit hitinfo;
        if(Physics.SphereCast(transform.position,capsule.radius,Vector3.down,out hitinfo,(capsule.height/2f)-capsule.radius+0.1f)){   //spherecast detects whether a collison has occured,in this case the "sphere" and the ground
            return true;
        }
        return false;
    }
}
