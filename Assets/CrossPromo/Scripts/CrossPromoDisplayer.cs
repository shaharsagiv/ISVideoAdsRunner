using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrossPromo;
using System;

public class CrossPromoDisplayer : MonoBehaviour
{
    public string PlayerId;
    public Vector2 VideoDimentions;
    public bool ShowDownloadIndicator;
    public CrossPromoController Controller;


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

    // Start is called before the first frame update
    void Start()
    {
        var settings = new CrossPromoSettings(VideoDimentions, ShowDownloadIndicator, PlayerId);
        if(Controller != null)
        {
            Controller.DownloadAndPlayVideos(settings);
        }
        
    }

}
