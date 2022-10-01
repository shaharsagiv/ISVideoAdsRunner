using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace CrossPromo
{
    public class VideoItem
    {
        const string FILE_NAME_TEMPLATE = "{0}.mp4";
        const string FOLDER_NAME = "CrossPromo";

        VideoAdResponse _response;
        byte[] _video;
        Action _onDownloadSuccess;
        Action<int> _onDownloadFailed;
        string _localUrl;

        public int Id => _response.id;
        public string LocalUrl => _localUrl;

        public VideoItem(VideoAdResponse response, Action onDownloadSuccess, Action<int> onDownloadFailed)
        {
            _response = response;
            _onDownloadSuccess = onDownloadSuccess;
            _onDownloadFailed = onDownloadFailed;
            WebRequests.Get(WebRequestType.Video, _response.video_url, OnError, onSuccessVideo: VideoDownloaded);
        }

        public void DeleteVideo()
        {
            if(File.Exists(_localUrl))
            {
                File.Delete(_localUrl);
            }
        }

        void VideoDownloaded(byte[] video)
        {
            Debug.Log("===>>> Video downloaded Success id: " + _response.id);
            _video = video;
            string fileName = string.Format(FILE_NAME_TEMPLATE, _response.id);
            string directory = Path.Combine(Application.persistentDataPath, FOLDER_NAME);

            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _localUrl = Path.Combine(directory, fileName);
            WriteByteArrayToFile(_localUrl, video);

            if (_onDownloadSuccess != null)
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

        void WriteByteArrayToFile(string fileName, byte[] data)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            fileStream.Write(data, 0, data.Length);
        }

        
    }

}
