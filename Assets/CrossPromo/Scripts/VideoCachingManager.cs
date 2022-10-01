using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace CrossPromo
{
    public class VideoCachingManager : MonoBehaviour
    {
        public void CacheVideosFromUrl(string url, Action<List<VideoItem>> onSuccess)
        {
            List<VideoItem> chachedVideoItemsList;
            int downloadedVideoCount = 0;

            WebRequests.Get(WebRequestType.Text, url, OnError, TextDownloaded);

            void TextDownloaded(string text)
            {
                Debug.Log("===>>> Success got text response: " + text);

                VideoAdsListResponse videoAdsList = JsonHelper.FromJson<VideoAdsListResponse>(text);

                if (videoAdsList.results != null)
                {
                    Debug.Log("===>>> Parse Json to VideoAdsListResponse Success. number of ads:  " + videoAdsList.results.Length);

                    int adsAmount = videoAdsList.results.Length;

                    chachedVideoItemsList = new List<VideoItem>();

                    for (int i = 0; i < adsAmount; i++)
                    {
                        VideoItem videoItem = new VideoItem(videoAdsList.results[i], OnVideoDownloadComplete, OnVideoDownloadFailed);
                        chachedVideoItemsList.Add(videoItem);
                    }
                }
                else
                {
                    Debug.LogError("===>>> Error parsing video ads data");
                }

            }

            void OnVideoDownloadComplete()
            {
                downloadedVideoCount++;

                if (downloadedVideoCount == chachedVideoItemsList.Count)
                {
                    if(onSuccess != null)
                    {
                       onSuccess(chachedVideoItemsList);
                    }   
                }
            }

            void OnVideoDownloadFailed(int id)
            {
                chachedVideoItemsList.RemoveAll(videoItem => videoItem.Id == id);

                if (downloadedVideoCount == chachedVideoItemsList.Count)
                {
                    if (onSuccess != null)
                    {
                        onSuccess(chachedVideoItemsList);
                    }
                }
            }

            void OnError(string message)
            {
                Debug.LogError("===>>> Error getting video ads data  " + message);
            }

        }
    }
}