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
    private float _brushSize = 0.5f;

    public Camera dummyCam;

    private void Start()
    {
        PaintWhite();
        SelectBrush(0);
        percentageText.text = "0%";
        _texture = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGB24, false);
        _slider.onValueChanged.AddListener(val => _brushSize = val);
    }

    private void Update()
    {
        if (!GameManager.Instance.isGameOver) return;

        if (Input.GetMouseButton(0))
        {
            Ray ray = dummyCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, collideWith))
            {
                if (!brushes[_selectedBrushID].activeInHierarchy)
                {
                    brushes[_selectedBrushID].SetActive(true);
                    brushes[_selectedBrushID].transform.localScale = new Vector3(_brushSize, _brushSize, _brushSize);
                }
                brushes[_selectedBrushID].transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            brushes[_selectedBrushID].SetActive(false);
        }

        CalculatePercent();
    }

    private void CalculatePercent(int interval = 30, int step = 5)
    {
        if (Time.frameCount % interval == 0)
        {
            RenderTexture.active = _renderTexture;
            _texture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            _texture.Apply();

            Color[] pixels = _texture.GetPixels();

            int totalPixels = pixels.Length / (step * step);
            int paintedPixels = 0;

            for (int y = 0; y < _texture.height; y += step)
            {
                for (int x = 0; x < _texture.width; x += step)
                {
                    Color pixelColor = _texture.GetPixel(x, y);
                    if (pixelColor != Color.white)
                    {
                        paintedPixels++;
                    }
                }
            }

            float percent = (float)paintedPixels / totalPixels * 100;
            percent = Mathf.Clamp(percent, 0, 100);

            percentageText.text = (int)percent + "%";
        }
    }

    public void SelectBrush(int id)
    {
        _selectedBrushID = id;
        brushes[_selectedBrushID].transform.localScale = new Vector3(_brushSize, _brushSize, _brushSize);
    }

    public void SetBrushSize(float size)
    {
        brushes[_selectedBrushID].transform.localScale = new Vector3(size, size, size);
    }

    private void PaintWhite()
    {
        Texture2D texture = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGB24, false);

        RenderTexture.active = _renderTexture;
        texture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
        texture.Apply();

        Color[] pixels = texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        texture.SetPixels(pixels);
        texture.Apply();

        Graphics.Blit(texture, _renderTexture);

        RenderTexture.active = null;
    }
}
