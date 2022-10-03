using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;
using CrossPromo.Scripts;

namespace CrossPromo
{
    public class VideoItem
    {
        private const string FILE_NAME_TEMPLATE = "{0}.mp4";
        private const string FOLDER_NAME = "CrossPromo";
        private const string REGEX_MATCH_PLAYER_ID = @"\[PLAYER_ID\]";

        private readonly VideoAdResponse _response;
        private readonly Action _onDownloadSuccess;
        private readonly Action<int> _onDownloadFailed;
        private bool _trackingSent = false;

        public int Id => _response.id;
        public string LocalUrl { get; private set; }

        public VideoItem(VideoAdResponse response, Action onDownloadSuccess, Action<int> onDownloadFailed)
        {
            _response = response;
            _onDownloadSuccess = onDownloadSuccess;
            _onDownloadFailed = onDownloadFailed;
            WebRequests.Get(WebRequestType.Video, _response.video_url, OnError, onSuccessVideo: VideoDownloaded);
        }

        public void DeleteVideo()
        {
            if(File.Exists(LocalUrl))
            {
                File.Delete(LocalUrl);
            }
        }

        public void OnButtonClick(string playerId)
        {
            if (!_trackingSent)
            {
                string trackingUrl = Regex.Replace(_response.tracking_url, 
                    REGEX_MATCH_PLAYER_ID, playerId);

                WebRequests.Get(WebRequestType.Tracking,trackingUrl,OnErrorTracking,onSuccessTracking:TrackingSuccess);
                _trackingSent = true;
            }
            Application.OpenURL(_response.click_url);
        }

        private static void TrackingSuccess()
        {
            Debug.Log("Tracking Success");
        }

        private void OnErrorTracking(string message)
        {
            _trackingSent = false;
            Debug.LogError("Error tracking with message: " + message);
        }
        
        private void VideoDownloaded(byte[] video)
        {
            Debug.Log("===>>> Video downloaded Success id: " + _response.id);
            string fileName = string.Format(FILE_NAME_TEMPLATE, _response.id);
            string directory = Path.Combine(Application.persistentDataPath, FOLDER_NAME);

            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            LocalUrl = Path.Combine(directory, fileName);
            

            
            
            try
            {
                WriteByteArrayToFile(LocalUrl, video);
                _onDownloadSuccess?.Invoke();
            }
    
            catch (Exception)
            {
                Debug.LogError("Error saving video id: " + _response.id + "continue to next video");
                _onDownloadFailed?.Invoke(_response.id);
            }
        }

        private void OnError(string message)
        {
            _onDownloadFailed?.Invoke(_response.id);

            Debug.LogError("Error downloading video id: " + _response.id + "error message: " + message);
        }

        private static void WriteByteArrayToFile(string fileName, byte[] data)
        {
            var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
        }

        
    }

}
