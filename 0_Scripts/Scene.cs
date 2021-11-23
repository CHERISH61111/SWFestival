using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void menu()
    {
        SceneManager.LoadScene("MENU");
    }
    public void s1()
    {
        SceneManager.LoadScene("STAGE1");
    }
    public void s2_1()
    {
        SceneManager.LoadScene("STAGE2_1");
    }

    public void s2_2()
    {
        SceneManager.LoadScene("STAGE2_2");
    }

    public void s3_1()
    {
        SceneManager.LoadScene("STAGE3_1");
    }

    public void s3_2_1()
    {
        SceneManager.LoadScene("STAGE3_2_1");
    }

    

}
