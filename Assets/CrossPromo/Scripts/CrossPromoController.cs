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
        int _videoIndex;

        public VideoPlayer videoPlayer;

        // Start is called before the first frame update
        public void DownloadAndPlayVideos(CrossPromoSettings settings)
        {
            var videoCachigManager = new VideoCachingManager();
            videoCachigManager.CacheVideosFromUrl(URL, OnVideosDownloaded);
        }

        public void Next()
        {

        }

        public void Previous()
        {

        }

        public void Pause()
        {

        }

        public void Resume()
        {

        }

        private void OnVideosDownloaded(List<VideoItem> videoItemsList)
        {
            _videoItemsList = videoItemsList;
            Debug.Log("======>>>>> WOW");

            videoPlayer.url = _videoItemsList[0].LocalUrl;
            videoPlayer.source = VideoSource.Url;
            
        }

        void OnDestroy()
        {
            foreach(VideoItem videoItem in _videoItemsList)
            {
                videoItem.DeleteVideo();
            }
        }

    }
}