using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour {
    
    private const float OpacityMax = 1.0f;
    private const float OpacityMin = 0.0f;
    public const float FadeTimeDefault = 1.0f;
    public const float TimeBeforeFadeDefault = 1.0f;
    
    [Header("[ FADING IN ]")]
    [SerializeField] private bool _fadeIn;
    [SerializeField] private float _fadeInTimeInSeconds;  

    private Texture _overlayTexture;    
    private Color _overlayColour;    
    private bool _fadeComplete;

    public void FadeOut()
    {
        StartCoroutine(FadeOverlayAlpha(1.0f, FadeTimeDefault));
    }
    
    /// <summary>
    /// Creates a <c>Texture2D</c> of the specified size and colour.
    /// </summary>
    /// <param name="width">width of the texture in pixels.</param>
    /// <param name="height">height of the texture in pixels.</param>
    /// <param name="colour">colour of the texture.</param>
    /// <returns><c>Texture2D</c> of the specified size and colour.</returns>
    private static Texture2D CreateTexture(int width, int height, Color32 colour)
    {
        var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        for (var y = 0; y < texture.height; y++)
        {
            for (var x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, colour);
            }
        }
        texture.Apply();
        return texture;
    }

    public bool HasFaded()
    {
        return _fadeComplete;
    }

    /// <summary>
    /// Initiates fading and/or scene transitions depending using Unity inspector input.
    /// </summary>
    private void Awake()
    {
        // Create the texture that will overlay the screen during scene transitions.
        _overlayTexture = CreateTexture(1, 1, Color.black);
        
        if (_fadeIn)
        {
            // Scene requires fading in, make the texture fully opaque and fade it in.
            _overlayColour.a = OpacityMax;
            StartCoroutine(FadeOverlayAlpha(OpacityMin, _fadeInTimeInSeconds)); 
        }
    }

    /// <summary>
    /// Fades the opacity of the overlaying texture to the given value over a period of time.
    /// </summary>
    /// <param name="value">desired opacity value</param>
    /// <param name="time">fade time in seconds</param>
    private IEnumerator FadeOverlayAlpha(float value, float time)
    {
        _fadeComplete = false;
        var alpha = _overlayColour.a;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            _overlayColour.a = Mathf.Lerp(alpha, value, t);
            Debug.LogFormat("Overlay Alpha: {0}", _overlayColour.a);
            yield return null;
        }
        _fadeComplete = true;
    }

    /// <summary>
    /// Draws the overlaying texture to the screen.
    /// </summary>
    private void OnGUI()
    {
        GUI.color = _overlayColour;
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), _overlayTexture);
    }
    
}