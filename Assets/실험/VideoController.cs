using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // �����̽��ٸ� ������
        {
            if (videoPlayer.isPlaying) // ������ ��� ���̸�
            {
                videoPlayer.Pause(); // ������ �Ͻ�����
            }
            else
            {
                videoPlayer.Play(); // ������ ���
            }
        }
    }
}

