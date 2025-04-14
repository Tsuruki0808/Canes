using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{

    //ツルキ追加
    private GameObject playerObject;//親オブジェクトの取得
    private Player playerScript;//Start()内で親のスクリプトを参照(scriptんうぃが変更される場合注意が必要)


    [SerializeField] private AudioSource Sound;//AudioSourceを入れるための箱
    [SerializeField] private AudioClip Touchse;
    



void Start()
    {
        // Playerオブジェクトを名前で検索して取得
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            playerScript = playerObj.GetComponent<Player>(); // Playerスクリプトを取得
        }
        else
        {
            Debug.LogError("Player object is not found in the scene.");
            return; // 処理を停止
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {

        if (c.gameObject.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false; // 2D用

            Sound.PlayOneShot(Touchse);

            playerScript.changeFlg_isBanana(true);
        }
    }
}
