using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Player playerScript; // Playerスクリプトの参照
    private Animator animator;   // アニメーション制御用
    private bool moveflg = false;

    void Start()
    {
        // Playerオブジェクトの取得
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            playerScript = playerObj.GetComponent<Player>();
        }
        else
        {
            Debug.LogError("Player object is not found in the scene.");
            return;
        }

        // 自身のAnimatorコンポーネントを取得
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing.");
        }
    }

    void Update()
    {

        if (playerScript.getFlg_isMove() != moveflg)
        {
            moveflg = playerScript.getFlg_isMove();
            if (animator != null)
            {
                animator.SetBool("isStop", !moveflg);
            }
        }
    }

    public void Walk()
    {
        moveflg = !moveflg;
        playerScript.changeFlg_isMove(moveflg);

        if (animator != null)
        {
            animator.SetBool("isStop", !moveflg);
        }
    }
}
