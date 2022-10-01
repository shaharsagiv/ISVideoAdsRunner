using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


namespace CrossPromo
{
    public class CrossPromoController : MonoBehaviour
    {
        const string URL = "https://run.mocky.io/v3/082a08da-9c8e-49f9-afc9-0a154728fc17";

        List<VideoItem> _videoItemsList;
        int _videoIndex = 0;

        public VideoPlayer videoPlayer;

        // Start is called before the first frame update
        public void DownloadAndPlayVideos(CrossPromoSettings settings)
        {
            var videoCachigManager = new VideoCachingManager();
            videoCachigManager.CacheVideosFromUrl(URL, OnVideosDownloaded);
            videoPlayer.loopPointReached -= LoopPointReached;
            videoPlayer.loopPointReached += LoopPointReached;
        }

        public void Next()
        {
            if(_videoItemsList != null)
            {
                _videoIndex = (_videoIndex + 1) % _videoItemsList.Count;
                PlayVideo(_videoItemsList[_videoIndex].LocalUrl);
            }
        }

        public void Previous()
        {
            if (_videoItemsList != null)
            {
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


        private void OnVideosDownloaded(List<VideoItem> videoItemsList)
        {
            if(videoItemsList != null)
            {
                _videoItemsList = videoItemsList;
                Debug.Log("======>>>>> WOW");

                PlayVideo(_videoItemsList[_videoIndex].LocalUrl);
            }
        }

        void OnDestroy()
        {
            foreach(VideoItem videoItem in _videoItemsList)
            {
                videoItem.DeleteVideo();
            }
        }

        void LoopPointReached(VideoPlayer videoPlayer)
        {
            Next();
        }


        void PlayVideo(string url)
        {
            if(videoPlayer != null)
            {
                videoPlayer.url = url;
                videoPlayer.source = VideoSource.Url;
            }
        }
    }
}