using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CrossPromo
{
    [Serializable]
    public struct VideoAdResponse
    {
        public int id;
        public string video_url;
        public string click_url;
        public string tracking_url;
    }
}

