using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchblockjudgement : MonoBehaviour
{
    [Header("1が前方、2が足元")]
    public int index = 0;

    private GameObject playerObject;
    private Player playerScript;

    Renderer myRenderer;

    void Start()
    {
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
        myRenderer = GetComponent<Renderer>();
    }

    private bool shouldReturn = false;
    private bool actionJump = false;

    void Update()
    {
        if (shouldReturn)
        {
            GameObject.Find(transform.root.name).SendMessage("ReturnX");
            shouldReturn = false;
        }
        
        if(actionJump )
        {
            playerScript.changeFlg_isJanp(true);
            actionJump = false;
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        //移動中でないなら処理を中止
        if (!playerScript.getFlg_isMove())
        {
         //   Debug.Log("停止中のため処理を中止");

            return;
        }
        if ((c.gameObject.tag == "Ground" || c.gameObject.tag == "ConnectBlock" || c.gameObject.tag == "MouseDragBlock") && index == 2)
        {
            playerScript.changeFlg_isFalling(false);
        }

        if ((c.gameObject.tag == "Ground" || c.gameObject.tag == "ConnectBlock" || c.gameObject.tag == "MouseDragBlock") && index == 1)
        {
            //すでに切り返すフラグが真なら
            if (shouldReturn) return;

            bool canJump = true;

            foreach (Collider2D collider in Physics2D.OverlapBoxAll(transform.position, myRenderer.bounds.size, 0f))
            {
                if (collider.gameObject.tag == "Ground" || collider.gameObject.tag == "ConnectBlock" || collider.gameObject.tag == "MouseDragBlock")
                {
                    Debug.Log($"[判定] block={collider.gameObject.tag}, collider maxY={collider.bounds.max.y}, my maxY={myRenderer.bounds.max.y}");

                    // ジャンプ判定の修正
                    if (collider.bounds.max.y >= myRenderer.bounds.max.y)
                    {
                        Debug.Log("[切り返し] 高さが足りないので折り返し処理");
                        canJump = false;
                        shouldReturn = true;
                        break;
                    }
                }
            }

            if (canJump && !playerScript.getFlg_isFalling())
            {
                Debug.Log("[ジャンプ] 高さをクリアしているのでジャンプ実行");
                actionJump = true;
            }
        }

        
    }

    void OnTriggerStay2D(Collider2D c)
    {
        //移動中でないなら処理を中止
        if (!playerScript.getFlg_isMove())
        {
          //  Debug.Log("停止中のため処理を中止");
            return;
        }
        if ((c.gameObject.tag == "Ground" || c.gameObject.tag == "ConnectBlock" || c.gameObject.tag == "MouseDragBlock") && index == 2)
        {
            playerScript.changeFlg_isFalling(false);
        }

        if ((c.gameObject.tag == "Ground" || c.gameObject.tag == "ConnectBlock" || c.gameObject.tag == "MouseDragBlock") && index == 1)
        {
            //すでに切り返すフラグが真なら
            if (shouldReturn) return;
            bool canJump = true;

            foreach (Collider2D collider in Physics2D.OverlapBoxAll(transform.position, myRenderer.bounds.size, 0f))
            {
                if (collider.gameObject.tag == "Ground" || collider.gameObject.tag == "ConnectBlock" || collider.gameObject.tag == "MouseDragBlock")
                {
                    Debug.Log($"[判定] block={collider.gameObject.tag}, collider maxY={collider.bounds.max.y}, my maxY={myRenderer.bounds.max.y}");

                    // ジャンプ判定の修正
                    if (collider.bounds.max.y >= myRenderer.bounds.max.y)
                    {
                        Debug.Log("[切り返し] 高さが足りないので折り返し処理");
                        canJump = false;
                        shouldReturn = true;
                        break;
                    }
                }
            }

            if (canJump &&  !playerScript.getFlg_isFalling())
            {
                Debug.Log("[ジャンプ] 高さをクリアしているのでジャンプ実行");
                actionJump = true;
            }
        }


      
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if ((c.gameObject.tag == "Ground" || c.gameObject.tag == "ConnectBlock" || c.gameObject.tag == "MouseDragBlock") && index == 2)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, myRenderer.bounds.size, 0f);
            bool isTouchingGroundOrConnectBlock = false;

            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.tag == "Ground" || collider.gameObject.tag == "ConnectBlock" || collider.gameObject.tag == "MouseDragBlock")
                {
                    isTouchingGroundOrConnectBlock = true;
                    break;
                }
            }

            if (!isTouchingGroundOrConnectBlock)
            {
                Debug.Log("地上またはコネクトブロックから完全に離れた");
                playerScript.changeFlg_isFalling(true);
            }
        }
    }
}
