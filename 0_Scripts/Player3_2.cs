using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player3_2 : MonoBehaviour
{
    public float speed;
    public int hasGrenades;
    public GameObject grenadeObj;
    public Camera followCamera;
    public GameManager manager;

    public AudioSource dodgeSound; //->jump 함수
    public AudioSource hangcheSound;

    public Text paneltxt;
    public Text panelmiddletxt;
    public Text stagePanalText;


    public int health;
    public int score;

    public int maxHealth;
    public int maxHasGrenades;

    float hAxis;
    float vAxis;

    bool wDown;
    //bool jDown;
    bool dDown;
    bool iDown;
    bool gDown;
    bool rDown;


    bool isJump;
    bool isDodge;
    bool isBorder;
    bool isDamage;
    internal bool isDead;
    internal bool isGrenade = true;
    bool cantMove;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;
    MeshRenderer[] meshs;




    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();

        if(DataManager.isFirstMemoried == true && DataManager.isFirstCleared == false)
        {
            paneltxt.text = "2차 면역 반응";
            stagePanalText.text = "2차 면역 반응";
            panelmiddletxt.text = "\n\n\n\n\n\n2차 면역 반응에서는 1차 면역 반응 중에 생성된\n\n기억세포가 형질세포로 빠르게 분화합니다 \n\n" +
                "분화된 형질세포는 항원-항체 반응을 훨씬 신속하게 진행하여\n\n항원의 재침입에 효율적으로 대처합니다 \n ";

            hasGrenades = 999;
            maxHasGrenades = 999;

        }

        //Debug.Log(PlayerPrefs.GetInt("MaxScore"));
        //PlayerPrefs.SetInt("MaxScore", 112500);
    }


    void Update()
    {
        GetInput();
        Move();
        Turn();
        //Jump();
        Grenade();
        Dodge();

    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        //jDown = Input.GetButtonDown("Dodge");
        dDown = Input.GetButtonDown("Jump");
        gDown = Input.GetButtonDown("Fire1");
        iDown = Input.GetButtonDown("Interaction");


    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //대각선값도 루트2가 아니고 1로

        if (cantMove)
            moveVec = dodgeVec;
       
        if (isDead)     //!isFireReady
            moveVec = Vector3.zero;

        if (!isBorder)
            transform.position += moveVec * speed * (wDown ? 0.3f : 1) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

    }

    void Turn()
    {
        //1. 키보드에 의한 회전
        transform.LookAt(transform.position + moveVec); //나아가는 방향 바라보기

        //2. 마우스에 의한 회전
        /*
         if (gDown && !isDead)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
         */



    }


    /*
     void Jump()
    {
        if (jDown && !isJump &&  !isDead)
        {
            jumpSound.Play();   //사운드실행
            rigid.AddForce(Vector3.up * 20, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;

            
        }

    }
     */


    void Grenade()
    {
        if (hasGrenades == 0)
            return;

        if(gDown && !isDead && !isGrenade)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                hangcheSound.Play();
                cantMove = true;
                isGrenade = true;
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;

                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
                //rigidGrenade.AddTorque(Vector3.up * 10, ForceMode.Impulse);

                hasGrenades--;
                //grenades[hasGrenades].SetActive(false); //4개
                Destroy(instantGrenade, 4f);
                cantMove = false;
                if (DataManager.isFirstMemoried == true && DataManager.isFirstCleared == false)
                    Invoke("GrenadeOut2", 0f);
                else
                    Invoke("GrenadeOut", 1);
            }
        }
    }

    void GrenadeOut()
    {
        isGrenade = false;
    }
    void GrenadeOut2()
    {
        isGrenade = false;
    }

    



    void Dodge()
    {
        if (dDown && !isDodge && !isDead)
        {
            dodgeSound.Play();
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);   //함수로 시간차함수 호출. "함수이름",시간차
            Invoke("DodgeOut2", 1);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
    }

    void DodgeOut2()
    {
        isDodge = false;
    }


    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;   //계속 회전속도 0으로 만듦
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, moveVec, 5, LayerMask.GetMask("Wall"));
    }

    
    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    hasGrenades += item.value;
                    if (hasGrenades > maxHasGrenades)
                        hasGrenades = maxHasGrenades;
                    break;
            }
            Destroy(other.gameObject);
        }
        else if(other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;
                if (other.GetComponent<Rigidbody>() != null)
                    Destroy(other.gameObject);
                StartCoroutine(OnDamage());
            }
            
        }
    }

    IEnumerator OnDamage()
    {
        isDamage = true;
        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.red;
        }
        if (health <= 0 && !isDead)
        {
            OnDie();
        }

        yield return new WaitForSeconds(1f);

        isDamage = false;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white; //new Color32(223, 241, 255, 255);
        }
        
        
    }
     
    void OnDie()
    {
        anim.SetTrigger("doDie");
        isDead = true;
        manager.GameOver();
    }



}

