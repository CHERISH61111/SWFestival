using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletZone : MonoBehaviour
{

    public GameObject bullet1;
    public Transform target1;
    bool isAttack = false;
    //bool isChase = false;

    NavMeshAgent nav;

    //  Rigidbody rigid;

  

    void Update() {
        
        Invoke("ChaseStart",2f);


    }

    // void Update() {
    //         if(nav.enabled){
    //             nav.SetDestination(target1.position);
    //             nav.isStopped = !isChase;
    //         }  
            
    //     }

    // void FreezeVelocity(){
    //     if(isChase){
    //      rigid.velocity = Vector3.zero;
    //      rigid.angularVelocity = Vector3.zero;

    //     }
  
    // }

    void ChaseStart(){
      //  NavMeshAgent bulletNav = bullet1.GetComponent<NavMeshAgent>();
      //  bulletNav.enabled = true;
        
         if(!isAttack){
                     StartCoroutine(Attack());
                 }
    }
    
//     void Targeting(){



//     }

    IEnumerator Attack(){

                isAttack = true;
                yield return new WaitForSeconds(1f);
                //isChase = true;
                GameObject instantBullet = Instantiate(bullet1, transform.position, transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();

                nav = instantBullet.GetComponent<NavMeshAgent>();
                nav.SetDestination(target1.position);

                rigidBullet.velocity = (target1.transform.position - transform.position).normalized * 1 ;
                yield return new WaitForSeconds(2f);

                    rigidBullet.velocity = (target1.transform.position - transform.position).normalized * 10 ;
                    
                    Destroy(instantBullet, 5);
                
                
                

                isAttack = false;

        }


 }
