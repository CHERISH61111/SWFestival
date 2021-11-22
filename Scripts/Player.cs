using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public GameObject[] weapons;
    public bool[] hasWeapons;
    public int hasAmmo;
    public Camera followCamera;

    public int ammo;
    public int coin;
    public int health;
    public int score;

    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;


    float hAxis;
    float vAxis;

    bool wDown;
    bool jDown;
    bool dDown;
    bool iDown;
    bool fDown;
    bool rDown;

    bool isJump;
    bool isDodge;
    bool isReload;
    bool isFireReady = true;
    bool isBorder;
    bool isDamage = false;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;
    MeshRenderer[] meshs;

    public GameObject randEnemy;
    public GameObject randAmmo;
    public Transform[] createEnemy;

    public AudioSource jaudio;
    public AudioSource baudio;
    public AudioSource waudio;

    public Bullet enemybullet;

    public Weapon equipWeapon;

    GameObject nearObject;  //�÷��̾� �����̿� �ִ� ������Ʈ ����

    float fireDelay;


    public GameObject enemy;


    void Start()
    {
        createVirus();
        createAmmo();

    }
    void createVirus()
    {
        InvokeRepeating("aTime", 2, 3);
        Invoke("stopInvokeV", 62);
        Debug.Log("createVirusStart");
    }

    void createAmmo(){
        InvokeRepeating("bTime", 5, 15);
        Invoke("stopInvokeA", 65);
        Debug.Log("createAmmoStart");
    }

    void stopInvokeV()
    {
        CancelInvoke("aTime");
    }

    void stopInvokeA(){
        CancelInvoke("bTime");
    }

    void aTime()
    {

        int ranZone = Random.Range(0,4);
        GameObject createdEnemy = Instantiate(randEnemy, createEnemy[ranZone].position,
                                                createEnemy[ranZone].rotation);
        Enemy enemy = createdEnemy.GetComponent<Enemy>();
        enemy.target = transform;

    }

   void bTime()
    {
        int ranZone = Random.Range(0,4);
        GameObject createdAmmo = Instantiate(randAmmo, createEnemy[ranZone].position,
                                                createEnemy[ranZone].rotation);
        Enemy enemy = createdAmmo.GetComponent<Enemy>();
        Destroy(createdAmmo, 10);


    }


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();
            
    }


    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
        Reload();
        Dodge();
        Interaction();
     
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        dDown = Input.GetButtonDown("Dodge");
        fDown = Input.GetButton("Fire1");
        rDown = Input.GetButtonDown("Reload");
        iDown = Input.GetButtonDown("Interaction");
    }

    
    
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //�밢������ ��Ʈ2�� �ƴϰ� 1��

        if (isDodge)
            moveVec = dodgeVec;

        if (isReload || !isFireReady)
            moveVec = Vector3.zero;

        if (!isBorder)
            transform.position += moveVec * speed * (wDown ? 0.3f : 1) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

    }

    void Turn()
    {

        transform.LookAt(transform.position + moveVec); 

        if (fDown)
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
        

    }

    void Jump()
    {
        if ( jDown && !isJump )
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            jaudio.Play();
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }

    }

    
    void Attack()
    {
        if (equipWeapon == null)
            return;
        
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isDodge)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            baudio.Play();
            fireDelay = -0.1f;
        }

    }
     
    void Reload()
    {
        if (equipWeapon == null)
            return;

        if (equipWeapon.type == Weapon.Type.Melee)
            return;

        if (ammo == 0)
            return;

        if(rDown && !isJump && !isDodge && isFireReady && !isReload)
        {
            anim.SetTrigger("doReload");
            hasAmmo --;
            isReload = true;
            Invoke("ReloadOut",0.5f);
        }
    }

    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        //hasAmmo --;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }

    void Dodge()
    {
        if (dDown && !isDodge )
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);   //�Լ��� �ð����Լ� ȣ��. "�Լ��̸�",�ð���
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }


    void Interaction()
    {
        if (iDown && nearObject != null && !isJump && !isDodge)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                hasWeapons[2] = true;
                equipWeapon = weapons[2].GetComponent<Weapon>();
                equipWeapon.gameObject.SetActive(true);

                waudio.Play();

                Destroy(nearObject);
            }

        }
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;   //��� ȸ���ӵ� 0���� ����
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 3, Color.green);
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
        
        if (collision.gameObject.tag == "Enemy")
        {
            
            health -= enemybullet.damage;

            StartCoroutine(OnDamage());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    waudio.Play();
                    hasAmmo ++;
                    break;
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "EnemyBullet"){
            if(!isDamage){
                
         //       Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= 20;
            
            if(other.GetComponent<Rigidbody>() != null){
                

                Destroy(other.gameObject);
            }

            StartCoroutine(OnDamage());
            }
            
        }
    }
    
    

    IEnumerator OnDamage(){
        
        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.red;
        }
        yield return new WaitForSeconds(1f);

        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.white;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = other.gameObject;

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }

}

