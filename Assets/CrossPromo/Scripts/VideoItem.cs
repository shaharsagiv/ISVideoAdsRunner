using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CrossPromo
{
    public class VideoItem
    {
        VideoAdResponse _response;
        byte[] _video;
        Action _onDownloadSuccess;
        Action<int> _onDownloadFailed;

        public int Id => _response.id;

        public VideoItem(VideoAdResponse response, Action onDownloadSuccess, Action<int> onDownloadFailed)
        {
            _response = response;
            _onDownloadSuccess = onDownloadSuccess;
            _onDownloadFailed = onDownloadFailed;
            WebRequests.Get(WebRequestType.Video, _response.video_url, OnError, onSuccessVideo: VideoDownloaded);
        }

        void VideoDownloaded(byte[] video)
        {
            Debug.Log("===>>> Video downloaded Success id: " + _response.id);
            _video = video;

            if(_onDownloadSuccess != null)
            {
                _onDownloadSuccess();
            }
        }

        void OnError(string message)
        {
            if (_onDownloadFailed != null)
            {
                _onDownloadFailed(_response.id);
            }

            Debug.LogError("Error downloading video id: "+ _response.id + "error message: " + message);
        }

    }

}
