using UnityEngine;
using CrossPromo.Scripts;

namespace CrossPromo
{
    public class CrossPromoDisplayer : MonoBehaviour
    {
        [SerializeField]
        private string playerId;
        
        [SerializeField]
        private CrossPromoController controller;


        public void SetPlayerId(string id)
        {
            controller.SetPlayerId(id);
        }
        
        public void Next()
        {
            if (controller != null)
            {
                controller.Next();
            }
        }

        public void Previous()
        {
            if (controller != null)
            {
                controller.Previous();
            }
        }

        public void Pause()
        {
            if (controller != null)
            {
                controller.Pause();
            }
        }

        public void Resume()
        {
            if (controller != null)
            {
                controller.Resume();
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            if (controller != null)
            {
                controller.DownloadAndPlayVideos(playerId);
            }
            else
            {
                Debug.LogError("CrossPromoDisplayer controller unassigned reference");
            }
        }

    }
}