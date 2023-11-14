using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using Unity.Collections;
using UnityEngine.UI;

public class CameraImageAccess : MonoBehaviour
{
    private ARCameraManager arCameraManager;
    private Texture2D texture2D;
    public Texture2D errorTexture;
    private XRCpuImage.ConversionParams conversionParams;

    void Awake()
    {
        // 카메라 매니저 컴포넌트를 참조
        arCameraManager = GetComponent<ARCameraManager>();
    }

    void OnEnable()
    {
        // 카메라 프레임이 갱신될 때마다 이벤트 구독
        arCameraManager.frameReceived += OnCameraFrameReceived;
    }

    void OnDisable()
    {
        // 이벤트 구독 해제
        arCameraManager.frameReceived -= OnCameraFrameReceived;
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        // CPU 이미지를 얻기 시도
        if (!arCameraManager.TryAcquireLatestCpuImage(out XRCpuImage image)) {
            return;
        }

        // 변환 매개변수 설정
        conversionParams = new XRCpuImage.ConversionParams {
            // 이미지 전체를 사용
            inputRect = new RectInt(0, 0, image.width, image.height),
            // 출력 텍스처 크기 설정
            outputDimensions = new Vector2Int(image.width, image.height),
            // RGBA32 포맷으로 설정
            outputFormat = TextureFormat.RGBA32,
            // 이미지 회전 없음
            transformation = XRCpuImage.Transformation.None
        };

        // Texture2D 생성 혹은 재사용
        if (texture2D == null || texture2D.width != image.width || texture2D.height != image.height) {
            texture2D = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
        }

        // 네이티브 배열로 이미지 데이터를 RGBA32 포맷으로 변환
        var rawData = new NativeArray<byte>(image.GetConvertedDataSize(conversionParams), Allocator.Temp);
        image.Convert(conversionParams, rawData);

        // Texture2D에 데이터 적용
        texture2D.LoadRawTextureData(rawData);
        texture2D.Apply();

        // 네이티브 배열과 이미지 리소스를 정리
        rawData.Dispose();
        image.Dispose();
    }

    public RawImage rawImageDisplay; // Inspector에서 할당해야 함

    private void UpdateRawImageTexture()
    {
        if (texture2D != null) {
            rawImageDisplay.texture = texture2D;
        } else {
            rawImageDisplay.texture = errorTexture;
        }
    }

    void Update()
    {
        UpdateRawImageTexture();
    }

}
