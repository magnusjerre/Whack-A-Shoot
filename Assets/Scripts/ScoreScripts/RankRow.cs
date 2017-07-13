﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankRow : MonoBehaviour {
	
	private Text rank, playername, playerscore;

	public Color textColor = Color.black;
	public int fontSize = 14;
	public FontStyle fontStyle = FontStyle.Normal;

	void Awake() {
		var textChildren = transform.GetComponentsInChildren<Text> ();
		foreach (var textChild in textChildren) {
			if (textChild.name.Equals("Rank")) {
				rank = textChild;
			} else if (textChild.name.Equals("PlayerName")) {
				playername = textChild;
			} else if (textChild.name.Equals("PlayerScore")) {
				playerscore = textChild;
			}
		}
	}

	void Start () {
		SetColor(textColor);
		SetFontSize(fontSize);
		SetFontStyle(fontStyle);
	}

	public void Set(string rank, string playername, string playerscore) {
		this.rank.text = rank;
		this.playername.text = playername;
		this.playerscore.text = playerscore;
	}
	
	public void Set(int rank, string playername, int score) {
		this.rank.text = rank.ToString();
		this.playername.text = playername;
		this.playerscore.text = score.ToString();
	}

	public void SetColor(Color color) {
		rank.color = color;
		playername.color = color;
		playerscore.color = color;
	}

	public void SetFontSize(int fontSize) {
		rank.fontSize = fontSize;
		playername.fontSize = fontSize;
		playerscore.fontSize = fontSize;
	}

	public void SetFontStyle(FontStyle fontStyle) {
		rank.fontStyle = fontStyle;
		playername.fontStyle = fontStyle;
		playerscore.fontStyle = fontStyle;
	}
}