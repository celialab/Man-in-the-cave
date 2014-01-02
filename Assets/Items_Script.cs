﻿using UnityEngine;
using System.Collections;
using System;

public class Items_Script : MonoBehaviour
{
    float size, margin;
    public Texture blueligth, pinkligth, redligth, greenligth, ranbowligth, white;
    public GUISkin custom;
    int availableLights;
    bool draw;
    public AudioClip equipSound;
    public Texture back;
    float elementSize, positionYButtons;
    float UnTerzo;
    Vector2 position = Vector2.zero;
    // Use this for initialization
    void Start()
    {
        CameraScript.LoadData();
        size = Screen.width / 20;
        margin = Screen.width / 60;
        UnTerzo = Screen.height / 3;
        elementSize = UnTerzo;
        positionYButtons = UnTerzo;
        availableLights = 0;
        custom.label.fontSize = (int)(size);
        custom.button.fontSize = (int)(size / 2);
        custom.button.normal.textColor = Color.white;

        //test
        //CameraScript.LoadData();
        //CameraScript.data.lightRed = CameraScript.data.lightBlue = CameraScript.data.lightGreen = CameraScript.data.lightPink = CameraScript.data.lightRainbow = true;
    }

    void OnGUI()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            CameraScript.SaveData();
            Application.LoadLevel(0);
        }
        
        if (GUI.skin != custom)
            GUI.skin = custom;

        if (GUI.Button(new Rect(margin, margin/3, ((UnTerzo / 3) * 168) / 141, UnTerzo / 3), back))
        {
            CameraScript.SaveData();
            Application.LoadLevel(0);
        }
        Rect labelPosition = GUILayoutUtility.GetRect(new GUIContent("Items"), custom.label);
        GUI.Label(new Rect(margin*2 + ((UnTerzo / 3) * 168) / 141, UnTerzo / 6 - labelPosition.height / 2, labelPosition.width, labelPosition.height), "Items");
        //LUCE EQUIPAGGIATA
        //GUI.skin.label.fontSize = (int)(size);
        //GUI.Label(new Rect(margin, size * 4, size * 10, size * 2 + margin), "Light equipped");
        //equippedLight();

        //LUCI DISPONIBILI
       // availableLight();
        drawElements();
    }

    private void equippedLight()
    {
        if (CameraScript.data.helmet == Helmet.white)
        {
            GUI.DrawTexture(new Rect(size * 8, size * 4, size * 2, size * 2), white, ScaleMode.ScaleToFit, false);
            return;
        }
        if (CameraScript.data.helmet == Helmet.red)
        {
            GUI.DrawTexture(new Rect(size * 8, size * 4, size * 2, size * 2), redligth, ScaleMode.ScaleToFit, false);
            return;
        }
        if (CameraScript.data.helmet == Helmet.blue)
        {
            GUI.DrawTexture(new Rect(size * 8, size * 4, size * 2, size * 2), blueligth, ScaleMode.ScaleToFit, false);
            return;
        }
        if (CameraScript.data.helmet == Helmet.green)
        {
            GUI.DrawTexture(new Rect(size * 8, size * 4, size * 2, size * 2), greenligth, ScaleMode.ScaleToFit, false);
            return;
        }
        if (CameraScript.data.helmet == Helmet.pink)
        {
            GUI.DrawTexture(new Rect(size * 8, size * 4, size * 2, size * 2), pinkligth, ScaleMode.ScaleToFit, false);
            return;
        }
        if (CameraScript.data.helmet == Helmet.rainbow)
        {
            GUI.DrawTexture(new Rect(size * 8, size * 4, size * 2, size * 2), ranbowligth, ScaleMode.ScaleToFit, false);
            return;
        }
    }

    //float getSize(Helmet e)
    //{
    //    if(e==CameraScript.data.helmet)
    //        sele
    //}
    bool trovatoSetected = false;
    void drawElements()
{
    float selected = elementSize * 1.5f;
    float difference = (selected - elementSize);
    GUILayout.BeginHorizontal();       
     //GUILayout.Button("I'm the first button", GUILayout.Width(100),GUILayout.Height(100));
     if (GUI.Button(new Rect((Screen.width / 2 - elementSize / 2) + availableLights * (elementSize + margin), positionYButtons,
         CameraScript.data.helmet == Helmet.white ? selected : elementSize, CameraScript.data.helmet == Helmet.white ? selected : elementSize), white))
     {
         CameraScript.data.helmet = Helmet.white;
         audio.PlayOneShot(equipSound);
     }
    GUILayout.EndHorizontal();
    //scrollview
    int elem = 0;
    position = GUI.BeginScrollView(new Rect(0, UnTerzo / 3, Screen.width, Screen.height - (UnTerzo / 3)), position,
                                   new Rect(0, 0, (6 * elementSize) +(margin*5) +(Screen.width/2-elementSize/2)*2, Screen.height - (UnTerzo / 3)), true, false);
    availableLights = 0;
    trovatoSetected = false;
    if (CameraScript.data.helmet == Helmet.white)
    {
        if (GUI.Button(new Rect((Screen.width / 2 - selected / 2) + availableLights * (elementSize + margin)+difference,
            positionYButtons - selected/4, selected, selected), white))
        {
            CameraScript.data.helmet = Helmet.white;
            audio.PlayOneShot(equipSound);
        }
        trovatoSetected = true;
    }
    else
    {
        if (GUI.Button(new Rect((Screen.width / 2 - elementSize / 2) + availableLights * (elementSize + margin) + (trovatoSetected ? difference * 2 : 0),
            positionYButtons, elementSize, elementSize), white))
        {
            CameraScript.data.helmet = Helmet.white;
            audio.PlayOneShot(equipSound);
        }
    }
    availableLights++;

    if (CameraScript.data.helmet == Helmet.red)
    {
        if (GUI.Button(new Rect((Screen.width / 2 - selected / 2) + availableLights * (elementSize + margin) + difference, 
            positionYButtons - selected / 4, selected, selected), redligth))
        {
            CameraScript.data.helmet = Helmet.red;
            audio.PlayOneShot(equipSound);
        }
        trovatoSetected = true;
    }
    else
    {
        if (GUI.Button(new Rect((Screen.width / 2 - elementSize / 2) + margin + availableLights * (elementSize + margin) +(trovatoSetected?difference*2:0),
            positionYButtons, elementSize, elementSize), redligth))
        {
            CameraScript.data.helmet = Helmet.red;
            audio.PlayOneShot(equipSound);
        }
    }  
    availableLights++;

    if (CameraScript.data.helmet == Helmet.blue)
    {
        if (GUI.Button(new Rect((Screen.width / 2 - selected / 2) + availableLights * (elementSize + margin) + difference,
            positionYButtons - selected / 4, selected, selected), blueligth))
        {
            CameraScript.data.helmet = Helmet.blue;
            audio.PlayOneShot(equipSound);
        }
        trovatoSetected = true;
    }
    else
    {
        print("blue: " + trovatoSetected + "   " + difference);
        if (GUI.Button(new Rect((Screen.width / 2 - elementSize / 2) + margin + availableLights * (elementSize + margin) + (trovatoSetected ? difference*2 : 0),
            positionYButtons, elementSize, elementSize), blueligth))
        {
            CameraScript.data.helmet = Helmet.blue;
            audio.PlayOneShot(equipSound);
        }
    }
    availableLights++;
    
    if (CameraScript.data.helmet == Helmet.green)
    {
        if (GUI.Button(new Rect((Screen.width / 2 - selected / 2) + availableLights * (elementSize + margin) + difference,
            positionYButtons - selected / 4, selected, selected), greenligth))
        {
            CameraScript.data.helmet = Helmet.green;
            audio.PlayOneShot(equipSound);
        }
        trovatoSetected = true;
    }
    else
    {
        if (GUI.Button(new Rect((Screen.width / 2 - elementSize / 2) + margin + availableLights * (elementSize + margin) + (trovatoSetected ? difference*2 : 0),
            positionYButtons, elementSize, elementSize), greenligth))
        {
            CameraScript.data.helmet = Helmet.green;
            audio.PlayOneShot(equipSound);
        }
    }
    availableLights++;

    if (CameraScript.data.helmet == Helmet.pink)
    {
        if (GUI.Button(new Rect((Screen.width / 2 - selected / 2) + availableLights * (elementSize + margin) + difference,
            positionYButtons - selected / 4, selected, selected), pinkligth))
        {
            CameraScript.data.helmet = Helmet.pink;
            audio.PlayOneShot(equipSound);
        }
        trovatoSetected = true;
    }
    else
    {
        if (GUI.Button(new Rect((Screen.width / 2 - elementSize / 2) + margin + availableLights * (elementSize + margin) + (trovatoSetected ? difference*2 : 0),
            positionYButtons, elementSize, elementSize), pinkligth))
        {
            CameraScript.data.helmet = Helmet.pink;
            audio.PlayOneShot(equipSound);
        }
    }   
    availableLights++;

    if (CameraScript.data.helmet == Helmet.rainbow)
    {
        if (GUI.Button(new Rect((Screen.width / 2 - selected / 2) + availableLights * (elementSize + margin) + difference,
            positionYButtons - selected / 4, selected, selected), ranbowligth))
        {
            CameraScript.data.helmet = Helmet.rainbow;
            audio.PlayOneShot(equipSound);
        }
        trovatoSetected = true;
    }
    else
    {
        if (GUI.Button(new Rect((Screen.width / 2 - elementSize / 2) + margin + availableLights * (elementSize + margin) + (trovatoSetected ? difference*2 : 0),
            positionYButtons, elementSize, elementSize), ranbowligth))
        {
            CameraScript.data.helmet = Helmet.rainbow;
            audio.PlayOneShot(equipSound);
        }
    }   
    availableLights++;

    GUI.EndScrollView();
    }
    
    private void availableLight()
    {
        availableLights = 0;
        if (CameraScript.data.helmet != Helmet.white)
        {

           
        }
        if (CameraScript.data.lightRed && CameraScript.data.helmet != Helmet.red)
        {
           
        }
        if (CameraScript.data.lightBlue && CameraScript.data.helmet != Helmet.blue)
        {
           
        }
        if (CameraScript.data.lightGreen && CameraScript.data.helmet != Helmet.green)
        {
           
        }
        if (CameraScript.data.lightPink && CameraScript.data.helmet != Helmet.pink)
        {
            
        }
        if (CameraScript.data.lightRainbow && CameraScript.data.helmet != Helmet.rainbow)
        {
           
        }
        if (availableLights == 0)
        {          
            if (GUI.Button(new Rect(margin, size * 6 - margin, size * 11, size * 2 + margin), "Buy a new helmet's light!"))
            {
                CameraScript.SaveData();
                Application.LoadLevel(2);
            }
        }
    }
}
