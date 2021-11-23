using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCell: MonoBehaviour
{
    public GameObject memory;
    public GameObject baby;
    public GameObject B;
    public GameObject memoryTxt;
    public GameObject babyTxt;



    void Start()
    {
        StartCoroutine(CreateB());
    }

    void Awake()
    {

    }

    IEnumerator CreateB()
    {

        //   GameObject loadingCell = Instantiate(loading, transform.position, transform.rotation);
        yield return new WaitForSeconds(2f);

        //   loadingCell.SetActive(false);
        GameObject babyCell = Instantiate(baby, transform.position, transform.rotation);
        babyTxt.SetActive(true);

        Follow follow = babyTxt.GetComponent<Follow>();
        follow.target = babyCell.transform;

        GameObject MemoryCell = Instantiate(memory, transform.position, transform.rotation);
        memoryTxt.SetActive(true);

        AudioSource boonaudio = MemoryCell.GetComponent<AudioSource>();
        boonaudio.Play();

        Follow follow1 = memoryTxt.GetComponent<Follow>();
        follow1.target = MemoryCell.transform;
        yield return new WaitForSeconds(0.1f);
        B.SetActive(false);

        yield return new WaitForSeconds(2f);

        Rigidbody rigidBaby1 = babyCell.GetComponent<Rigidbody>();

        rigidBaby1.AddForce(Vector3.forward * 3, ForceMode.Impulse);


    }


}