using System.Collections;
using UnityEngine;

public class FadeManager : MonoBehaviour {
    
    [Header("[ FADING IN ]")]
    [SerializeField] private bool _fadeIn;

    private const float OpacityMax = 1.0f;
    private const float OpacityMin = 0.0f;
    private const float FadeTimeDefault = 1.0f;
       
    private Texture _overlayTexture;    
    private Color _overlayColour;   
    private bool _fadeComplete;

    public void FadeIn()
    {
        FadeIn(FadeTimeDefault);
    }
    
    public void FadeIn(float fadeTime)
    {
        _overlayColour.a = OpacityMax;
        StartCoroutine(FadeOverlayAlpha(OpacityMin, fadeTime));
    }

    public void FadeOut()
    {
        FadeOut(FadeTimeDefault);
    }

    public void FadeOut(float fadeTime)
    {
        StartCoroutine(FadeOverlayAlpha(OpacityMax, fadeTime));
    }
    
    public bool HasFaded()
    {
        return _fadeComplete;
    }
    
    private void Start()
    {
        _overlayTexture = CreateTexture(1, 1, Color.black);

        if (!_fadeIn) return;
        
        FadeIn();
    }
    
    
    private void OnGUI()
    {
        GUI.color = _overlayColour;
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), _overlayTexture);
    }
 
    private static Texture2D CreateTexture(int width, int height, Color32 colour)
    {
        var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        for (var y = 0; y < texture.height; y++)
            for (var x = 0; x < texture.width; x++)
                texture.SetPixel(x, y, colour);
        texture.Apply();
        return texture;
    }

    private IEnumerator FadeOverlayAlpha(float value, float time)
    {
        _fadeComplete = false;
        var alpha = _overlayColour.a;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            _overlayColour.a = Mathf.Lerp(alpha, value, t);
            yield return null;
        }
        _fadeComplete = true;
    }
}
