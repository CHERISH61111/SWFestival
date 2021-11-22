using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;

    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;
    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;

    

    public void GameStart() {

        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
       
    }

    void LateUpdate() {
        
        //stageTxt.text = "STAGE" + stage; 

        playerHealthTxt.text = player.health + " / " + player.maxHealth;
      //  playerCoinTxt.text = string.Format("{0:n0}", player.coin);

        if(player.equipWeapon == null)
            playerAmmoTxt.text = "- / " + player.ammo;
        else if(player.equipWeapon == null)
            playerAmmoTxt.text = "- / " + player.ammo;
        else if(player.equipWeapon.type == Weapon.Type.Melee)
            playerAmmoTxt.text = "- / " + player.ammo;
        else
            playerAmmoTxt.text =player.equipWeapon.curAmmo +  " / " + player.equipWeapon.maxAmmo;

        // weapon1Img.color = new Color(1,1,1,player.hasWeapons[0] ? 1 : 0);
        // weapon2Img.color = new Color(1,1,1,player.hasWeapons[1] ? 1 : 0);
        // weapon3Img.color = new Color(1,1,1,player.hasWeapons[2] ? 1 : 0);
        // weaponRImg.color = new Color(1,1,1,player.hasGrenades > 0 ? 1 : 0);

        enemyATxt.text = enemyCntA.ToString();
        enemyBTxt.text = enemyCntB.ToString();
        enemyCTxt.text = enemyCntC.ToString();

        playerCoinTxt.text = player.hasAmmo.ToString();
        


    }



}
