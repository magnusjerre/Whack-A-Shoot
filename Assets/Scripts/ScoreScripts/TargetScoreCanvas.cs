using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TargetScoreCanvas : MonoBehaviour
{

    private Text text;

    public float lifetime;

    private bool showScore;
    private float elapsedTime;

    private Vector3 initialScale, calculationScale;
    public float maxScaleFactor = 1.5f;
    public float vrTextScaleFactor = 0.5f;
    public float upwardsSpeed = 0.5f;

    public Color textColor;

    public bool IsAvailable { get; set; }

    void Awake() {
        text = GetComponentInChildren<Text>();
    }

    void Start() {
        initialScale = transform.localScale;
        calculationScale = initialScale;
        transform.LookAt(transform.position - Vector3.up, Vector3.forward);
        text.color = textColor;
    }

    void Update() {
        if (showScore)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > lifetime)
            {
                Hide();
                return;
            }

            float tempScale = Mathf.Lerp(calculationScale.x, calculationScale.x * maxScaleFactor, elapsedTime / lifetime);
            transform.localScale = new Vector3(tempScale, tempScale, tempScale);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f - elapsedTime / lifetime);
            transform.localPosition += Vector3.up * Time.deltaTime * upwardsSpeed;
        }
    }

    public void ShowScore(NetworkInstanceId targetId, int score, int maxScore, bool isVrPlayer)
    {
        var target = GetTarget(targetId);

        transform.position = target.transform.position;  //Canvas position in world space
        if (isVrPlayer) {
            transform.LookAt(transform.position + (transform.position - Vector3.zero) * 5, Vector3.up);
            calculationScale = initialScale * vrTextScaleFactor;
        } else {
            transform.LookAt(transform.position - Vector3.up, Vector3.forward);
        }
        transform.localScale = calculationScale;

        text.text = score + " / " + maxScore;
        text.color = textColor;
        showScore = true;
        text.enabled = true;
        IsAvailable = false;
        elapsedTime = 0f;
    }

    public void Hide()
    {
        showScore = false;
        text.enabled = false;
        IsAvailable = true;
    }

    private Target GetTarget(NetworkInstanceId targetId)
    {
        Target[] targets = GameObject.FindObjectsOfType<Target>();
        foreach (var target in targets)
        {
            if (target.GetNetId().Equals(targetId))
            {
                return target;
            }
        }
        return null;
    }

}
