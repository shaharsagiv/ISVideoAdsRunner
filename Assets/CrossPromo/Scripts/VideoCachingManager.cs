using System.Collections.Generic;
using UnityEngine;
using System;


namespace CrossPromo.Scripts
{
    public static class VideoCachingManager
    {
        public static void CacheVideosFromUrl(string url, Action<List<VideoItem>> onSuccess)
        {
            List<VideoItem> cachedVideoItemsList;
            int videoIndex = 0;
            VideoAdsListResponse videoAdsList;

            WebRequests.Get(WebRequestType.Text, url, OnError, TextDownloaded);

            void TextDownloaded(string text)
            {
                Debug.Log("===>>> Success got text response: " + text);
                videoAdsList = JsonUtility.FromJson<VideoAdsListResponse>(text);
                
                
                if (videoAdsList.results != null)
                {
                    Debug.Log("===>>> Parse Json to VideoAdsListResponse Success. number of ads:  " 
                              + videoAdsList.results.Length);

                    int adsAmount = videoAdsList.results.Length;

                    cachedVideoItemsList = new List<VideoItem>();

                    if (videoIndex < adsAmount)
                    {
                        var videoItem = new VideoItem(videoAdsList.results[videoIndex], 
                            OnVideoDownloadComplete, OnVideoDownloadFailed);
                        cachedVideoItemsList.Add(videoItem);
                    }

                }
                else
                {
                    Debug.LogError("===>>> Error parsing video ads data");
                }

            }

            void OnVideoDownloadComplete()
            {
                videoIndex++;

                onSuccess?.Invoke(cachedVideoItemsList);

                if (videoIndex < videoAdsList.results.Length)
                {
                    var videoItem = new VideoItem(videoAdsList.results[videoIndex], 
                        OnVideoDownloadComplete, OnVideoDownloadFailed);
                    cachedVideoItemsList.Add(videoItem);
                }
            }

            void OnVideoDownloadFailed(int id)
            {
                cachedVideoItemsList.RemoveAll(videoItem => videoItem.Id == id);
                var videoItem = new VideoItem(videoAdsList.results[videoIndex], 
                    OnVideoDownloadComplete, OnVideoDownloadFailed);
                cachedVideoItemsList.Add(videoItem);
            }

            void OnError(string message)
            {
                Debug.LogError("===>>> Error getting video ads data  " + message);
            }
        }
    }
}