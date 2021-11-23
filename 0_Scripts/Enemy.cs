using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C };
    public Type enemyType;

    public int maxHealth;
    public int curHealth;
    public int score;
    public GameManager manager;
    public Transform target;
    public BoxCollider meleeArea;
    public GameObject bullet;
    public GameObject[] coins;
    public GameObject GaText;

    public bool isChase;
    public bool isAttack;
    public bool isDead;

    Rigidbody rigid;
    BoxCollider boxCollider;
    SphereCollider sphCollider;
    Material mat;

    NavMeshAgent nav;
    Animator anim;


    public AudioSource aimaudio;
    public AudioSource dieaudio;
    void Awake()
    {

        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        sphCollider = GetComponent<SphereCollider>();

        mat = GetComponentInChildren<MeshRenderer>().material;
        
        anim = GetComponentInChildren<Animator>();
        if (enemyType != Type.A)
        {
            nav = GetComponent<NavMeshAgent>();
            Invoke("ChaseStart", 2);
        }
        if (enemyType == Type.C)
        {
            
            Invoke("canAttack", 2);
        }

    }

    void canAttack()
    {
        gameObject.layer = 11;
    }
    void ChaseStart()
    {

        isChase = true;
        anim.SetBool("isWalk", true);
        
        
    }
    void Update()
    {
        if (enemyType != Type.A && nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;

        }

        

    }

    void FreezeVelocity()
    {
;
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;   //계속 회전속도 0으로 만듦
        }
        
    }

    void Targeting()
    {

        if (!isDead)
        {
            float targetRadius = 0;
            float targetRange = 0f;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 6f;
                    break;
                case Type.B:
                    targetRadius = 1.5f;
                    targetRange = 9f;
                    break;
                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
            }
            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                                                       targetRadius,
                                                       transform.forward,
                                                       targetRange,
                                                       LayerMask.GetMask("Player"));
            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }

       
        
    }

    IEnumerator Attack()
    {

        isChase = false; //몬스ㅓ터 정지
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch (enemyType)
        {
            case Type.A:
                break;
            case Type.B:
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;  
            case Type.C:
                yield return new WaitForSeconds(0.2f);
                
                yield return new WaitForSeconds(1f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);//-Vector3.right*2

                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                //yield return new WaitForSeconds(1f);    
                rigidBullet.velocity = transform.forward * 30;  //앞으로 나가게하기.AddForce도 가능

                break; 
        }

        
        isChase = true; //몬스ㅓ터 이동
        isAttack = false;
        anim.SetBool("isAttack", false);
    }


    void FixedUpdate()
    {
        switch (enemyType)
        {
            case Type.B:
            case Type.C:
                Targeting();
                FreezeVelocity();
                break;
        }
    }


    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            
            StartCoroutine(OnDamage(reactVec, false));


        }
        else if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;

            switch (enemyType)
            {
                case Type.C:
                    aimaudio.Play();
                    break;
            }

            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);

            StartCoroutine(OnDamage(reactVec, false));
        }
    }
    public void HitByGrenade(Vector3 explosionPos)
    {

        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, false));
    }
    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {

        mat.color = Color.red;


        if(curHealth > 0)
        {
            yield return new WaitForSeconds(0.1f);
            mat.color = Color.white;
            
        }

        else if(!isDead)
        {
            isDead = true;
            
            gameObject.layer = 12;  //레이어 번호
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");

            switch (enemyType)
            {
                case Type.A:
                    manager.enemyCntA--;
                    mat.color = Color.black;
                    break;
                case Type.B:
                    manager.enemyCntB--;
                    mat.color = Color.black;
                    break;

                case Type.C:
                    manager.enemyCntC--;
                    mat.color = Color.blue;
                    GameObject instantGaText = Instantiate(GaText, transform.position + Vector3.up * 6, Quaternion.Euler(0, 0, 0));
                    Destroy(instantGaText, 1);
                    dieaudio.Play();
                    break;
            }

            switch (manager.allStage)
            {
                case 0:
                    Player player = target.GetComponent<Player>();
                    //player.score += score;
                    break;
                case 2:
                    Player3_2 player3_2 = target.GetComponent<Player3_2>();
                    player3_2.score += score;
                    break;
            }
            
            //int ranCoin = Random.Range(0, 3);
            //Instantiate(coins[ranCoin], transform.position, Quaternion.identity);




            /*
             if (isGrenade)      //항체로 죽을때 효과
            {
                reactVec = reactVec.normalized; //값 1로통일
                reactVec += Vector3.up*3;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized; //값 1로통일
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }
             */

            reactVec = reactVec.normalized; //값 1로통일
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);


            Destroy(gameObject, 4);
        }
        
    }
}
