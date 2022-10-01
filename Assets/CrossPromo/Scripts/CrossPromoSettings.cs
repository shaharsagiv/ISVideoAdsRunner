using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossPromo
{
    public class CrossPromoSettings
    {
        public string PlayerId { get; }
        public Vector2 VideoDimentions { get; }
        public bool ShowDownloadIndicator { get; }

        public CrossPromoSettings(Vector2 videoDimentions, bool showDownloadIndicator,string playerId)
        {
            VideoDimentions = videoDimentions;
            ShowDownloadIndicator = showDownloadIndicator;
            PlayerId = playerId;
        }
    }
}