using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class ResetSceneController : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] float delayBeforeLoad = 0.5f;
    private void Start()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        yield return StartCoroutine(FadeIn());
        yield return new
        WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(GameManager.Instance.nextSceneName);
    }
    IEnumerator FadeIn()
    {
        float t = 0;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }
}

