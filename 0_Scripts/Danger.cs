using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Danger : MonoBehaviour
{
    //Color color;
    public Image image;


    // Start is called before the first frame update
    void Awake()
    {
        //image = GetComponent<Image>().color;

        StartCoroutine(Warning());

    }

    // Update is called once per frame
    IEnumerator Warning()
    {
        while (true)
        {
            image.color = new Color32(118, 4, 0, 150);
            Debug.Log("red");
            yield return new WaitForSeconds(0.5f);
            Debug.Log("white");
            image.color = new Color32(0, 0, 0, 150);
            yield return new WaitForSeconds(0.5f);
        }

    }
}
