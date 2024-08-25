using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingTextManager : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab; 
    [SerializeField] private Transform canvasTransform; 

    public void ShowFloatingText(string textValue)
    {
        GameObject floatingTextInstance = Instantiate(textPrefab, canvasTransform);
        TextMeshProUGUI textComponent = floatingTextInstance.GetComponent<TextMeshProUGUI>();
        textComponent.text = textValue;
        textComponent.fontSize = 36;  
        textComponent.rectTransform.sizeDelta = new Vector2(200, 1);  
        floatingTextInstance.transform.localPosition = GetRandomPositionWithinCanvas();
        StartCoroutine(FadeAndMove(floatingTextInstance));
    }

    private Vector3 GetRandomPositionWithinCanvas()
    {
        RectTransform canvasRect = canvasTransform.GetComponent<RectTransform>();
        float x = Random.Range(-canvasRect.rect.width / 2, canvasRect.rect.width / 2);
        float y = Random.Range(-canvasRect.rect.height / 2, canvasRect.rect.height / 2);
        return new Vector3(x, y, 0);
    }

    private IEnumerator FadeAndMove(GameObject floatingText)
    {
        float duration = 1f;
        Vector3 startPosition = floatingText.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(Random.Range(-50, 50), Random.Range(50, 100), 0);
        float elapsed = 0f;

        CanvasGroup canvasGroup = floatingText.AddComponent<CanvasGroup>();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            floatingText.transform.localPosition = Vector3.Lerp(startPosition, endPosition, progress);
            canvasGroup.alpha = 1 - progress;
            yield return null;
        }

        Destroy(floatingText);
    }
}
