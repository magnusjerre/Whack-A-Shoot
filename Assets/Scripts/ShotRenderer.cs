using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

[RequireComponent(typeof(LineRenderer))]
public class ShotRenderer : MonoBehaviour
{
    public float lifetime;
    public Vector3 start, end;

    private float elapsedTime;

    private LineRenderer lineRenderer;
    private Gradient gradient;
    private GradientAlphaKey[] initialAlphaKeys, targetAlphaKeys;
    private GradientColorKey[] initialColorKeys;

    private bool animate;
	public float maxWidthExpansion;

    public bool isAvailable;

    public float vrStartWidth = 0.06f, vrEndWidth = 0.02f;
    public float nvrStartWidth = 0.12f, nvrEndWidth = 0.04f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gradient = lineRenderer.colorGradient;

        initialAlphaKeys = new GradientAlphaKey[]{
            new GradientAlphaKey(0f, 0f),
            new GradientAlphaKey(1f, 1f)
        };
        targetAlphaKeys = new GradientAlphaKey[]{
            new GradientAlphaKey(0f, 0f),
            new GradientAlphaKey(0f, 1f)
        };
        initialColorKeys = gradient.colorKeys;
        Hide();
    }

    void Start()
    {
        if (VRSettings.enabled && VRSettings.isDeviceActive)
        {
            lineRenderer.startWidth = vrStartWidth;
            lineRenderer.endWidth = vrEndWidth;
        }
        else
        {
            lineRenderer.startWidth = nvrStartWidth;
            lineRenderer.endWidth = nvrEndWidth;
        }
    }

    void Update()
    {
        if (animate)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > lifetime)
            {
                Hide();
                isAvailable = true;
                return;
            }

            float amount = elapsedTime / lifetime;
            Vector3 newStart = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), 0.1f);
            Gradient newGradient = new Gradient();
            newGradient.SetKeys(new GradientColorKey[]{
                initialColorKeys[0], initialColorKeys[1]
            }, new GradientAlphaKey[]{
                new GradientAlphaKey(
                    initialAlphaKeys[0].alpha, Mathf.Min(amount, 0.8f)),
                new GradientAlphaKey(Mathf.Lerp(
                    initialAlphaKeys[1].alpha, targetAlphaKeys[1].alpha, amount
                ), 1f)
            });
            lineRenderer.colorGradient = newGradient;

			float newWidth = Mathf.Lerp(1, maxWidthExpansion, (elapsedTime / lifetime));
			lineRenderer.widthMultiplier = newWidth;
        }
    }

    public void Show(Vector3 start, Vector3 end)
    {
        if (animate)
        {
            return;
        }

        animate = true;
        lineRenderer.enabled = true;
        lineRenderer.SetPositions(new Vector3[] { start, end });
        isAvailable = false;
        
        Debug.Log("startwidth: " + lineRenderer.widthCurve.keys[0].value);//startwidth: 0.06078529
        Debug.Log("endWidth: " + lineRenderer.widthCurve.keys[1].value);//endWidth: 0.02206421
    }

    public void Hide()
    {
        animate = false;
        elapsedTime = 0;
        lineRenderer.colorGradient = gradient;
        lineRenderer.enabled = false;
		lineRenderer.widthMultiplier = 1;        
    }

}
