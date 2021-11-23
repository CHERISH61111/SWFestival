using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isEntered = false;

    public GameObject MenuBCtext;
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public GameObject playerText;
    public Player1 player1;
    public GameObject player1Text;
    public Player2 player2;
    public GameObject player2Text;
    public Player3_2 player3_2;
    public GameObject player3_2Text;
    public memoryEnding memoryending;
    public GameObject startZone;
    public RectTransform bookUi;

    public int allStage;
    public float playTime;
    public bool isBattle;
    public bool isStage2_2;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    public GameObject menuPanel;
    public GameObject stagePanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    
    public Text uiUpTxt;
    public Text uiUp_CenterTxt;
    public Text uiCenterTxt;
    public Text uiCenter_DownTxt;
    public Text uiDownTxt;
    public Image informImg;
    public Text informTitleTxt;
    public Text informContentsTxt;
    //public Text maxScoreTxt;
    //public Text scoreTxt;
    //public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;
    public Text playerGrenadeTxt;
    //public Image weapon1Img;
    //public Image weapon2Img;
    //public Image weapon3Img;
    //public Image weaponRImg;
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;
    public Text curScoreText;
    public Text bestText;

    public GameObject Amimg;

    
    void Awake()
    {
        
        if (allStage == 2) 
            enemyList = new List<int>();
        //maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));

        //if (PlayerPrefs.HasKey("MaxScore"))
        //    PlayerPrefs.SetInt("MaxScore", 0);

        if (allStage == 22)
            Invoke("showScene",8);
        if (allStage == 8)
            Invoke("showScene9", 5);


    }
    
    public void showScene8()
    {
        SceneManager.LoadScene(8);
    }
    void showScene()
    {
        SceneManager.LoadScene(2);
    }
    void showScene9()
    {
        SceneManager.LoadScene(9);
    }

    public void GameStart()
    {
        switch(allStage){
            case -1:
                SceneManager.LoadScene("STAGE1");
                break;
            case 0:
                player.gameObject.SetActive(true);
                
                break;
            case 1:
                player1.gameObject.SetActive(true);
                
                
                break;
            case 2:
                player3_2.gameObject.SetActive(true);
                
                break;
          

        }
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        stagePanel.SetActive(true);


    }
    public void GameOver()
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        switch (allStage)
        {
            case 0:
                player.isDead = true;
                break;
            case 1:
                player1.isDead = true;
                break;
            case 2:
                player3_2.isDead = true;
                break;
            case 21:
                player2.isDead = true;
                break;
            case 322:
                memoryending.isDead = true;
                break;

        }
        //curScoreText.text = scoreTxt.text;
        //int maxScore = PlayerPrefs.GetInt("MaxScore");
        //switch (allStage)
        //{
        //    case 0:
                
        //        if (player.score > maxScore)
        //        {
        //            bestText.gameObject.SetActive(true);
        //            PlayerPrefs.SetInt("MaxScore", player.score);
        //        }
        //        break;
        //    case 1:

        //        if (player1.score > maxScore)
        //        {
        //            bestText.gameObject.SetActive(true);
        //            PlayerPrefs.SetInt("MaxScore", player1.score);
        //        }
        //        break;
        //    case 2:
                
        //        if (player3_2.score > maxScore)
        //        {
        //            bestText.gameObject.SetActive(true);
        //            PlayerPrefs.SetInt("MaxScore", player3_2.score);
        //        }
        //        break;
                
        //}

    }

    public void Restart()
    {
        switch (allStage)
        {
            case 0:
                SceneManager.LoadScene(2);
                break;
            case 1:
                SceneManager.LoadScene(1);
                break;
            case 2:
                SceneManager.LoadScene(3);
                break;
            case 21:
                if (!isStage2_2)
                    SceneManager.LoadScene(4);
                else if (isStage2_2)
                    SceneManager.LoadScene(5);
                break;
            case 322:   
                SceneManager.LoadScene(10);
                break;

        }
        
    }

    public void StageStart()
    {
        stagePanel.SetActive(false);
        gamePanel.SetActive(true);
        startZone.SetActive(false);

        if (allStage == 2) player3_2.isGrenade = false;

        switch (allStage)
        {
            case 0:

                //MeshRenderer ptext = playerText.GetComponent<MeshRenderer>();
                //ptext.enabled = true;
                playerText.SetActive(true);
                player.startBottle.SetActive(true);
                player.startGun.SetActive(true);    
                player.createVirus();
                player.createAmmo();
                isBattle = true;
                
                break;
            case 1:
                player1.Enemy1.gameObject.SetActive(true);
                player1.Enemy2.gameObject.SetActive(true);
                player1.Enemy3.gameObject.SetActive(true);
                MeshRenderer p1text = player1Text.GetComponent<MeshRenderer>();
                p1text.enabled = true;
                player1.Spawn();    
                break;
            case 2:
                player3_2Text.SetActive(true);
                foreach (Transform zone in enemyZones)
                    zone.gameObject.SetActive(true);
                break;
            case 322:
            case 21:
                MeshRenderer p2text = player2Text.GetComponent<MeshRenderer>();
                p2text.enabled = true;
                break;
            



        }
        
        if (allStage != 21)
        {
            isBattle = true;
            StartCoroutine(InBattle());
        }
        

    }

    public void StageEnd()
    {
        switch (allStage) { 
            case 0:
                SceneManager.LoadScene(3);
                // player.transform.position = Vector3.up * 0.8f;
                break;
            case 1:
                SceneManager.LoadScene(4);
                //player1.transform.position = Vector3.up * 0.8f;
                break;
            case 2:
                
                if (DataManager.isFirstMemoried == true && DataManager.isFirstCleared == false)
                {
                    DataManager.isFirstCleared = true;
                    Invoke("clearrr", 3);
                }
                else
                    SceneManager.LoadScene(10);
                break;
            case 21:
                if (!isStage2_2)
                    SceneManager.LoadScene(5);
                else if (isStage2_2)
                    SceneManager.LoadScene(6);
                break;
            case 22:
                SceneManager.LoadScene(2);
                break;
        }

        isBattle = false;
        //stage++;
    }

    void clearrr()
    {
        SceneManager.LoadScene(11);
    }
    IEnumerator InBattle()
    {


        /*
        for (int index=0; index < stage; index++)
        {
            int ran = Random.Range(0,3);
            enemyList.Add(ran);

            switch (ran)
            {
                case 0:
                    enemyCntA++;
                    break;
                case 1:
                    enemyCntB++;
                    break;
                case 2:
                    enemyCntC++;
                    break;
            }
        }*/
        
        // 몬스터 수
        switch (allStage)
        {
            case 0:
                break;
            case 2:
                int n = 20;
                for(int i=0;i<n;i++)
                    enemyList.Add(1);
                enemyCntB = n;
                break;
        }

        while (enemyList.Count != 0)
        {
            
            switch (allStage)
            {
                case 0:
                    //enemy.target = player.transform;
                    

                    break;
                case 1:
                    //enemy.target = player1.transform;
                   
                    break;
                case 2:
                    int ranZone = Random.Range(0, 3);
                    GameObject instantEnemy = Instantiate(enemies[enemyList[0]],
                                                          enemyZones[ranZone].position,
                                                          enemyZones[ranZone].rotation);
                    Enemy enemy = instantEnemy.GetComponent<Enemy>();
                    enemy.target = player3_2.transform;
                    enemy.manager = this;
                    enemyList.RemoveAt(0);
                    yield return new WaitForSeconds(1f);
                    //while ( enemyCntB > 0)
                    //{
                    //    yield return null;          //이렇게 쓰면 update()랑 비슷한역할. 한프레임
                    //}
                    break;

            }

            //if (allStage == 2 && enemyCntB == 0)
            //{
            //    yield return new WaitForSeconds(4f);
            //    StageEnd();
            //}


        }

        while (enemyCntB > 0)
        {
            yield return null;          //이렇게 쓰면 update()랑 비슷한역할. 한프레임
        }

        if (allStage == 2 && enemyCntB == 0)
        {
            yield return new WaitForSeconds(4f);
            StageEnd();
        }
            //while (enemyCntA + enemyCntB + enemyCntC > 0)
            //{
            //    yield return null;          //이렇게 쓰면 update()랑 비슷한역할. 한프레임
            //}

            //yield return new WaitForSeconds(4f);
            //StageEnd();

        }



    public void Enter()
    {   

        bookUi.anchoredPosition = Vector3.zero;
        Debug.Log("book");
        isEntered = true;

        if (DataManager.isFirstMemoried == true )
            Amimg.gameObject.SetActive(true);

    }
    public void Exit()
    {
        bookUi.anchoredPosition = Vector3.down * 1000;
        isEntered = false;
        if (allStage == 322)
        {
            uiDownTxt.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (isBattle)
            playTime += Time.deltaTime;

        //게임오버
        switch (allStage)
        {
            case 0:
                if (player.health == 0)
                {
                    GameOver();
                    player.isDead = true;
                }

                else if (enemyCntC == 0)
                    Invoke("StageEnd", 4);
;                    break;
            case 1:
                if ((int)playTime == 60)
                    GameOver();

                break;


        }
        

    }

    public void MenuBeforClear()
    {
        if (DataManager.isFirstCleared == false)
        {
            MenuBCtext.SetActive(true);
            Invoke("TimeLater", 1);
        }
        else
            SceneManager.LoadScene("MENU");
    }

    void TimeLater()
    {
        MenuBCtext.SetActive(false);
    }
    void LateUpdate()
    {
        //상단 UI
        //switch (allStage)
        //{
        //    case 0:

        //        break;
        //    case 1:
        //        stageTxt.text = "STAGE " + "1. 항원 정보를 획득하라";

        //        break;
        //    case 2:

        //        break;
        //}
        

        int second = 60-(int)(playTime % 60);

        if (allStage == 1)
            playTimeTxt.text = second + "s";

        switch (allStage)
        {
            case 0:
                //플레이어 UI
                //플레이어 UI
                playerHealthTxt.text = player.health + " / " + player.maxHealth;
                Weapon weapon = player.weapons[2].GetComponent<Weapon>();
                playerAmmoTxt.text = weapon.curAmmo + " / 30";

                playerCoinTxt.text = player.hasAmmo.ToString();

                enemyCTxt.text = enemyCntC.ToString();

               
                break;
            case 1:

                enemyATxt.text = enemyCntA.ToString();
                break;
            case 2:
                //플레이어 UI
                playerHealthTxt.text = player3_2.health + " / " + player3_2.maxHealth;
                playerGrenadeTxt.text = player3_2.hasGrenades + " / " + player3_2.maxHasGrenades;
                enemyBTxt.text = enemyCntB.ToString();
                break;
        }

    }
}
