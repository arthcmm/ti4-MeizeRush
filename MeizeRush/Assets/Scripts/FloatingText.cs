using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fadeDuration = 1f;
    private TextMeshPro textMesh;
    private Color originalColor;

    void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshPro>();
        if (textMesh == null)
        {
            Debug.LogError("TextMeshPro component not found!");
        }
        else
        {
            originalColor = textMesh.color;
        }
    }

    void Start()
    {
        if (textMesh != null)
        {
            StartCoroutine(FadeAndMove());
        }
    }

    private IEnumerator FadeAndMove()
    {
        float elapsedTime = 0f;
        Vector3 originalPosition = transform.position;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            transform.position = originalPosition + Vector3.up * (moveSpeed * elapsedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
