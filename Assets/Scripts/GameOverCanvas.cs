using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCanvas : MonoBehaviour {

    public RankRow rowPrefab;
    public GameObject rowContainer;
	private Canvas canvas;

	void Start () {
		canvas = rowContainer.GetComponentInParent<Canvas> ();
		canvas.enabled = false;
	}

	public void Show() {
		canvas.enabled = true;
	}

	public void Hide() {
		canvas.enabled = false;
	}

    public void AddScores(List<PlayerScore> scores) {
		scores.Sort();
		RemoveExistingScores ();
		var rowHeader = Instantiate (rowPrefab, rowContainer.transform);
		var headerTransform = rowHeader.GetComponent<RectTransform> ();
		rowHeader.Set ("Rank", "Player name", "Score");
		rowHeader.SetFontStyle (FontStyle.Bold);
		for (var i = 0; i < scores.Count; i++) {
			var row = Instantiate (rowPrefab, rowContainer.transform);
			var rectTransform = row.GetComponent<RectTransform> ();
			rectTransform.anchoredPosition = new Vector2 (0, -(i + 1) * rectTransform.rect.height);
			var playerScore = scores [i];
			row.Set (i + 1, playerScore.PlayerId + "", playerScore.TotalScore);
		}
    }

	private void RemoveExistingScores() {
		int index = 0;
		while (rowContainer.transform.childCount > 0) {
			var row = rowContainer.transform.GetChild (index).GetComponent<RankRow> ();
			if (row == null) {
				index++;
			} else {
				row.gameObject.SetActive (false);
				Destroy (row);
			}
		}
	}

}
