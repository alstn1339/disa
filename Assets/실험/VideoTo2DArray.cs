using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Video;

public class VideoTo2DArray : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 비디오 플레이어 컴포넌트
    private Color[,] frameData2D;    // 프레임 데이터를 저장할 2D 배열
    private float interval = 3.0f;   // 처리 간격 (3초)
    private float timer = 0f;        // 타이머

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
        RenderTexture renderTexture = videoPlayer.texture as RenderTexture; // 현재 프레임 텍스처 가져오기
        if (renderTexture != null) // 텍스처가 있으면
        {
            // 현재 프레임을 Texture2D로 변환
            Texture2D currentFrame = new Texture2D(renderTexture.width, renderTexture.height);
            RenderTexture.active = renderTexture;
            currentFrame.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            currentFrame.Apply();

            ConvertTo2DArray(currentFrame); // 2D 배열로 변환
        }

        yield return null;

        if (frameData2D != null)
        {
            Print2DArray(frameData2D);
        }
    }

    void ConvertTo2DArray(Texture2D texture) // Texture2D를 2D 배열로 변환하는 메서드
    {
        if (texture == null) return; // 텍스처가 없으면 리턴

        Color[] pixels = texture.GetPixels(); // 텍스처의 픽셀 데이터 가져오기
        int width = texture.width; // 텍스처의 너비
        int height = texture.height; // 텍스처의 높이

        frameData2D = new Color[width, height]; // 2D 배열 초기화
        for (int x = 0; x < width; x++) // 너비만큼 반복
        {
            for (int y = 0; y < height; y++) // 높이만큼 반복
            {
                frameData2D[x, y] = pixels[y * width + x]; // 픽셀 데이터를 2D 배열에 저장
            }
        }
    }

    void Print2DArray(Color[,] array) // 2D 배열을 출력하는 메서드
    {
        int width = array.GetLength(0); // 배열의 너비
        int height = array.GetLength(1); // 배열의 높이

        StringBuilder sb = new StringBuilder(); // 문자열 생성기
        for (int y = 0; y < height; y++) // 높이만큼 반복
        {
            for (int x = 0; x < width; x++) // 너비만큼 반복
            {
                sb.Append(array[x, y]).Append(" "); // 배열의 값과 공백을 추가
            }
            sb.AppendLine(); // 한 줄 띄우기
        }
        Debug.Log(sb.ToString()); // 문자열 출력
    }
}
