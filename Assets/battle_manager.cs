﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battle_manager : MonoBehaviour {
	public Text text;
	public Text logtext;
	public Image[] player;
	public Image[] enemy;
	public status[] PS = new status[4];
	public status[] ES =new status[4];
	public string TURN;
	public int allturn;
	public Sprite[] texture;
	public Image image;
	public float time;
	public int turnnonber;
	public int wazaN;
	public int playernonber;
	public int enemynonber;
	public int target;
	int enemytime=4;
	int dellogtext=3;
	public int rand;
	// Use this for initialization
	void Start () {
		playernonber = player.Length;
		enemynonber = enemy.Length;
		for(int i=0;i<player.Length;i++)
		{
			PS [i] = player[i].GetComponent<status> ();
			player [i].GetComponent<Outline> ().enabled = false;
		}

		for(int i=0;i<enemy.Length;i++)
		{
			ES [i] = enemy [i].GetComponent<status> ();
			enemy [i].GetComponent<Outline> ().enabled = false;
		}

		int Rand = (int)Random.Range (0, 2);
		if (Rand == 0) {
			TURN = "enemy";
			image.GetComponent<Image>().sprite= texture [0];

		} else {
			TURN = "player";
			image.GetComponent<Image>().sprite= texture [1];
		}
		image.enabled = true;
		time = 0;
		turnnonber = 0;
		allturn = 0;
		text.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		if(playernonber<=0)
			Application.LoadLevel ("gameover");

		if(enemynonber<=0)
			Application.LoadLevel ("map");
			

		switch(allturn)
		{
		case 0:
			if (time > 2) 
				image.enabled = false;
			
			if (image.enabled == false) {
				if (TURN == "enemy")
					allturn = 1;
				if (TURN == "player")
					allturn = 2;
			}
			break;
		case 1:
			if(time>dellogtext)
				logtext.GetComponent<Text> ().text = "";
			
			text.enabled = false;

			BE ();

			if (turnnonber > 3) {
				time = 0;
				turnnonber = 0;
				allturn = 3;
				TURN="player";
			}
			break;
		case 2:
			if(time>dellogtext)
				logtext.GetComponent<Text> ().text = "";
			text.enabled = true;

			for(int i=0;i<player.Length;i++)
			{
				if(i==turnnonber)
					player [i].GetComponent<Outline> ().enabled = true;
				else
					player [i].GetComponent<Outline> ().enabled = false;
			}

			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				if (!(wazaN >=3))
					wazaN++;
			}



			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				if (!(wazaN <= 0))
					wazaN--;
			}
			BP ();
			if (turnnonber > 3) {
				time = 0;
				allturn = 3;
				turnnonber = 0;
				TURN="enemy";
			}
			break;
		case 3:
			text.enabled = false;
			if (time > 2) {
				logtext.GetComponent<Text> ().text = TURN + "のターン";
				for(int i=0;i<player.Length;i++)
				{
					PS [i] = player[i].GetComponent<status> ();
					player [i].GetComponent<Outline> ().enabled = false;
				}

				for(int i=0;i<enemy.Length;i++)
				{
					ES [i] = enemy [i].GetComponent<status> ();
					enemy [i].GetComponent<Outline> ().enabled = false;
				}
			}
			

			if(time>3){
				if (TURN == "enemy")
					allturn = 1;
				if (TURN == "player")
					allturn = 2;
				logtext.GetComponent<Text> ().text = "";
				time = 0;
			}
			break;
		}
			
	}

	public void BP()
	{
		if (PS [turnnonber].STATE == "LIVE") {
			switch (wazaN) {
			case 0:
				text.GetComponent<Text> ().text = "\t〇1" + PS [turnnonber].waza [0] + "\n\t\t2" + PS [turnnonber].waza [1] + "\n\t\t3" + PS [turnnonber].waza [2] + "\n\t\t4" + PS [turnnonber].waza [3];
				PE2 ();
				break;
			case 1:
				text.GetComponent<Text> ().text = "\t\t1" + PS [turnnonber].waza [0] + "\n\t〇2" + PS [turnnonber].waza [1] + "\n\t\t3" + PS [turnnonber].waza [2] + "\n\t\t4" + PS [turnnonber].waza [3];
				PE2 ();
				break;
			case 2:
				text.GetComponent<Text> ().text = "\t\t1" + PS [turnnonber].waza [0] + "\n\t\t2" + PS [turnnonber].waza [1] + "\n\t〇3" + PS [turnnonber].waza [2] + "\n\t\t4" + PS [turnnonber].waza [3];
				PE2 ();
				break;
			case 3:
				text.GetComponent<Text> ().text = "\t\t1" + PS [turnnonber].waza [0] + "\n\t\t2" + PS [turnnonber].waza [1] + "\n\t\t3" + PS [turnnonber].waza [2] + "\n\t〇4" + PS [turnnonber].waza [3];
				PE2 ();
				break;

			}
		} else if (PS [turnnonber].STATE == "DEL") {
			turnnonber++;
		}
	}
		
	public void PE2()
	{
		if (Input.GetKeyDown (KeyCode.Return)) {
			time = 0;
			do{
				target = (int)Random.Range (0, 4);
			}
			while(ES[target].STATE!="LIVE");
			logtext.GetComponent<Text> ().text = ES [target].name + "に" + PS [turnnonber].name + "の" + PS [turnnonber].waza [wazaN] + "\n" + ES [target].name + "に" + PS [turnnonber].waza_power [wazaN] + "ダメージ";
			ES [target].HP -= PS [turnnonber].waza_power [wazaN];
			if (ES [target].HP <= 0) {
				ES [target].STATE = "DEL";
				enemynonber--;
			}
			turnnonber++;
		}
	}
	public void BE()
	{
		if (Input.GetKeyDown (KeyCode.Return)|| time>enemytime) {
			for(int i=0;i<enemy.Length;i++)
			{
				if(i==turnnonber)
					enemy [i].GetComponent<Outline> ().enabled = true;
				else
					enemy [i].GetComponent<Outline> ().enabled = false;
			}
		}
		if (ES [turnnonber].STATE == "LIVE") {
			switch (turnnonber) {
			case 0:
				BE2 ();
				break;
			case 1:
				BE2 ();
				break;
			case 2:
				BE2 ();
				break;
			case 3:
				BE2 ();
				break;

			}
		} else if (ES [turnnonber].STATE == "DEL") {
			turnnonber++;
		}
	}

	public void BE2()
	{
		if (Input.GetKeyDown (KeyCode.Return) || time > enemytime) {
			rand = (int)Random.Range (0, 4);
			do{
				target = (int)Random.Range (0, 4);
			}
			while(PS[target].STATE!="LIVE");
			logtext.GetComponent<Text> ().text = PS [target].name + "に" + ES [turnnonber].name + "の" + ES [turnnonber].waza [rand] + "\n" + PS [target].name + "に" + ES [turnnonber].waza_power [rand] + "ダメージ";
			PS [target].HP -= ES [turnnonber].waza_power [rand];
			if (PS [target].HP <= 0) {
				PS [target].HP = 0;
				PS [target].STATE = "DEL";
				playernonber--;
			}
			turnnonber++;
			time = 0;
		}
	}

}
