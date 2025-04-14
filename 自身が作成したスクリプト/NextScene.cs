using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    private float staytime = 1f; // シーン遷移までの待機時間（2秒）

    [SerializeField] private AudioSource Sound; // AudioSourceを入れるための箱
    [SerializeField] private AudioClip GoalSE; // ゴール時のSE（効果音）

    private GameObject imageObject; // 子オブジェクトのImageを参照するための変数

    
    // シーン遷移を実行するメソッド
    public void changeStage(string SceneName)
    {
        // 親オブジェクトに紐づくImageオブジェクトを取得
        imageObject = transform.Find("Image")?.gameObject; // 子オブジェクト「Image」を探して取得

        if (imageObject != null)
        {
            // 子オブジェクトのStartFadeIn()メソッドを呼び出す
            var fadeScript = imageObject.GetComponent<FadeManager>(); // 子オブジェクトのフェードイン処理スクリプトを取得（例: ImageFadeScript）
            if (fadeScript == null)
            {
                Debug.LogError("子オブジェクトに StartFadeIn() メソッドを持つスクリプトが見つかりません。");
            }
            else
            {
                fadeScript.StartFadeOut(); // フェードイン開始{

            }
        }
        else
        {
            Debug.LogError("子オブジェクトの Image がシーンに見つかりません。");
            return; // 処理を停止
        }
        
        // シーン遷移を待機してから実行
        StartCoroutine(WaitAndChangeScene(SceneName));
    }

    // シーン遷移を遅延させるコルーチン
    private IEnumerator WaitAndChangeScene(string SceneName)
    {

        yield return new WaitForSeconds(staytime); // staytime秒待つ
        SceneManager.LoadScene(SceneName); // シーン遷移
    }
}
