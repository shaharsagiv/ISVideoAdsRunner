using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CrossPromo.Scripts
{
    public enum WebRequestType
    {
        Text = 1,
        Texture = 2,
        Video = 3,
        Tracking = 4
    }

    public static class WebRequests
    {
        public static async void Get(WebRequestType requestType,
            string url, Action<string> onError, Action<string> onSuccessText = null,
            Action<Texture2D> onSuccessTexture = null, Action<byte[]> onSuccessVideo = null,
            Action onSuccessTracking = null)
        {
            if (onSuccessText == null && onSuccessTexture == null && onSuccessVideo == null && onSuccessTracking == null)
            {
                Debug.LogError($"WebRequests.Get() must assign at least one of the optional success delegates");
            }

            switch (requestType)
            {
                case WebRequestType.Text:
                    using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
                    {
                        await ProcessRequest(requestType, unityWebRequest);

                        if (unityWebRequest.result == UnityWebRequest.Result.ProtocolError ||
                            unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
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
                        if (unityWebRequest.result == UnityWebRequest.Result.ProtocolError ||
                            unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
                        {
                            onError(unityWebRequest.error);
                        }
                        else
                        {
                            if (unityWebRequest.downloadHandler is DownloadHandlerTexture downloadHandlerTexture)
                            {
                                onSuccessTexture?.Invoke(downloadHandlerTexture.texture);
                            }
                                
                        }
                        break;
                    }
                }
                case WebRequestType.Video:
                {
                    using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url))
                    { 
                        await ProcessRequest(requestType, unityWebRequest);
                        if (unityWebRequest.result == UnityWebRequest.Result.ProtocolError ||
                            unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
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
                case WebRequestType.Tracking:
                {
                    using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
                    { 
                        await ProcessRequest(requestType, unityWebRequest);
                        if (unityWebRequest.result == UnityWebRequest.Result.ProtocolError ||
                            unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
                        {
                            onError(unityWebRequest.error);
                        }
                        else
                        {
                            onSuccessTracking?.Invoke();
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