using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//sceneの再読み込みを行うために追加


public class securityguard : MonoBehaviour
{

    [SerializeField] private AudioSource Sound;//AudioSourceを入れるための箱
    [SerializeField] private AudioClip warningse;
    [SerializeField] private AudioClip watchse;



    private GameObject playerObject;//親オブジェクトの取得
    private Player playerScript;//Start()内で親のスクリプトを参照(scriptんうぃが変更される場合注意が必要)



        



    private Animator animator;//アニメーション
    private GameObject target;
    Rigidbody2D rb;

    private int direction;

    [Header("この範囲内にはいると目を開き警戒状態へ")]
    public float warningarea = 6.0f;
    [Header("この範囲内に入ると発見されミスとなる")]
    public float watcharea = 3.0f;



    // Start is called before the first frame update
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
        

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component not found on this GameObject.");
        }

        target = GameObject.Find("Player");//プレイヤーがどこにいるかの取得(playerの名前が変更されるとエラーになる)
        rb = target.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Euler(0, direction, 0); // プレイヤーを→向きにする


        if (rb.transform.position.x >= transform.position.x)
        {
            direction = 180;
        }
        else
        {
            direction = 0;
        }



        
        Debug.DrawRay(transform.position, transform.right * -1 * watcharea, Color.blue, 0.1f);

        //レイキャストに触れたすべてのオブジェクトを取得し、そのオブジェクトがplayerのみか検証
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right * -1, watcharea);

        bool foundPlayer = false;

        //保存されているオブジェクトの数ループ
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                // "Player"タグのオブジェクトが最初にヒット
                if (hit.collider.CompareTag("Player"))
                {
                    foundPlayer = true;
                }
                // "Player"以外のタグのオブジェクトが最初にヒットした場合は処理しない
                else
                {
                    foundPlayer = false;
                    break; // 他のオブジェクトがあれば処理しない
                }
            }
        }
        //上記のループが問題なく終了できたのなら
        if (foundPlayer)
        {
            if (!animator.GetBool("isLook"))
            {
                Sound.PlayOneShot(watchse);
            }
            animator.SetBool("isLook", true);

            Invoke(nameof(ReStart), 2.0f);

           


            playerScript.changeFlg_isAlive(false);
        }







        //playerとの距離を取得
        float dis = Vector3.Distance(rb.position, transform.position);

        if (dis > warningarea)
        {
            animator.SetBool("isWarning", false);
            animator.SetBool("isLook", false);

        }
        else if (dis < warningarea)
        {
            if (!animator.GetBool("isWarning"))
            {
                Sound.PlayOneShot(warningse);
            }
            animator.SetBool("isWarning", true);

         



        }
        else if (dis < watcharea)
        {
            //    animator.SetBool("isLook", true);
            //            Sound.PlayOneShot(watchse);

        }


    }

    void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f, 1.0f); // 色を変更(RGB,透明度)
        Gizmos.DrawWireSphere(transform.position, watcharea);// 座標に球体を描画


        Gizmos.color = new Color(1.0f, 0.2f, 0.0f, 1.0f); // 色をに変更(RGB,透明度)
        Gizmos.DrawWireSphere(transform.position, warningarea);

     
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;  // 選択された時に色を変更
        Gizmos.DrawWireSphere(transform.position, watcharea);
    }
}
