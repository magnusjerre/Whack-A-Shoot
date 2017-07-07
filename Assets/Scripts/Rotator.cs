using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotationAxis;
    public float rotationAmount;
    public float rotationTime;

    [SerializeField]
    private bool isRotating, rotateToTarget;
    private float elapsedTime;
    private Quaternion originRotation;
    private Quaternion targetRotation;
    public Quaternion TargetRotation { get { return targetRotation; } }

    void Awake()
    {
        SetOriginRotation();
    }

    void Update()
    {
        if (isRotating)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= rotationTime)
            {
                isRotating = false;
                elapsedTime = 0;
                transform.rotation = rotateToTarget ? targetRotation : originRotation;
                rotateToTarget = !rotateToTarget;
                return;
            }

            if (rotateToTarget)
            {
                transform.rotation = Quaternion.Lerp(originRotation, targetRotation, elapsedTime / rotationTime);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(targetRotation, originRotation, elapsedTime / rotationTime);
            }
        }
    }

    public void SetOriginRotation()
    {
        originRotation = transform.rotation;
        targetRotation = Quaternion.Euler(originRotation.eulerAngles + rotationAxis * rotationAmount);
    }

    public void RotateToTarget(float animationTime)
    {
        if (isRotating)
        {
            return;
        }

        this.rotationTime = animationTime;
        this.rotateToTarget = true;
        isRotating = true;
    }

    public void RotateToOrigin(float animationTime)
    {
        if (isRotating)
        {
            return;
        }

        this.rotationTime = animationTime;
        this.rotateToTarget = false;
        isRotating = true;
    }

    public void RotateToTarget()
    {
        RotateToTarget(rotationTime);
    }

    public void RotateToOrigin()
    {
        RotateToOrigin(rotationTime);
    }
}
