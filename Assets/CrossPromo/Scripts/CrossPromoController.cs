using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CrossPromo
{
    public class CrossPromoController : MonoBehaviour
    {
        string url = "https://run.mocky.io/v3/082a08da-9c8e-49f9-afc9-0a154728fc17";

        List<VideoItem> _videoItemsList;
        int _downloadedVideoCount = 0;

        // Start is called before the first frame update
        void Start()
        {
            WebRequests.Get(WebRequestType.Text, url, OnError, GetText);
        }

        void GetText(string text)
        {
            Debug.Log("===>>> Success got text response: " + text);

            VideoAdsListResponse videoAdsList = JsonHelper.FromJson<VideoAdsListResponse>(text);

            if(videoAdsList.results != null)
            {
                Debug.Log("===>>> Parse Json to VideoAdsListResponse Success. number of ads:  " + videoAdsList.results.Length);

                int adsAmount = videoAdsList.results.Length;

                _videoItemsList = new List<VideoItem>();
           
                for (int i = 0 ; i < adsAmount ; i++)
                {
                    VideoItem videoItem = new VideoItem(videoAdsList.results[i], OnVideoDownloadComplete, OnVideoDownloadFailed);
                    _videoItemsList.Add(videoItem);
                }
            }
            else
            {
                Debug.LogError("===>>> Error parsing video ads data");
            }

        }

        void OnVideoDownloadComplete()
        {
            _downloadedVideoCount++;

            if(_downloadedVideoCount == _videoItemsList.Count)
            {

            }
        }

        void OnVideoDownloadFailed(int id)
        {
            _videoItemsList.RemoveAll(videoItem => videoItem.Id == id);

            if (_downloadedVideoCount == _videoItemsList.Count)
            {

            }
        }

        void OnError(string message)
        {
            Debug.LogError("===>>> Error parsing video ads data  " + message);
        }
    }
}