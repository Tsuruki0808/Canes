using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip[] bgmClips; // 5つのBGMをアタッチする配列

    public static GameManager Instance
    {
        get; private set;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // シーンがロードされたときに PlayBGMForScene を呼び出すイベントを登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // イベントを解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(); // シーンがロードされるたびに呼び出す
    }

    void Start()
    {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        //  PlayBGMForScene(); // ゲーム開始時に適切なBGMを再生
    }



    void PlayBGMForScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        //タイトル
        if (sceneName == "Title")
        {
            audioSource.clip = bgmClips[0];
        }
        //ステージセレクト
        else if (sceneName == "StageSelect")
        {
            audioSource.clip = bgmClips[1];
        }
        else
        {
            audioSource.clip = bgmClips[2]; 
        }

        if (audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void SelectedStage()//ステージセレクトでステージが決まったら行う処理
    {
        audioSource.Stop();
    }

    void StopBGM()
    {
        audioSource.Stop();
    }

}
