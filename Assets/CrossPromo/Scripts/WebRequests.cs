using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

namespace CrossPromo
{
    public enum WebRequestType
    {
        Text = 1,
        Texture = 2,
        Video = 3
    }

    public class WebRequests
    {
        public static async void Get(WebRequestType requestType,
            string url, Action<string> onError, Action<string> onSuccessText = null,
            Action<Texture2D> onSuccessTexture = null, Action<byte[]> onSuccessVideo = null)
        {
            if (onSuccessText == null && onSuccessTexture == null && onSuccessVideo == null)
            {
                Debug.LogError($" must assign at least one of the optional success delegates");
            }

            switch (requestType)
            {
                case WebRequestType.Text:
                    using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
                    {
                        await ProcessRequest(requestType, unityWebRequest);

                        if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
                        {
                            onError(unityWebRequest.error);
                        }
                        else
                        {
                            onSuccessText?.Invoke(unityWebRequest.downloadHandler.text);
                        }
                    }
                    break;
                case WebRequestType.Texture:
                {
                    using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url))
                    {
                        await ProcessRequest(requestType, unityWebRequest);
                        if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
                        {
                            onError(unityWebRequest.error);
                        }
                        else
                        {
                            DownloadHandlerTexture downloadHandlerTexture =
                                unityWebRequest.downloadHandler as DownloadHandlerTexture;
                            onSuccessTexture?.Invoke(downloadHandlerTexture.texture);
                        }
                        break;
                    }
                }
                case WebRequestType.Video:
                {
                    using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url))
                    { 
                        await ProcessRequest(requestType, unityWebRequest);
                        if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
                        {
                            onError(unityWebRequest.error);
                        }
                        else
                        {
                            onSuccessVideo?.Invoke(unityWebRequest.downloadHandler.data);
                        }
                    }
                    break;
                }
            }
        }

        private static async Task ProcessRequest(WebRequestType requestType, UnityWebRequest unityWebRequest)
        {
            var operation = unityWebRequest.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
                Debug.Log($"operation {requestType} is still handled. progress is {operation.progress}");
            }
        }
    }
}