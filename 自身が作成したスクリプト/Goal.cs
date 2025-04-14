using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // sceneの再読み込みを行うために追加

public class Goal : MonoBehaviour
{
    private GameObject playerObject; // 親オブジェクトの取得
    private Player playerScript; // Start()内で親のスクリプトを参照 (script名が変更される場合注意が必要)

    //feadオブジェクトのスクリプトを取得
    private GameObject feadObject;
    private FadeManager feadScript;
    
        
    

    public string SceneName = "ClearScene";

    [SerializeField] private AudioSource Sound; // AudioSourceを入れるための箱
    [SerializeField] private AudioClip GoalSE;

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

        GameObject feadObj = GameObject.Find("FeadImage");
        if (feadObj != null)
        {
            feadScript = feadObj.GetComponent<FadeManager>(); // FadeManagerスクリプトを取得
        }
        else
        {
            Debug.LogError("fead object is not found in the scene.");
            return; // 処理を停止
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            Sound.PlayOneShot(GoalSE);

            if (playerScript != null)
            {
                playerScript.changeFlg_isGoal(true);
            }
            else
            {
                Debug.LogError("playerScriptが設定されていません");
            }

            // 3秒後にReStartGameを呼び出す
            Invoke("ReStartGame", 3f);
        }
    }

    void ReStartGame()
    {
        //ここでFeadBordの子供オブジェクト、FeadImageについているscriptの関数、IEnumerator FadeAndLoadAnyScene(string Name)を実行したい。Startでオブジェクトを取得するかんじかな？

        StartCoroutine(feadScript.FadeAndLoadAnyScene(SceneName));

        // Sceneの再読み込み
        //SceneManager.LoadScene(SceneName);
    }
}
