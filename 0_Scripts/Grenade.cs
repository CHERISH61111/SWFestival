using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    //public GameObject boxCol;
    public GameObject effectObj;
    public Rigidbody rigid;

    BoxCollider boxCollider;
    void Start()
    {
        //StartCoroutine(Explosion());
    }



    
    void OnCollisionStay(Collision collision)
    {
        boxCollider = GetComponent<BoxCollider>();
        if (collision.gameObject.tag == "Enemy")
        {
            collision.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
            boxCollider.isTrigger=true;
            rigid.isKinematic = true;
            Destroy(gameObject, 4f);
        }
    }



}
