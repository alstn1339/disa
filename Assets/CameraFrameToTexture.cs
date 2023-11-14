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
        // ī�޶� �Ŵ��� ������Ʈ�� ����
        arCameraManager = GetComponent<ARCameraManager>();
    }

    void OnEnable()
    {
        // ī�޶� �������� ���ŵ� ������ �̺�Ʈ ����
        arCameraManager.frameReceived += OnCameraFrameReceived;
    }

    void OnDisable()
    {
        // �̺�Ʈ ���� ����
        arCameraManager.frameReceived -= OnCameraFrameReceived;
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        // CPU �̹����� ��� �õ�
        if (!arCameraManager.TryAcquireLatestCpuImage(out XRCpuImage image)) {
            return;
        }

        // ��ȯ �Ű����� ����
        conversionParams = new XRCpuImage.ConversionParams {
            // �̹��� ��ü�� ���
            inputRect = new RectInt(0, 0, image.width, image.height),
            // ��� �ؽ�ó ũ�� ����
            outputDimensions = new Vector2Int(image.width, image.height),
            // RGBA32 �������� ����
            outputFormat = TextureFormat.RGBA32,
            // �̹��� ȸ�� ����
            transformation = XRCpuImage.Transformation.None
        };

        // Texture2D ���� Ȥ�� ����
        if (texture2D == null || texture2D.width != image.width || texture2D.height != image.height) {
            texture2D = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
        }

        // ����Ƽ�� �迭�� �̹��� �����͸� RGBA32 �������� ��ȯ
        var rawData = new NativeArray<byte>(image.GetConvertedDataSize(conversionParams), Allocator.Temp);
        image.Convert(conversionParams, rawData);

        // Texture2D�� ������ ����
        texture2D.LoadRawTextureData(rawData);
        texture2D.Apply();

        // ����Ƽ�� �迭�� �̹��� ���ҽ��� ����
        rawData.Dispose();
        image.Dispose();
    }

    public RawImage rawImageDisplay; // Inspector���� �Ҵ��ؾ� ��

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
