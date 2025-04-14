using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public string SceneName;

    public bool changeSheneflg = false;
    public float waittimer = 1.0f;
    public float fadeSpeed = 1f; // フェードスピード
    public bool startWithFadeIn = true;
    
    private Image fadeImage;
    private Coroutine fadeCoroutine;

    void Start()
    {
        fadeImage = GetComponent<Image>();
        if (fadeImage == null)
        {
            Debug.LogError("FadeManager: Image コンポーネントが見つかりません！");
            return;
        }

        if (startWithFadeIn)
        {
            SetAlpha(1f);
            StartFadeIn();
        }
        else
        {
            SetAlpha(0f);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (changeSheneflg)
            {
                StartFadeOut(); // 手動でフェードアウトを開始
            }
        }
    }


    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }

    public void StartFadeIn()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(Fade(1f, 0f)); // 不透明 → 透明
    }

    public void StartFadeOut()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeSpeed);
            color.a = newAlpha;
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }

    private IEnumerator FadeAndLoadScene()
    {
        // まずフェードアウトする
        yield return StartCoroutine(Fade(0f, 1f));

        // 少し待機（演出用）
        yield return new WaitForSeconds(waittimer);

        // シーンを非同期ロード
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        asyncLoad.allowSceneActivation = false; // 自動で切り替えず、ロード完了を待つ

        // ロードが終わるまで待つ
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f) // ほぼロード完了
            {
                asyncLoad.allowSceneActivation = true; // シーンを切り替える
            }
            yield return null;
        }
    }

    public IEnumerator FadeAndLoadAnyScene(string Name)
    {
        // まずフェードアウトする
        yield return StartCoroutine(Fade(0f, 1f));

        // 少し待機（演出用）
        yield return new WaitForSeconds(waittimer);

        // シーンを非同期ロード
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Name);
        asyncLoad.allowSceneActivation = false; // 自動で切り替えず、ロード完了を待つ

        // ロードが終わるまで待つ
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f) // ほぼロード完了
            {
                asyncLoad.allowSceneActivation = true; // シーンを切り替える
            }
            yield return null;
        }
    }
}
