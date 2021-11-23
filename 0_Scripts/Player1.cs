using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public float speed;

    public Camera followCamera;
    public GameManager manager;

    public AudioSource jumpSound; //->jump 함수
    public AudioSource attackSound;
    public AudioSource eatSound;
    public AudioSource successSound;
    public int health;
    public int score;

    public int maxHealth;

    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;

    public GameObject[] virusSign;
    public GameObject BozoTvirusSign;

    public GameObject overWall;

    public GameObject redic;


    float hAxis;
    float vAxis;


    bool jDown;
    bool fDown;
    bool iDown;


    bool isJump;
    bool isBorder;
    bool isDamage;
    internal bool isDead = false;
    bool isDodge;
    bool isAttack = false;
    bool getItem = false;
    bool isEat;

    bool ui1 = false;

    int virusNum = 0;


    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    SkinnedMeshRenderer[] meshs;

    

    Animation animation;


    GameObject nearObject = null;  //플레이어 가까이에 있는 오브젝트 저장


    void Start()
    {

        animation.Play("idle");
    }

    

    public void Spawn()
    {
        int a3 = Random.Range(-50, 50);
        int a2 = Random.Range(-50, 50);
        int a = Random.Range(-50, 50);
        int b = Random.Range(-100, -50);
        int b2 = Random.Range(-100, -50);
        int b3 = Random.Range(-100, -50);
        int c = Random.Range(-100, -50);
        int c2 = Random.Range(-100, -50);
        int c3 = Random.Range(-100, -50);

        

        Rigidbody enemy1Rigid = Enemy1.GetComponent<Rigidbody>();
        enemy1Rigid.AddForce(new Vector3(a, b, c - 30).normalized * 100, ForceMode.Impulse);

        Rigidbody enemy2Rigid = Enemy2.GetComponent<Rigidbody>();
        enemy2Rigid.AddForce(new Vector3(a2, b2, c2 - 30).normalized * 100, ForceMode.Impulse);

        Rigidbody enemy3Rigid = Enemy3.GetComponent<Rigidbody>();
        enemy3Rigid.AddForce(new Vector3(a3, b3, c3 - 30).normalized * 100, ForceMode.Impulse);


    }





    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        animation = GetComponent<Animation>();


        //PlayerPrefs.SetInt("MaxScore", 112500);
    }


    void Update()
    {
        GetInput();
        Move();
        Turn();
        Attack();
        Jump();
        End();
        if (ui1)
            manager.uiUpTxt.text = "보조 T 세포에게 항원 정보를 성공적으로 알려주었습니다.";

    }
    
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
        iDown = Input.GetButtonDown("Interaction");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //대각선값도 루트2가 아니고 1로


        if (isDead || isAttack )     //!isFireReady
            moveVec = Vector3.zero;
        if (!isBorder)
            transform.position += moveVec * speed  * Time.deltaTime;
        

    }

    void Turn()
    {
        //1. 키보드에 의한 회전
        transform.LookAt(transform.position + moveVec); //나아가는 방향 바라보기        

    }

    void Jump()
    {
        if (jDown && !isJump && !isDead)
        {
            jumpSound.Play();   //사운드실행
            rigid.AddForce(Vector3.up * 40, ForceMode.Impulse);
            isJump = true;
        }
    }



    void Attack()
    {

        if (fDown && !isAttack && !isDead)
        {
            attackSound.Play();
            animation.Stop("idle");
            animation.Play("attack");
            isAttack = true;

            if (nearObject == null)                
                Invoke("AttackOut", 0.9f);

            else if (nearObject != null && nearObject.tag == "Enemy")
            {
                isEat = true;
                isDamage = true;

                Rigidbody deadEnemy = nearObject.GetComponent<Rigidbody>();
                deadEnemy.isKinematic = true;
                Invoke("Eat", 0.5f);
                Destroy(nearObject,0.5f);

            }
        }
        
    }
    void AttackOut()
    {
        isAttack = false;
        animation.Play("idle");
    }
    void Eat()
    {
        
        eatSound.Play();
        isAttack = false;
        manager.enemyCntA--;
        Invoke("idle", 0.4f);
        virusSign[virusNum++].SetActive(true);
        if(DataManager.isFirstMemoried == true && virusNum==1 && DataManager.isFirstCleared == false)
        {
            redic.SetActive(true);
            manager.bookUi.gameObject.SetActive(false);
            
        }
        
        isEat = false;
        isDamage = false;
        if (virusNum == 3)
            overWall.gameObject.SetActive(false);
        
    }

    void End()
    {
        
    }

    void idle()
    {
        animation.Play("idle");
    }

    


    //void attack()
    //{
    //    if (equipweapon == null)
    //        return;

    //    firedelay += time.deltatime;
    //    isfireready = equipweapon.rate < firedelay;

    //    if (fdown && isfireready && !isdodge && !isswap && !isdead)
    //    {
    //        equipweapon.use();
    //        anim.settrigger(equipweapon.type == weapon.type.melee ? "doswing" : "doshot");
    //        if (equipweapon.type == weapon.type.range)
    //            gunsound.play();
    //        firedelay = 0;
    //    }

    //}



  

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;   //계속 회전속도 0으로 만듦
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 10, Color.green);
        isBorder = Physics.Raycast(transform.position, moveVec, 10, LayerMask.GetMask("Wall"));
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
            isJump = false;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            rigid.velocity = Vector3.zero;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            Destroy(other.gameObject);

        }
        else if (other.tag == "EnemyBullet" && !isEat)
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
        foreach(SkinnedMeshRenderer mesh in meshs)
        {
            mesh.material.color = new Color32(191, 120, 133, 255);
        }
        if (health <= 0 && !isDead)
        {
            OnDie();
        }

        yield return new WaitForSeconds(1f);

        isDamage = false;
        foreach (SkinnedMeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white; //new Color32(223, 241, 255, 255);
        }
        
        
    }
     
    void OnDie()
    {
        isDead = true;
        manager.GameOver();
    }


    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            nearObject = other.gameObject;
            stage1Enemy enemy = other.gameObject.GetComponent<stage1Enemy>();
            enemy.bubble.SetActive(true);
        }
        else if (other.tag == "BozoT")
        {
            manager.uiUpTxt.gameObject.SetActive(true);
            manager.uiUpTxt.text = "E 키를 눌러 보조 T 세포에게 항원 정보를 알려주세요.";

            if (iDown)
            {
                BozoTvirusSign.gameObject.SetActive(true);
                virusSign[0].gameObject.SetActive(false);
                virusSign[1].gameObject.SetActive(false);
                virusSign[2].gameObject.SetActive(false);
                ui1 = true;
                successSound.Play();
                Invoke("GameClear", 3f);
                manager.isBattle = false;
            }
            
        }
      

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            nearObject = null;
            stage1Enemy enemy = other.gameObject.GetComponent<stage1Enemy>();
            enemy.bubble.SetActive(false);
        }
        else if (other.tag == "BozoT")
        {
            manager.uiUpTxt.gameObject.SetActive(false);
        }

    }

    void TimeOver()
    {

        manager.GameOver();
    }

    void GameClear()
    {
        manager.StageEnd();
    }
}

