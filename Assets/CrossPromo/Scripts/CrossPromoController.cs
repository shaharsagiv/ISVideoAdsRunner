using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


namespace CrossPromo.Scripts
{
    public class CrossPromoController : MonoBehaviour
    {
        private const string URL = "https://run.mocky.io/v3/082a08da-9c8e-49f9-afc9-0a154728fc17";

        [SerializeField]
        private VideoPlayer videoPlayer;
        
        private List<VideoItem> _videoItemsList;
        private int _videoIndex = 0;
        private string _playerId;

        public void DownloadAndPlayVideos(string playerId)
        {
            if (videoPlayer != null)
            {
                VideoCachingManager.CacheVideosFromUrl(URL, OnVideosDownloaded);
                videoPlayer.loopPointReached -= LoopPointReached;
                videoPlayer.loopPointReached += LoopPointReached;
                _playerId = playerId;
            }
            else
            {
                Debug.LogError("CrossPromoController videoPlayer is unassigned!");
            }
        }

        public void SetPlayerId(string playerId)
        {
            _playerId = playerId;
        }
        
        public void Next()
        {
            if(_videoItemsList != null && videoPlayer != null)
            {
                videoPlayer.Stop();
                _videoIndex = (_videoIndex + 1) % _videoItemsList.Count;
                PlayVideo(_videoItemsList[_videoIndex].LocalUrl);
            }
        }

        public void Previous()
        {
            if (_videoItemsList != null && videoPlayer != null)
            {
                videoPlayer.Stop();
                _videoIndex = (_videoItemsList.Count + _videoIndex + -1) % _videoItemsList.Count;
                PlayVideo(_videoItemsList[_videoIndex].LocalUrl);
            }
        }

        public void Pause()
        {
            if(videoPlayer != null && videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
        }

        public void Resume()
        {
            if (videoPlayer != null && videoPlayer.isPaused)
            {
                videoPlayer.Play();
            }
        }

        public void OnButtonClick()
        {
            _videoItemsList?[_videoIndex].OnButtonClick(_playerId);
        }
        

        private void OnVideosDownloaded(List<VideoItem> videoItemsList)
        {
            _videoItemsList = videoItemsList;
            if(videoItemsList != null)
            {
                PlayVideo(_videoItemsList[_videoIndex].LocalUrl);
            }
        }
        
        private void OnDestroy()
        {
            foreach(VideoItem videoItem in _videoItemsList)
            {
                videoItem.DeleteVideo();
            }
        }

        private void LoopPointReached(VideoPlayer player)
        {
            Next();
        }


        private void PlayVideo(string url)
        {
            if(videoPlayer != null && !videoPlayer.isPlaying && !videoPlayer.isPaused)
            {
                videoPlayer.url = url;
                videoPlayer.source = VideoSource.Url;
            }
        }
    }
}
