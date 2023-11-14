using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class CameraManagerScript : MonoBehaviour
{
    WebCamTexture camTexture;

    public RawImage cameraViewImage; //ī�޶� ������ ȭ��

    public void CameraOn() //ī�޶� �ѱ�
    {
        //ī�޶� ���� Ȯ��
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        if (WebCamTexture.devices.Length == 0) //ī�޶� ���ٸ�..
        {
            Debug.Log("no camera!");
            return;
        }

        WebCamDevice[] devices = WebCamTexture.devices; //����Ʈ���� ī�޶� ������ ��� ������.
        int selectedCameraIndex = -1;

        //�ĸ� ī�޶� ã��
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                selectedCameraIndex = i;
                break;
            }
        }

        //ī�޶� �ѱ�
        if (selectedCameraIndex >= 0)
        {
            //���õ� �ĸ� ī�޶� ������.
            camTexture = new WebCamTexture(devices[selectedCameraIndex].name);

            camTexture.requestedFPS = 30; //ī�޶� �����Ӽ���

            cameraViewImage.texture = camTexture; //������ raw Image�� �Ҵ�.

            camTexture.Play(); // ī�޶� �����ϱ�
        }

    }

    public void CameraOff() //ī�޶� ����
    {
        if (camTexture != null)
        {
            camTexture.Stop(); //ī�޶� ����
            WebCamTexture.Destroy(camTexture); //ī�޶� ��ü�ݳ�
            camTexture = null; //���� �ʱ�ȭ
        }
    }
}