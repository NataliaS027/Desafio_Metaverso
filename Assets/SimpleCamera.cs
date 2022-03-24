using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class SimpleCamera : MonoBehaviour
{
    [Header("Video Source")] public RawImage raw_image_video;

    private WebCamTexture cam_texture;

    private bool is_reading = false;

    void OnEnable() => StartCoroutine(this.Start_webcam());

    private IEnumerator Start_webcam()
    {
        yield return new WaitForSeconds(0.11f);

        this.cam_texture = new WebCamTexture();

        this.cam_texture.requestedWidth = 540;
        this.cam_texture.requestedHeight = 720;

        this.cam_texture.Play();

        #region Plataforma
        if (Application.platform == RuntimePlatform.Android)
        {
            this.raw_image_video.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            this.raw_image_video.rectTransform.localScale = new Vector3(-1, 1, 1);
            this.raw_image_video.rectTransform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            this.raw_image_video.rectTransform.localScale = new Vector3(-1, 1, 1);
        }
        #endregion Plataforma

        this.raw_image_video.texture = cam_texture;

        this.is_reading = true;

        yield return null;
    }

    void OnDisable()
    {
        if (this.cam_texture != null)
        {
            this.cam_texture.Stop();
        }
    }

    private float interval_time = 0.1f;
    private float time_stamp = 0;

    void Update()
    {
        if (this.is_reading)
        {
            this.time_stamp += Time.deltaTime;
            if (this.time_stamp > this.interval_time)
            {
                this.time_stamp = 0;
                try
                {
                    IBarcodeReader barcodeReader = new BarcodeReader();

                    if (this.cam_texture != null && this.cam_texture.isPlaying && this.cam_texture.isReadable)
                    {
                
                    }
                }
                catch (Exception ex)   { }
            }
        }
    }
}

