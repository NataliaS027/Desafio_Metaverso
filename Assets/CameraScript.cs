using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class CameraScript : MonoBehaviour
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
                        var result = barcodeReader.Decode(this.cam_texture.GetPixels32(), this.cam_texture.width, this.cam_texture.height); //Converte o QR
                        Debug.Log($"QR Code :{result.Text}");


                        var client = new RestClient(result.Text);
                        client.Timeout = -1;
                        var request = new RestRequest(Method.GET);
                        request.AddHeader("Accept", "text/plain");
                        request.AddHeader("Authorization", "<API Key>");
                        IRestResponse response = client.Execute(request);
                        Debug.Log(response.Content);

                        var listSprites = JsonConvert.DeserializeObject<List<SafeBoxResponseVm>>(response.Content);
                        List<Sprite> list = new List<Sprite>();

                        int cont = 1;
                        foreach (var safeBoxResponseVm in listSprites)
                        {
                            byte[] imageBytes = Convert.FromBase64String(safeBoxResponseVm.Image);
                            Texture2D tex = new Texture2D(2, 2);
                            tex.LoadImage(imageBytes);
                            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                            sprite.name = cont++.ToString();

                            list.Add(sprite);
                        }

                        Controller.Instance.imageObject.GetComponent<AnimationObject>().ListAnimation = list;
  
                        Controller.Instance.QRCode.SetActive(false);
                        Controller.Instance.Camera.SetActive(true);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
        }
    }

    public class SafeBoxResponseVm
    {
        public string Image { get; set; }
        public int Value { get; set; }
        public SafeBoxResponseVm(string image)
        {
            Image = image;
        }
    }
}

