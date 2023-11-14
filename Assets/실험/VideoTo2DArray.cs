using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Video;

public class VideoTo2DArray : MonoBehaviour
{
    public VideoPlayer videoPlayer; // ���� �÷��̾� ������Ʈ
    private Color[,] frameData2D;    // ������ �����͸� ������ 2D �迭
    private float interval = 3.0f;   // ó�� ���� (3��)
    private float timer = 0f;        // Ÿ�̸�

    void Update()
    {
        if (videoPlayer.isPlaying)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0f;
                StartCoroutine(ExtractFrameDataCoroutine());
            }
        }
    }

    IEnumerator ExtractFrameDataCoroutine()
    {
        RenderTexture renderTexture = videoPlayer.texture as RenderTexture; // ���� ������ �ؽ�ó ��������
        if (renderTexture != null) // �ؽ�ó�� ������
        {
            // ���� �������� Texture2D�� ��ȯ
            Texture2D currentFrame = new Texture2D(renderTexture.width, renderTexture.height);
            RenderTexture.active = renderTexture;
            currentFrame.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            currentFrame.Apply();

            ConvertTo2DArray(currentFrame); // 2D �迭�� ��ȯ
        }

        yield return null;

        if (frameData2D != null)
        {
            Print2DArray(frameData2D);
        }
    }

    void ConvertTo2DArray(Texture2D texture) // Texture2D�� 2D �迭�� ��ȯ�ϴ� �޼���
    {
        if (texture == null) return; // �ؽ�ó�� ������ ����

        Color[] pixels = texture.GetPixels(); // �ؽ�ó�� �ȼ� ������ ��������
        int width = texture.width; // �ؽ�ó�� �ʺ�
        int height = texture.height; // �ؽ�ó�� ����

        frameData2D = new Color[width, height]; // 2D �迭 �ʱ�ȭ
        for (int x = 0; x < width; x++) // �ʺ�ŭ �ݺ�
        {
            for (int y = 0; y < height; y++) // ���̸�ŭ �ݺ�
            {
                frameData2D[x, y] = pixels[y * width + x]; // �ȼ� �����͸� 2D �迭�� ����
            }
        }
    }

    void Print2DArray(Color[,] array) // 2D �迭�� ����ϴ� �޼���
    {
        int width = array.GetLength(0); // �迭�� �ʺ�
        int height = array.GetLength(1); // �迭�� ����

        StringBuilder sb = new StringBuilder(); // ���ڿ� ������
        for (int y = 0; y < height; y++) // ���̸�ŭ �ݺ�
        {
            for (int x = 0; x < width; x++) // �ʺ�ŭ �ݺ�
            {
                sb.Append(array[x, y]).Append(" "); // �迭�� ���� ������ �߰�
            }
            sb.AppendLine(); // �� �� ����
        }
        Debug.Log(sb.ToString()); // ���ڿ� ���
    }
}
