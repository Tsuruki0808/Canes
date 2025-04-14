using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//sceneの再読み込みを行うために追加

public class Player : MonoBehaviour
{
    private Animator animator;//アニメーション
    private Rigidbody2D rb; // プレイヤーのRigidbody2D


    [SerializeField] private AudioSource Sound;//AudioSourceを入れるための箱

    [SerializeField] private AudioClip jump;//ジャンプの音
    [SerializeField] private AudioClip wallcontact;//壁衝突の音の音


    [SerializeField] private AudioClip Miss;//失敗時の音
    [SerializeField] private AudioClip Discovery;//発見時の音

    public float PlayerSpeed = 2.0f;//移動速度
    public float JanpForce = 5.0f;//ジャンプ力
    [Header("ミスしたときにこの時間待機してからsceneを再読み込みする")]
    public float MissSETime = 3.0f;

    public int PlayerX = 0;

   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component not found on this GameObject.");
        }
    }


    // Update is called once per frame
    void Update()
    {
        //ぶっちゃけReturnX()内に記載したほうがいい
        if (animator.GetBool("isAlive"))
        {
            transform.rotation = Quaternion.Euler(0, PlayerX, 0); // プレイヤーの向きを修正するにする

        }


        /*
                //確認用(このフェードインを使っていろいろしたいな)
                if (Input.GetKeyDown(KeyCode.W))
                {

                    FadeManager fadeManager = FindObjectOfType<FadeManager>();
                    if (fadeManager != null)
                    {
                        fadeManager.StartFadeOut(); // StartFadeInを呼び出す
                    }

                    //Sound.PlayOneShot(Discovery);
                }
        */

        //ゴールしたらピョンピョン跳ねる
        if (animator.GetBool("isGoal"))
        {

            if (!animator.GetBool("isFalling"))
            {
                rb.velocity = Vector2.zero; // 速度をリセットしてjumpを一定に
                rb.AddForce(new Vector2(0, 3.0f), ForceMode2D.Impulse);
            }
        }

        //生きているなら各種動作を行う
        else if (animator.GetBool("isAlive") && animator.GetBool("isMove") && !animator.GetBool("isGoal") && !animator.GetBool("isBanana"))
        {
            var velocity = Vector3.zero;//速度を0にリセット
            velocity.x = PlayerSpeed;//X方向に向けた速度を保存
            transform.position += transform.rotation * -velocity * Time.deltaTime;//実際に移動

        }
        else if (animator.GetBool("isAlive") == false || animator.GetBool("isBanana") == true)
        {
            ChangeSpriteVisibility("KeyImg", false);
            
        }

    }






    void ReturnX()
    {
        Debug.Log("関数受け取り");
        Sound.PlayOneShot(wallcontact);

        if (PlayerX == 0)
        {
            PlayerX = 180;

        }
        else
        {
            PlayerX = 0;
        }

    }


    public void changeFlg_isMove(bool newflg)
    {
        animator.SetBool("isMove", newflg);
        
    }
    public bool getFlg_isMove()
    {
        return animator.GetBool("isMove");
    }

    public void changeFlg_isAlive(bool newflg)
    {
        animator.SetBool("isAlive", newflg);
       
        //もし主人公が死亡したら再スタート
        if(newflg == false)
        {
            Debug.Log("ダメでした");
           
            transform.rotation = Quaternion.Euler(0, PlayerX, -10);
            rb.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
            
            //SEを再生
            //  Sound.PlayOneShot(Miss);

            //数秒後にシーンを再読み込み
            Invoke(nameof(ReStartGame), MissSETime);
          
        }
    }
    public void changeFlg_isFalling(bool newflg)
    {
        animator.SetBool("isFalling", newflg);
       //もしジャンプからの着地ならジャンプを終了させる
        if(newflg == false)
        {
            animator.SetBool("isJanp", false);
        }
    }
    public bool getFlg_isFalling()
    {
        return animator.GetBool("isFalling");
    }

    public void changeFlg_isJanp(bool newflg)
    {
        if (!animator.GetBool("isJanp"))
        {
            if (animator.GetBool("isAlive"))
            {
                animator.SetBool("isJanp", newflg);
                Sound.PlayOneShot(jump);
                //飛び越えるためにジャンプを行う(瞬間的に速度を加える)
                rb.AddForce(new Vector2(0, JanpForce), ForceMode2D.Impulse);
            }
        }
    }
    public bool getFlg_isJanp()
    {
        return animator.GetBool("isJanp");
    }


    public void changeFlg_isGoal(bool newflg)
    {
        animator.SetBool("isGoal", newflg);    
    }


    public bool getFlg_isItem()
    {
        return animator.GetBool("isItem");
    }


    public void changeFlg_isItem(bool newflg)
    {
        animator.SetBool("isItem", newflg);
        //アイテム画像(鍵)の表示非表示の切り替え
        ChangeSpriteVisibility("KeyImg", newflg);
        
    }

    void ChangeSpriteVisibility(string childName, bool isVisible)
    {
        //該当のオブジェクトを検索
        Transform child = transform.Find(childName);
        if (child != null)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = isVisible; // true なら表示、false なら非表示
            }
        }
    }



    public void changeFlg_isBanana(bool newflg)
    {
        if(newflg == true)
        {
            Vector2 forceDirection = transform.right * -3f; // 右方向に力を加える（数値は調整）
            rb.AddForce(forceDirection, ForceMode2D.Impulse);
        }
        animator.SetBool("isBanana", newflg);



        //数秒後にシーンを再読み込み
        Invoke(nameof(ReStartGame), MissSETime);

    }


    void ReStartGame()
    {
        //sceneの再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
