using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public int itemCount;

    public Camera followCamera;

    float hAxis;
    float vAxis;

    bool jDown;
    bool dDown;
 
    bool fDown;

    bool isJump;
    bool isDodge;

    Vector3 moveVec;

    Rigidbody rigid;

    public AudioSource jaudio;
    // public AudioSource baudio;
    // public AudioSource waudio;
    public AudioSource itemaudio;


    GameObject nearObject; 


   
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        GetInput();
        Move();
        Jump();
     
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButtonDown("Jump");
        dDown = Input.GetButtonDown("Dodge");
        fDown = Input.GetButton("Fire1");
    }

    
    
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        transform.position += moveVec * 20 * Time.deltaTime;

    }

    
    void Jump()
    {
        if ( jDown && !isJump )
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            jaudio.Play();
            isJump = true;
        }

    }


    // void FreezeRotation()
    // {
    //     rigid.angularVelocity = Vector3.zero; 
    // }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }
        
   
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            itemCount++;
            itemaudio.Play();
            other.gameObject.SetActive(false);
     //       manager.GetItem(itemCount);
        }
        else if(other.tag == "Point")
        {
            // if(itemCount == manager.TotalItemCount)
            // {
            //     if (manager.stage == 2)
            //         SceneManager.LoadScene(0);
            //     else
            //         SceneManager.LoadScene("Example1_" + (manager.stage + 1));

            }


            // else
            //     SceneManager.LoadScene("Example1_"+ manager.stage );


        }
      
            
        
    }
    
    



