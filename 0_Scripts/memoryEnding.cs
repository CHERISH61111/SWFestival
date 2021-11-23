using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class memoryEnding : MonoBehaviour
{

    internal bool issuc = false;
    float hAxis;
    float vAxis;

    bool jDown;

    bool isJump;
    internal bool edown;
    public bool isEntered = false;
    Vector3 moveVec;

    Rigidbody rigid;

    public float speed ; 

    public AudioSource jaudio;
    // public AudioSource baudio;
    // public AudioSource waudio;

    public AudioSource getaudio;
    public AudioSource doaudio;    

    public GameObject doText;
    public GameObject doBefore;
    public GameObject doAfter;
    public GameManager manager;

    public Image image;

    internal bool isDead = false;
       void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        GetInput();
        Jump();
        Show();
       
        if (transform.position.y < -50)
        {
            manager.GameOver();

        }


    }

    

    void FixedUpdate()
    {
        if (!isDead)
        {
            hAxis = Input.GetAxisRaw("Horizontal");
            vAxis = Input.GetAxisRaw("Vertical");
            rigid.AddForce(new Vector3(hAxis, 0, vAxis), ForceMode.Impulse);
        }
       // hAxis = Input.GetAxisRaw("Horizontal");
       // vAxis = Input.GetAxisRaw("Vertical");
       // moveVec = new Vector3(hAxis, 0, vAxis).normalized; 

       //// rigid.AddForce(new Vector3(hAxis,0,vAxis),ForceMode.Impulse);
       // transform.position += moveVec * speed * Time.deltaTime;
    }

    

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButtonDown("Jump");
        edown = Input.GetButtonDown("Interaction");
    }

    
    void Jump()
    {
        if ( jDown && !isJump )
        {
            rigid.AddForce(Vector3.up * 30, ForceMode.Impulse);
            jaudio.Play();
            isJump = true;

        }

    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    
    }


    void OnTriggerStay(Collider other)
    {
        if (other.tag == "EndZone")
        {
       //     getaudio.Play();
            if(!issuc)
                doText.SetActive(true);
            isEntered = true;

        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "EndZone")
        {
         //   getaudio.Play();
            //   moveVec = new Vector3(0, 0, 0).normalized;
            //   speed = 0;

            doText.SetActive(false);
            isEntered = false;


        }

    }



    void Show()
    {
        if (isEntered && manager.isEntered && issuc == false)
        {
            doText.gameObject.SetActive(false);
            manager.uiDownTxt.gameObject.SetActive(true);
            manager.uiDownTxt.text = "항원 정보를 등록하려면 E 키를 눌러주세요.";
            if (edown)
            {
                manager.Amimg.gameObject.SetActive(true);
                manager.uiDownTxt.text = "기억 도감 등록이 완료되었습니다.";
                issuc = true;
                DataManager.isFirstMemoried = true;

                StartCoroutine(Warning());

            }
        }
        
    }

    IEnumerator Warning()
    {
        yield return new WaitForSeconds(5f);
        int i = 0;
        while (i<5)
        {
            image.color = new Color32(118, 4, 0, 150);
            Debug.Log("red");
            yield return new WaitForSeconds(0.5f);
            Debug.Log("white");
            image.color = new Color32(0, 0, 0, 150);
            yield return new WaitForSeconds(0.5f);
            i++;
        }

        SceneManager.LoadScene(9);

    }






}

    
    



