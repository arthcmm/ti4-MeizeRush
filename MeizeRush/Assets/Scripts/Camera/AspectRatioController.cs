using UnityEngine;

public class AspectRatioController : MonoBehaviour
{
    public float targetAspectRatio = 4.0f / 3.0f;

    void Start()
    {
        float windowAspectRatio = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspectRatio / targetAspectRatio;
        Camera camera = Camera.main;

        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = camera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }
    }
}
