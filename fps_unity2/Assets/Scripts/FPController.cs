using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{
    public GameObject cam; //public exposes object to inspector, drag an drop camera over there
    public Animator anim;   //public exposes object to inspector, drag and drop animation controller over there
    public AudioSource[] footsteps;
    public AudioSource jump;
    public AudioSource land;
    public AudioSource ammopickup;
    public AudioSource medpickup;
    public AudioSource trigger; 
    public AudioSource reload;
    float xSensitivity = 2f;
    float ySensitivity = 2f;
    float MinimumX = -90f;
    float MaximumX = 90;
    float x;
    float y;

    int ammo = 0;
    int maxAmmo = 50;

    int health = 0;
    int maxHealth = 100;

    int ammoClip= 0;
    int ammoClipMax = 10;

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

        health=maxHealth;
        
        
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
            jump.Play();
            land.Play();    //this doesn't sound realistic enough


        }
        
        float x = Input.GetAxis("Horizontal")*0.09f;   //input code should stay in update method, not fixedupdate
        float z = Input.GetAxis("Vertical")*0.09f;
        transform.position += cam.transform.forward * z + cam.transform.right * x; //new Vector3(x * speed, 0, z * speed);


        if(Input.GetKeyDown(KeyCode.F)){
            anim.SetBool("arm", !anim.GetBool("arm"));
        }
        if(Input.GetMouseButtonDown(0)){
            if(ammoClip>0){
                anim.SetTrigger("fire");
                ammoClip--;
                Debug.Log("Ammo clip left = " + ammoClip);
            }else if(anim.GetBool("arm")){
                trigger.Play();    
                Debug.Log("Empty Clip");            

            }
            

            
            //shot.Play();
        }
        if(Input.GetKeyDown(KeyCode.R)){

            if(ammo>0){
                int ammoNeeded = ammoClipMax-ammoClip;
                anim.SetTrigger("reload");
                ammoClip=Mathf.Clamp(ammoClip+ammoNeeded,0,ammoClipMax);
                reload.Play();
            }
        }

        if(Mathf.Abs(x)>0||Mathf.Abs(z)>0){
            if(!anim.GetBool("walking"))    {
                anim.SetBool("walking", true);
                InvokeRepeating("PlayFootstepsAudio", 0, 0.4f);       //invoke doesnt work
                //PlayFootstepsAudio();   //please make invoking work
            }
            anim.SetBool("walking", true);

        } else if(anim.GetBool("walking")){
            anim.SetBool("walking", false);
            CancelInvoke("PlayFootstepsAudio");          //fix this
        }



    }

        void OnCollisionEnter(Collision col) {
            if(col.gameObject.tag=="Ammo" && ammo<maxAmmo){        //ammo  pickup
                ammo=Mathf.Clamp(ammo+10,0,maxAmmo);//checks or makes  it so that if ammo>maxammo cant update anymore
                Debug.Log("Ammo = "+ ammo);
                Destroy(col.gameObject);
                ammopickup.Play();
                }



            if(col.gameObject.tag=="MedBox" && health<maxHealth){                   //med health pickup
                health = Mathf.Clamp(health+20,0,maxHealth);
                Debug.Log("health = "+ health);
                Destroy(col.gameObject);
                medpickup.Play();
                }

            if(col.gameObject.tag=="Lava"){
                health=Mathf.Clamp(health-50,0,maxHealth);
                Debug.Log("Lava, health is " + health);
            }

            
        }

        void PlayFootstepsAudio(){
            AudioSource audioSource = new AudioSource();
            int n = Random.Range(0, footsteps.Length-1);

            audioSource = footsteps[n];
            audioSource.Play();
            footsteps[n] = footsteps[0];
            footsteps[0] = audioSource;

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
