using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw : MonoBehaviour
{
    public RenderTexture _renderTexture;
    public LayerMask collideWith;
    public GameObject[] brushes;
    private int _selectedBrushID;
    public Text percentageText;
    private Texture2D _texture;
    public Slider _slider;
    private float _brushSize = 1f;
    
    private Material _drawMaterial;
    private RenderTexture _tempTexture;
    private Vector2 _lastDrawPosition;
    private bool _isDrawing = false;
    private float _rainbowTime = 0f;
    private float _nextPercentageUpdate = 0f;
    private const float UPDATE_INTERVAL = 0.5f;
    private Texture2D _brushTexture;
    
    public Camera dummyCam;
    private Color[] _brushColors;

    private void Awake()
    {
        if (brushes == null || brushes.Length == 0)
        {
            Debug.LogError("Brushes array is not set in the inspector!");
            return;
        }
    }

    private void Start()
    {
        InitializeDrawing();
        _selectedBrushID = 0;
        percentageText.text = "0%";
        _slider.onValueChanged.AddListener(val => SetBrushSize(val));
        
        _brushColors = new Color[] {
            new Color(1, 0, 1, 1),      // Gökkuşağı başlangıç rengi
            new Color(0, 0, 1, 1),      // Mavi
            new Color(1, 0, 0, 1),      // Kırmızı
            new Color(1, 1, 0, 1),      // Sarı
            Color.white                  // Silgi (beyaz)
        };

        Shader drawShader = Shader.Find("Unlit/Texture");
        if (drawShader != null)
        {
            _drawMaterial = new Material(drawShader);
            _drawMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _drawMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        }
        else
        {
            Debug.LogError("Could not find Unlit/Texture shader!");
        }

        CreateBrushTexture();
        ConfigureRenderTextures();
    }

    private void ConfigureRenderTextures()
    {
        // Ana RenderTexture ayarları
        _renderTexture.filterMode = FilterMode.Point;
        _renderTexture.antiAliasing = 1;

        // Geçici RenderTexture ayarları
        _tempTexture.filterMode = FilterMode.Point;
        _tempTexture.antiAliasing = 1;
    }

    private void CreateBrushTexture()
    {
        int size = 32; // Daha küçük texture boyutu
        _brushTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        _brushTexture.filterMode = FilterMode.Point; // Keskin kenarlar için Point filtering
        
        float radius = size * 0.5f;
        Color[] colors = new Color[size * size];
        
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(radius, radius));
                float alpha = distance <= radius ? 1f : 0f; // Keskin kenarlar için
                colors[y * size + x] = new Color(1, 1, 1, alpha);
            }
        }
        
        _brushTexture.SetPixels(colors);
        _brushTexture.Apply();
    }

    private void Update()
    {
        if (!GameManager.Instance.isGameOver) return;

        HandleDrawing();
        
        if (Time.time >= _nextPercentageUpdate)
        {
            CalculatePercentOptimized();
            _nextPercentageUpdate = Time.time + UPDATE_INTERVAL;
        }
    }

    private void HandleDrawing()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = dummyCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, collideWith))
            {
                Vector2 drawPosition = new Vector2(hit.textureCoord.x * _renderTexture.width, 
                                                 hit.textureCoord.y * _renderTexture.height);
                
                if (!_isDrawing)
                {
                    _isDrawing = true;
                    _lastDrawPosition = drawPosition;
                }
                
                DrawLine(_lastDrawPosition, drawPosition, GetCurrentColor());
                _lastDrawPosition = drawPosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDrawing = false;
        }
    }

    private void DrawLine(Vector2 start, Vector2 end, Color color)
    {
        Graphics.Blit(_renderTexture, _tempTexture);
        
        float distance = Vector2.Distance(start, end);
        Vector2 direction = (end - start).normalized;
        
        float step = _brushSize * 2f; // Daha sık nokta yerleştirme
        int steps = Mathf.CeilToInt(distance / step);
        
        RenderTexture.active = _tempTexture;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, _tempTexture.width, 0, _tempTexture.height);
        
        float size = _brushSize * 30;
        
        // Keskin kenarlar için tam sayı pozisyonlama
        for (int i = 0; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector2 pos = Vector2.Lerp(start, end, t);
            
            // Pozisyonu tam sayılara yuvarlama
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            
            float halfSize = size * 0.5f;
            Rect brushRect = new Rect(pos.x - halfSize, pos.y - halfSize, size, size);
            
            Graphics.DrawTexture(brushRect, _brushTexture, new Rect(0, 0, 1, 1), 0, 0, 0, 0, color);
        }
        
        GL.PopMatrix();
        RenderTexture.active = null;
        
        Graphics.Blit(_tempTexture, _renderTexture);
    }

    private void CalculatePercentOptimized()
    {
        RenderTexture.active = _renderTexture;
        _texture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
        _texture.Apply();

        int step = 20; // Daha az sıklıkta kontrol
        int totalChecks = 0;
        int paintedChecks = 0;

        for (int y = 0; y < _texture.height; y += step)
        {
            for (int x = 0; x < _texture.width; x += step)
            {
                totalChecks++;
                Color pixelColor = _texture.GetPixel(x, y);
                
                if (!Mathf.Approximately(pixelColor.r, 1) || 
                    !Mathf.Approximately(pixelColor.g, 1) || 
                    !Mathf.Approximately(pixelColor.b, 1))
                {
                    paintedChecks++;
                }
            }
        }

        float percent = (float)paintedChecks / totalChecks * 100f;
        percent = Mathf.Clamp(percent, 0, 100);
        percentageText.text = $"{(int)percent}%";

        RenderTexture.active = null;
    }

    private Color GetCurrentColor()
    {
        if (_selectedBrushID >= _brushColors.Length)
        {
            Debug.LogWarning("Selected brush ID is out of range!");
            return Color.white;
        }

        if (_selectedBrushID == 0) // Gökkuşağı fırçası
        {
            _rainbowTime += Time.deltaTime;
            float hue = (_rainbowTime % 1f);
            return Color.HSVToRGB(hue, 1f, 1f);
        }
        
        return _brushColors[_selectedBrushID];
    }

    public void SelectBrush(int id)
    {
        if (id >= 0 && id < _brushColors.Length)
        {
            _selectedBrushID = id;
            _rainbowTime = 0f; // Gökkuşağı zamanını sıfırla
        }
        else
        {
            Debug.LogWarning($"Invalid brush ID: {id}. Must be between 0 and {_brushColors.Length - 1}");
        }
    }

    public void SetBrushSize(float size)
    {
        _brushSize = Mathf.Clamp(size, 1f, 8f);
    }

    private void PaintWhite()
    {
        RenderTexture.active = _renderTexture;
        GL.Clear(true, true, Color.white);
        
        Color[] pixels = new Color[_texture.width * _texture.height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        
        _texture.SetPixels(pixels);
        _texture.Apply();
        
        Graphics.Blit(_texture, _renderTexture);
        RenderTexture.active = null;
    }

    private void InitializeDrawing()
    {
        _texture = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, false);
        _texture.filterMode = FilterMode.Point;
        PaintWhite();
        
        _tempTexture = new RenderTexture(_renderTexture.width, _renderTexture.height, 0, RenderTextureFormat.ARGB32);
        _tempTexture.Create();
    }

    private void OnDestroy()
    {
        if (_tempTexture != null)
        {
            _tempTexture.Release();
        }
        if (_brushTexture != null)
        {
            Destroy(_brushTexture);
        }
    }
}
