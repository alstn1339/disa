using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NpyLoader : MonoBehaviour
{
    public string filePath = "file_900x1200x3.bin";
    public RawImage rawImage;

    public int width = 1200;
    public int height = 900;
    public int channels = 3;

    void Start()
    {
        byte[] bytes = File.ReadAllBytes(Path.Combine(Application.dataPath, filePath));

        string[] dimensions = Path.GetFileNameWithoutExtension(filePath).Split('_')[1].Split('x');
        width = int.Parse(dimensions[1]);
        height = int.Parse(dimensions[0]);
        channels = 3;
        Debug.Log("width: " + width);
        Debug.Log("height: " + height);

        Texture2D texture = new Texture2D(width, height);
        Color[] colors = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float r = bytes[(height - 1 - y) * width * channels + x * channels] / 255f;
                float g = bytes[(height - 1 - y) * width * channels + x * channels + 1] / 255f;
                float b = bytes[(height - 1 - y) * width * channels + x * channels + 2] / 255f;
                colors[y * width + x] = new Color(r, g, b);
            }
        }
        texture.SetPixels(colors);
        texture.Apply();

        // 화면에 출력
        rawImage.texture = texture;
        rawImage.rectTransform.sizeDelta = new Vector2(width, height);
        Debug.Log("Texture width: " + texture.width + ", height: " + texture.height);
        Debug.Log("RawImage texture: " + rawImage.texture);
    }
}




