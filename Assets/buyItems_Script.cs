﻿using UnityEngine;
using System.Collections;
using System;
using OnePF;
using System.Collections.Generic;

public class buyItems_Script : MonoBehaviour {

    public Texture arcoLight, bengal, bluLight, greenLight, piccone, pinkLight, reborn, redLight, coins;
    float size, margin;
    public GUISkin custom;
	public AudioClip cashsound;
    public static event EventHandler plus500, plus1000, plus5000;
    float[] span;
    bool goBack;
    bool imbuying;
    public Texture back;
	float unmarginino, scrollparam;

    #region Android e iOS inizializzazione
#if (UNITY_ANDROID || UNITY_IPHONE)
    private void OnEnable()
    {
        OpenIABEventManager.billingSupportedEvent += billingSupportedEvent;
        OpenIABEventManager.billingNotSupportedEvent += billingNotSupportedEvent;
        OpenIABEventManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
        OpenIABEventManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
        OpenIABEventManager.purchaseSucceededEvent += purchaseSucceededEvent;
        OpenIABEventManager.purchaseFailedEvent += purchaseFailedEvent;
        OpenIABEventManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
        OpenIABEventManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
    }

    #region failed (errors)
    private void consumePurchaseFailedEvent(string obj)
    {
        //sticazzi, muori utente
    }

    private void purchaseFailedEvent(string obj)
    {
        throw new NotImplementedException();
    }

    private void queryInventoryFailedEvent(string obj)
    {
        throw new NotImplementedException();
    }

    private void billingNotSupportedEvent(string obj)
    {
        throw new NotImplementedException();
    }

    #endregion

    private void consumePurchaseSucceededEvent(Purchase obj)
    {
        //IMPLEMENTARE
    }

    private void purchaseSucceededEvent(Purchase obj)
    {
        //IMPLEMENTARE
    }

    private void queryInventorySucceededEvent(Inventory obj)
    {
        //IMPLEMENTARE
    }

    private void billingSupportedEvent()
    {
        //IMPLEMENTARE
    }
#endif
    #endregion

    // Use this for initialization
    void Start()
    {
        #region Android inapp
        #if UNITY_ANDROID
            OpenIAB.init(new Dictionary<string, string> {
                {OpenIAB_Android.STORE_GOOGLE, "pt500"},
                {OpenIAB_Android.STORE_GOOGLE, "p1000"},
                {OpenIAB_Android.STORE_GOOGLE, "p5000"}
                });
        #endif
        #endregion

        #region iOS inapp
        #if UNITY_IPHONE
            OpenIAB.mapSku("SKU", OpenIAB_iOS.STORE, "some.ios.sku");   //scoprire cosa sono "some.ios.sku" forse mentre pubblichiamo lo scopriamo
            OpenIAB.mapSku("SKU", OpenIAB_iOS.STORE, "some.ios.sku");
            OpenIAB.mapSku("SKU", OpenIAB_iOS.STORE, "some.ios.sku");

            OpenIAB.init(new Dictionary<string, string> {
            {OpenIAB_iOS.STORE, "pt500"},
            {OpenIAB_iOS.STORE, "p1000"},
            {OpenIAB_iOS.STORE, "p5000"}
            });
        #endif
        #endregion
        imbuying = false;
        goBack = false;
        size = Screen.width / 20;
        margin = Screen.width / 60;
		if (Application.platform == RuntimePlatform.WP8Player || Application.platform == RuntimePlatform.MetroPlayerARM ||
            Application.platform == RuntimePlatform.MetroPlayerX64 || Application.platform == RuntimePlatform.MetroPlayerX86)
        {
						span = new float[] {
								0,
								0,
								size * 2 + margin ,
								margin * 2 + size * 4,
								margin * 3 + size * 6,
								margin * 4 + size * 8
						};
						unmarginino = margin * 11;
				} else {
						span = new float[] {
								0,
								0,
								size * 2 + 2 * margin ,
								margin * 4 + size * 4,
								margin * 6 + size * 6,
								margin * 8 + size * 8
						};
			unmarginino = margin * 12;
		}
        scrollparam = (Screen.height * 2) / 768;
        
        #if UNITY_METRO
        if (Input.touchCount == 0)
            imbuying = true;
        #endif

	}
    Vector2 position = Vector2.zero;

    void OnGUI()
    {
        if (Input.GetKey(KeyCode.Escape))
            goBack = true;
        GUI.skin = custom;
        custom.label.normal.textColor = new Color(255, 255, 255);
        custom.label.fontSize = (int)(size * 1.5);
        if (GUI.Button(new Rect(margin * 2, size, size * 1f, size * 1f), back))
            Application.LoadLevel(0);
        GUI.Label(new Rect(margin + size*1.5f, margin, size * 10, size * 2 + margin), "Buy Items"); // titolo
        custom.label.fontSize = (int)(size * 0.5f);
        //reborn e picconi
        GUI.Label(new Rect(margin*3, margin * 11, size * 6, size), "n Bengal: " + CameraScript.data.numBengala);
        GUI.Label(new Rect(size * 5 + margin*2, margin * 11, size * 6, size), "n Reborn: " + CameraScript.data.NumberReborn);
        //coins
        GUI.DrawTexture(new Rect(size * 10 , margin*2, size, size), coins);
		custom.label.fontSize = (int)(size*0.7);
        GUI.Label(new Rect(size * 11+ margin, margin * 2, size*2, size), "" + CameraScript.data.points);
		custom.label.fontSize = (int)(size * 0.5f);
        //purchases
		custom.button.normal.textColor = new Color(205, 127, 50);
		custom.button.fontSize = (int)(size * 0.7f);
        if (GUI.Button(new Rect(size * 13, margin * 2, size + 2*margin, size), "+500"))
            if (plus500 != null)
                plus500(this, new EventArgs());
		custom.button.normal.textColor = new Color(192, 192, 192);
		custom.button.fontSize = (int)(size * 0.8f);
        if (GUI.Button(new Rect(size * 15-margin, margin *2, size+margin*4, size), "+1000"))
            if (plus1000 != null)
                plus1000(this, new EventArgs());
		custom.button.normal.textColor = new Color(246, 193, 0);
		custom.button.fontSize = (int)(size * 0.9f) ;
        if (GUI.Button(new Rect(size * 17, margin * 1.8f, size + 5*margin, size), "+5000"))
            if (plus5000 != null)
                plus5000(this, new EventArgs());
        //colonna di sinistra
        if (GUI.Button(new Rect(margin, margin * 13, size * 9, size * 3), bengal))
            if (CameraScript.data.points >= 90)
            {
                CameraScript.data.points -= 90;
                CameraScript.data.numBengala += 2;
                CameraScript.SaveData();
                audio.PlayOneShot(cashsound);
            }
        if (GUI.Button(new Rect(margin, unmarginino + size * 3, size * 9, size * 3), reborn))
            if (CameraScript.data.points >= 300)
            {
                CameraScript.data.points -= 300;
                CameraScript.data.NumberReborn++;
                CameraScript.SaveData();
                audio.PlayOneShot(cashsound);
            }
        //scrollview
        int elem = 0;
		position = GUI.BeginScrollView(new Rect(size * 10, margin * 7, size * 10, Screen.height-(margin * 7)), position, 
		                               new Rect(0, 0, size * 10, size * 14));
        if (!CameraScript.data.lightRed)
        {
            elem++;
            if (GUI.Button(new Rect(0, span[elem], size * 9, size * 3), redLight))
            {
                if (CameraScript.data.points >= 500 && imbuying)
                {
                    CameraScript.data.points -= 500;
                    CameraScript.data.lightRed = true;
                    CameraScript.data.helmet = Helmet.red;
                    CameraScript.SaveData();
                    elem++;
                    //SUONA CASSA
                    audio.PlayOneShot(cashsound);
                }
            }
        }
        if (!CameraScript.data.lightBlue)
        {
            elem++;
            if (GUI.Button(new Rect(0, span[elem], size * 9, size * 3), bluLight))
                if (CameraScript.data.points >= 550 && imbuying)
                {
                    CameraScript.data.points -= 550;
                    CameraScript.data.lightBlue = true;
                    CameraScript.data.helmet = Helmet.blue;
                    CameraScript.SaveData();
                    //SUONA CASSA
                    audio.PlayOneShot(cashsound);
                }
        }
        if (!CameraScript.data.lightGreen)
        {
            elem++;
            if (GUI.Button(new Rect(0, span[elem], size * 9, size * 3), greenLight))
                if (CameraScript.data.points >= 750 && imbuying)
                {
                    CameraScript.data.points -= 750;
                    CameraScript.data.lightGreen = true;
                    CameraScript.data.helmet = Helmet.green;
                    CameraScript.SaveData();
                    //SUONA CASSA
                    audio.PlayOneShot(cashsound);
                }
        }
        if (!CameraScript.data.lightPink)
        {
            elem++;
            if (GUI.Button(new Rect(0, span[elem], size * 9, size * 3), pinkLight))
                if (CameraScript.data.points >= 800 && imbuying)
                {
                    CameraScript.data.points -= 800;
                    CameraScript.data.lightPink = true;
                    CameraScript.data.helmet = Helmet.pink;
                    CameraScript.SaveData();
                    //SUONA CASSA
                    audio.PlayOneShot(cashsound);
                }
        }
        if (!CameraScript.data.lightRainbow)
        {
            elem++;
            if (GUI.Button(new Rect(0, span[elem], size * 9, size * 3), arcoLight))
                if (CameraScript.data.points >= 6000 && imbuying)
                {
                    CameraScript.data.points -= 6000;
                    CameraScript.data.lightRainbow = true;
                    CameraScript.data.helmet = Helmet.rainbow;
                    CameraScript.SaveData();
                    //SUONA CASSA
                    audio.PlayOneShot(cashsound);
                }
        }
        GUI.EndScrollView(); 
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            bool fInsideList = IsTouchInsideList(touch.position);
            if (touch.phase == TouchPhase.Began)
                imbuying = true;
            if (touch.phase == TouchPhase.Moved && fInsideList)
            {
                position.y += touch.deltaPosition.y * scrollparam; //2:768= x:Screen.height
                imbuying = false;
            }
        }
        if (goBack)
            Application.LoadLevel(0);
    }

    bool IsTouchInsideList(Vector2 touchPos)
    {
        Vector2 screenPos = new Vector2(touchPos.x, touchPos.y);
        Rect rAdjustedBounds = new Rect(size * 10, margin * 7, size * 10, size * 16);

        return rAdjustedBounds.Contains(screenPos);
    }

}
