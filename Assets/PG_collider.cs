﻿using UnityEngine;
using System.Collections;

public class PG_collider : MonoBehaviour {
    
    public Transform player;

	// Use this for initialization
	void Start () {
	
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "rock")
        {
            Destroy(col.gameObject);
            Destroy(player.gameObject);
        }
        if (col.gameObject.tag == "coin")
        {
            //aggiungi soldi
        }
    }
}
