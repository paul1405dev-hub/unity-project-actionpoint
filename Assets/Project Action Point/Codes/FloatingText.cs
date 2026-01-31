using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    private Text textComponent;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [Header("Config")]
    public float moveSpeed = 60f;
    public float fadeDuration = 0.5f;
    public float displayTime = 0.3f;

    void Awake()
    {
        textComponent = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
        }

    }

    void OnEnable()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < displayTime + fadeDuration)
        {
            // Y축으로 moveSpeed * Time.deltaTime 만큼 이동
            rectTransform.anchoredPosition += new Vector2(0f, moveSpeed * Time.deltaTime);

            // 페이드 아웃
            if (elapsedTime > displayTime)
            {
                float fade = (elapsedTime - displayTime) / fadeDuration;
                canvasGroup.alpha = 1f - fade;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

}


