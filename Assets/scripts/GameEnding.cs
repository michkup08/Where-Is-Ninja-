using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameEnding : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup fadeScreenGroup;
    public VideoPlayer endingVideo;
    public RawImage videoDisplay;

    [Header("Settings")]
    public float delayAfterBossDeath = 3f;
    public float fadeDuration = 2f;
    public string menuSceneName = "MainMenu";

    private void Awake()
    {
        Time.timeScale = 1f;

        if (fadeScreenGroup != null)
        {
            fadeScreenGroup.alpha = 0f;
            fadeScreenGroup.blocksRaycasts = false;
        }

        if (videoDisplay != null)
        {
            videoDisplay.gameObject.SetActive(false);
        }

        if (endingVideo != null && endingVideo.targetTexture != null)
        {
            endingVideo.targetTexture.Release();
        }
    }

    public void TriggerEndingSequence()
    {
        StartCoroutine(SequenceRoutine());
    }

    IEnumerator SequenceRoutine()
    {
        yield return new WaitForSeconds(delayAfterBossDeath);

        Time.timeScale = 0f;

        if (fadeScreenGroup != null) fadeScreenGroup.blocksRaycasts = true;

        if (endingVideo != null)
        {
            endingVideo.frame = 0;
            endingVideo.Prepare();
        }

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            if (fadeScreenGroup != null)
                fadeScreenGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        if (fadeScreenGroup != null) fadeScreenGroup.alpha = 1f;

        if (endingVideo != null)
        {
            while (!endingVideo.isPrepared)
            {
                yield return null;
            }

            endingVideo.Play();

            yield return new WaitForSecondsRealtime(0.1f);

            if (videoDisplay != null)
            {
                videoDisplay.gameObject.SetActive(true);
            }

            yield return new WaitForSecondsRealtime((float)endingVideo.length - 0.1f);
        }
        else
        {
            yield return new WaitForSecondsRealtime(2f);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}