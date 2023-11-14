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
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 누르면
        {
            if (videoPlayer.isPlaying) // 비디오가 재생 중이면
            {
                videoPlayer.Pause(); // 비디오를 일시정지
            }
            else
            {
                videoPlayer.Play(); // 비디오를 재생
            }
        }
    }
}

