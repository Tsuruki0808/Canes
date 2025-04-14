using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonimage : MonoBehaviour
{
    private GameObject playerObject; // 親オブジェクトの取得
    private Player playerScript; // Start()内で親のスクリプトを参照 (script名が変更される場合注意が必要)


    private Animator animator;//アニメーション

    private bool stopflg = false; 

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

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

    // Update is called once per frame
    void Update()
    {

        if (playerScript.getFlg_isMove() != stopflg)
        {
            stopflg = playerScript.getFlg_isMove();
            
        }

        if (stopflg)
        {
            //停止buttonの再生
            animator.SetBool("isStop", false);
        }
        else
        {
            //再生ボタンの表示
            animator.SetBool("isStop", true);
        }
    }

    public void changeAnime(bool newflg)
    {

        stopflg = newflg;

    }
}
