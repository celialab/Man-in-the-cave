﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;

public class MenuScript : MonoBehaviour
{
    public GUISkin custom;
    float height;
    float margin;
    static bool play = true;
    public Texture PlayButton, ScoreButton, ItemsButton, BuyItemsButton, SettingsButton;
    public Texture Title, facebook, twitter, review, celialab;
    public Texture myAppFreeBanner, normalBanner;
    public static event EventHandler GOReviews;
    public AudioClip promotionSound;


    private Quaternion cameraBase = Quaternion.identity;
    private Quaternion calibration = Quaternion.identity;
    private Quaternion baseOrientation = Quaternion.Euler(90, 0, 0);
    private Quaternion baseOrientationRotationFix = Quaternion.identity;
    private Quaternion referanceRotation = Quaternion.identity;
    private readonly Quaternion baseIdentity = Quaternion.Euler(90, 0, 0);
    private readonly Quaternion landscapeRight = Quaternion.Euler(0, 0, 90);
    private readonly Quaternion landscapeLeft = Quaternion.Euler(0, 0, -90);
    private readonly Quaternion upsideDown = Quaternion.Euler(0, 0, 180);
    Vector3 accel;
    float filter = 5.0f;
    void Start()
    {
        PlayScript.State = PlayScript.PlayState.menu;
        height = Screen.width / 20;
        margin = Screen.width / 60;
        ////Data.LoadFromText();

        Debug.Log(PlayerPrefs.GetString("data"));
        CameraScript.LoadData();

        if (PlayerPrefs.GetString("gift1") == "")
        {
            if (Application.platform == RuntimePlatform.WP8Player)
              ;//  StartWebRequest("http://celialab.com/Promotion.txt");
            else
                addPoints(1000);
        }
        accel = Input.acceleration;
    }
    float StartPromotionSound;
    bool StartPromotion = false;
    float StayPromotionBannar;
    void Update()
    {
        if (play)
        {
            play = false;
            audio.Play();
        }
        if (StartPromotion && Time.time >= StartPromotionSound)
        {
            audio.PlayOneShot(promotionSound);
            StayPromotionBannar = Time.time + 5;
            StartPromotion = false;
        }

        if (SystemInfo.supportsGyroscope)
        {
            if (start)
            {
                Input.gyro.enabled = true;
                AttachGyro();
                start = false;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation,
            cameraBase * (ConvertRotation(referanceRotation * Input.gyro.attitude) * GetRotFix()), 0.2f);
        }
        else if (SystemInfo.supportsAccelerometer)
        {
            // filter the jerky acceleration in the variable accel:
            accel = Vector3.Lerp(accel, Input.acceleration, filter * Time.deltaTime);
            float x = -((accel.y * 100)); //si muove in alto e basso
            //if(x<-5)
            //    x=-5;
            //else if(x>+33)
            //    x=33;

            float DestraSinistra = -90 * accel.x;//si muove a destra e sinistra
            //if (y < -55)
            //    y = -55;
            //else if (x > +55)
            //    y = 55;

            float Altobasso = (accel.y * 90) + 90;
            if (accel.z >= 0)
                Altobasso *= -1;
            //print(Altobasso);
            transform.rotation = Quaternion.Euler(Altobasso, DestraSinistra, 0f);
        }
    }
    bool start = true;
    private void AttachGyro()
    {
        ResetBaseOrientation();
        UpdateCalibration(true);
        UpdateCameraBaseRotation(true);
        RecalculateReferenceRotation();
    }
    private void UpdateCameraBaseRotation(bool onlyHorizontal)
    {
        if (onlyHorizontal)
        {
            var fw = transform.forward;
            fw.y = 0;
            if (fw == Vector3.zero)
            {
                cameraBase = Quaternion.identity;
            }
            else
            {
                cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
            }
        }
        else
        {
            cameraBase = transform.rotation;
        }
    }
    private void UpdateCalibration(bool onlyHorizontal)
    {
        if (onlyHorizontal)
        {
            var fw = (Input.gyro.attitude) * (-Vector3.forward);
            fw.z = 0;
            if (fw == Vector3.zero)
            {
                calibration = Quaternion.identity;
            }
            else
            {
                calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
            }
        }
        else
        {
            calibration = Input.gyro.attitude;
        }
    }
    private static Quaternion ConvertRotation(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
    private Quaternion GetRotFix()
    {
#if UNITY_3_5
		if (Screen.orientation == ScreenOrientation.Portrait)
			return Quaternion.identity;
		
		if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.Landscape)
			return landscapeLeft;
				
		if (Screen.orientation == ScreenOrientation.LandscapeRight)
			return landscapeRight;
				
		if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
			return upsideDown;
		return Quaternion.identity;
#else
        return Quaternion.identity;
#endif
    }
    private void ResetBaseOrientation()
    {
        baseOrientationRotationFix = GetRotFix();
        baseOrientation = baseOrientationRotationFix * baseIdentity;
    }

    /// <summary>
    /// Recalculates reference rotation.
    /// </summary>
    private void RecalculateReferenceRotation()
    {
        referanceRotation = Quaternion.Inverse(baseOrientation) * Quaternion.Inverse(calibration);
    }
    private void addPoints(int p)
    {
        CameraScript.data.points += p;
        CameraScript.SaveData();
        PlayerPrefs.SetString("gift1", "done");//  salvataggio su unity
        PlayerPrefs.Save();
        StartPromotionSound = Time.time + 2.113f;
        StartPromotion = true;
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (GUI.skin != custom)
            GUI.skin = custom;
        float piccoliBottoniSize = Screen.width / 4.6f;
        float UnTerzo = Screen.height / 3;
        float playSize = UnTerzo - margin;
        float BottoniHeight = UnTerzo * 0.7f - margin;
        float SocialSize = UnTerzo * 0.5f - margin;
        //print("width: "+((BottoniHeight * 4 + margin * 3)));
        //print("h : " + BottoniHeight);
        GUI.DrawTexture(new Rect((Screen.width / 2) - (((BottoniHeight * 4 + margin * 3)) / 2),
                                UnTerzo / 2 - (BottoniHeight/2), 
                                ((BottoniHeight * 4 + margin * 3)),
                                BottoniHeight),
                                Title, ScaleMode.ScaleToFit, true);
        if (GUI.Button(new Rect((Screen.width / 2) - playSize / 2, UnTerzo + ((UnTerzo / 2) - playSize / 2), playSize, playSize), PlayButton))
        {
            Application.LoadLevel("main");
        }

        float posizioneButton;
        int numBottone = 0;
        if (true/*CameraScript.data.Records[0].x != 0*/)
        {
            posizioneButton = Screen.width / 2 - ((BottoniHeight * 4 + margin * 3)) / 2;
            if (GUI.Button(new Rect(posizioneButton,
                                     UnTerzo * 2 + ((UnTerzo / 2) - BottoniHeight / 2),
                                     BottoniHeight, BottoniHeight), ScoreButton))
            {
                Application.LoadLevel("Scores");
            }
            numBottone++;
        }
        else
            posizioneButton = Screen.width / 2 - ((BottoniHeight * 3 + margin * 3)) / 2;

        if (GUI.Button(new Rect(posizioneButton + (margin + BottoniHeight) * numBottone,
                               UnTerzo * 2 + ((UnTerzo / 2) - BottoniHeight / 2),
                                 BottoniHeight, BottoniHeight), BuyItemsButton))
        {
            Application.LoadLevel("BuyItems");
        }
        numBottone++;
        if (GUI.Button(new Rect(posizioneButton + (margin + BottoniHeight) * numBottone,
                                UnTerzo * 2 + ((UnTerzo / 2) - BottoniHeight / 2),
                                BottoniHeight, BottoniHeight), ItemsButton))
        {
            Application.LoadLevel("Items");
        }
        numBottone++;
        if (GUI.Button(new Rect(posizioneButton + (margin + BottoniHeight) * numBottone,
                               UnTerzo * 2 + ((UnTerzo / 2) - BottoniHeight / 2),
                                 BottoniHeight, BottoniHeight), SettingsButton))
        {
            Application.LoadLevel("Settings");
        }
        if (GUI.Button(new Rect(Screen.width - (SocialSize + margin), Screen.height - (SocialSize * 3 + margin), SocialSize, SocialSize), facebook))
        {
            Application.OpenURL("https://www.facebook.com/Celialab");//vai su facebook
        }
        if (GUI.Button(new Rect(Screen.width - (SocialSize + margin), Screen.height - (SocialSize * 2 + margin), SocialSize, SocialSize), twitter))
        {
            Application.OpenURL("https://twitter.com/celialabGames");//vai su twitter
        }
        if (GUI.Button(new Rect(Screen.width - (SocialSize + margin), Screen.height - (SocialSize + margin), SocialSize, SocialSize), review))
        {
            if (Application.platform == RuntimePlatform.Android)
                Application.OpenURL("market://details?id=com.celialab.ManInTheCave");
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                Application.OpenURL("");//vai su review  mistery
            else if (Application.platform == RuntimePlatform.WP8Player || Application.platform == RuntimePlatform.MetroPlayerARM ||
            Application.platform == RuntimePlatform.MetroPlayerX64 || Application.platform == RuntimePlatform.MetroPlayerX86)
            {
                if (GOReviews != null)
                    GOReviews(this, new EventArgs());
            }
        }

        var celialabHeight = ((Screen.width / 5) * 59) / 200;
        if (GUI.Button(new Rect(margin / 2.3f, Screen.height - (celialabHeight + margin / 2.3f), Screen.width / 5, celialabHeight), celialab))
        {
            Application.OpenURL("http://celialab.com/");
        }
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();

        if (Time.time < StayPromotionBannar)
        {
            if (Application.platform == RuntimePlatform.WP8Player)
            {
                if (GUI.Button(new Rect(0, 0, Screen.width, (Screen.width * 156) / 1280), myAppFreeBanner)) //1280: 156= width :x
                {
                    Application.OpenURL("https://www.myAppFree.com");//visualizza bottone myappfree con link al sito  
                }
            }
            else
                GUI.DrawTexture(new Rect(0, 0, Screen.width, (Screen.width * 250) / 2048), normalBanner, ScaleMode.ScaleToFit, true);
        }

    }

    private void StartWebRequest(string url)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);
        }
        catch { }
    }

    private void FinishWebRequest(IAsyncResult result)
    {
        try
        {
            HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
            // Debug.WriteLine(response.ContentType);
            if (response.StatusCode == HttpStatusCode.NotFound)
                print("non c'è");
            else
                addPoints(1000);//c'è
        }
        catch (Exception e)
        {
            print(e.Message);
        }

    }
}