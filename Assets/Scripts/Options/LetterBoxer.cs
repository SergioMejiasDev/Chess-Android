using UnityEngine;

/// <summary>
/// Add two letterboxes (or pillarboxes) to the game and correct the screen resolution.
/// </summary>
public static class LetterBoxer
{
    /// <summary>
    /// Add two letterboxes or pillarboxes to the scene depending on the resolution of the screen you are playing on.
    /// </summary>
    public static void AddLetterBoxing()
    {
        // An alternate camera is created that is the current screen size and projects a black background.
        // This camera should be placed behind the main camera so as not to overlap it.

        Camera letterBoxerCamera = new GameObject().AddComponent<Camera>();
        letterBoxerCamera.backgroundColor = Color.black;
        letterBoxerCamera.cullingMask = 0;
        letterBoxerCamera.depth = -100;
        letterBoxerCamera.farClipPlane = 1;
        letterBoxerCamera.useOcclusionCulling = false;
        letterBoxerCamera.allowHDR = false;
        letterBoxerCamera.allowMSAA = false;
        letterBoxerCamera.clearFlags = CameraClearFlags.Color;
        letterBoxerCamera.name = "Letter Boxer Camera";

        // We adapt the main camera so that it has a 16:9 resolution.

        PerformSizing();
    }

    /// <summary>
    /// Adapts the size of the main camera to the indicated resolution.
    /// </summary>
    static void PerformSizing()
    {
        Camera mainCamera = Camera.main;

        float targetRatio = 16.0f / 9.0f;

        float windowaspect = (float)Screen.width / (float)Screen.height;

        float scaleheight = windowaspect / targetRatio;

        // If the aspect ratio of the screen is less than 16:9 resolution, we adapt the size of the main camera to add the letterboxes.

        if (scaleheight < 1.0f)
        {
            Rect rect = mainCamera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            mainCamera.rect = rect;
        }

        // If it is higher than 16:9 resolution, we adapt the size of the main camera to add the pillarboxes.

        else
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = mainCamera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            mainCamera.rect = rect;
        }
    }
}