using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public int Reditemforce;
    public int Yellowitemforce;
    public int Purpleitemforce;

    public GameManager manager;

    public GameObject virusSign;

    public AudioSource successSound;
    // public float speed;



    float hAxis;
    float vAxis;

    bool jDown;
    bool iDown;
    bool isJump;
    bool isChange;
    internal bool isDead = false;
    bool ui1 = false;

    Vector3 moveVec;

    Rigidbody rigid;

    public AudioSource jaudio;
    // public AudioSource baudio;
    // public AudioSource waudio;

    
    public AudioSource itemaudio;
    public AudioSource flooraudio;


       void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        GetInput();
        Jump();
        if (ui1)
            manager.uiUpTxt.text = "세포 독성 T 세포에게 항원 정보를 성공적으로 전달하였습니다.";
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            hAxis = Input.GetAxisRaw("Horizontal");
            vAxis = Input.GetAxisRaw("Vertical");
            rigid.AddForce(new Vector3(hAxis, 0, vAxis), ForceMode.Impulse);
        }
        
    }

    

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interaction");
    }

    
    void Jump()
    {
        if ( jDown && !isJump && !isChange && !isDead )
        {
            rigid.AddForce(Vector3.up * 20, ForceMode.Impulse);
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
            flooraudio.Play();
        }
        else if (collision.gameObject.tag == "FakeFloor"){
            isJump = false;

            Destroy(collision.gameObject, 0.5f);
            flooraudio.Play();
            

        }

    }

    

   
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            itemaudio.Play();
            other.gameObject.SetActive(false);
        }
        else if(other.tag == "RedItem"){
            isChange = true;
            rigid.velocity = Vector3.forward*0 ;
            rigid.AddForce(Vector3.forward * Reditemforce, ForceMode.Impulse);
         
            other.gameObject.SetActive(false);
            Invoke("ChangeOut", 0f); 

        }
        else if(other.tag == "YellowItem"){
            
            isChange = true;
            rigid.velocity = Vector3.up*0;
            rigid.AddForce(Vector3.up * Yellowitemforce, ForceMode.Impulse);

            other.gameObject.SetActive(false);
            Invoke("ChangeOut", 0.5f); 

        }

        else if(other.tag == "PurpleItem"){
            isChange = true;
            rigid.velocity = Vector3.down*0;
            rigid.AddForce(Vector3.down * Purpleitemforce, ForceMode.Impulse);
            
            other.gameObject.SetActive(false);
            Invoke("ChangeOut", 0f); 

        }
        
        if (other.tag == "DieFloor")
        {
            manager.GameOver();
        }

        

        itemaudio.Play();

    }

    void OnTriggerStay(Collider other)
    {

        if (other.tag == "BozoT")
        {
            manager.uiUpTxt.gameObject.SetActive(true);
            manager.uiUpTxt.text = "E 키를 눌러 세포 독성 T 세포에게 항원 정보를 전달하세요.";

            if (iDown)
            {
                virusSign.gameObject.SetActive(false);
                ui1 = true;
                successSound.Play();
                Invoke("GameClear", 3f);
                manager.isBattle = false;
            }

        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "BozoT")
        {
            manager.uiUpTxt.gameObject.SetActive(false);
        }

    }

    void ChangeOut()
    {
        isChange = false;

    }

    void GameClear()
    {
        manager.StageEnd();
    }


}
    
    



