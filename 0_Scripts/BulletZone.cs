using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletZone : MonoBehaviour
{

    public GameObject bullet1;
    public Transform target1;
    bool isAttack = false;
    bool isChase = false;

    public GameManager manager;

    NavMeshAgent nav;

    Rigidbody rigid;

  

    void Update() {
        if (manager.isBattle == true)
            Invoke("ChaseStart", 2f);

        //if (nav.enabled)
        //{
        //    nav.SetDestination(target1.position);
        //    nav.isStopped = !isChase;
        //}


    }

    private void Awake()
    {
        nav = bullet1.GetComponent<NavMeshAgent>();
        rigid = bullet1.GetComponent<Rigidbody>();
    }


    //void Update() {
    //        if(nav.enabled){

    //            nav.SetDestination(target1.position);
    //            nav.isStopped = !isChase;
    //        }  

    //    }

    //void FreezeVelocity(){
    //    if(isChase){
    //     rigid.velocity = Vector3.zero;
    //     rigid.angularVelocity = Vector3.zero;

    //    }

    //}

    void ChaseStart(){

        nav.enabled = false;
        
         if(!isAttack){
                     StartCoroutine(Attack());
                 }
    }
    
//     void Targeting(){



//     }

    IEnumerator Attack(){

        isAttack = true;
        
        //isChase = true;
        GameObject instantBullet = Instantiate(bullet1, transform.position, transform.rotation);
        NavMeshAgent instantNav = instantBullet.GetComponent<NavMeshAgent>();
        instantNav.enabled = false;
        yield return new WaitForSeconds(1f);
        instantNav.enabled = true;

        Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();   
        rigidBullet.velocity = (target1.transform.position - transform.position).normalized * 10 ;


        //rigidBullet.velocity = (target1.transform.position - transform.position).normalized * 1 ;
        //yield return new WaitForSeconds(2f);

            //rigidBullet.velocity = (target1.transform.position - transform.position).normalized * 10 ;
        yield return new WaitForSeconds(1f);

        //nav = instantBullet.GetComponent<NavMeshAgent>();
        //nav.SetDestination(target1.position);

        Destroy(instantBullet, 5);
                
                
                

        isAttack = false;

        }


 }
