using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class Lerper : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    [Range(0,1)] public float tValue = 0.5f;
    public TextMeshProUGUI textOutput;
    public LineRenderer lr;

    public enum LerpMode
    {
        LERP,
        SLERP
    }
    public LerpMode mode = LerpMode.LERP;


    // Update is called once per frame
    void Update()
    {
        if (textOutput)
            textOutput.text = "T = " + (Mathf.Round(tValue * 100) / 100.0f);

        if(pointA && pointB)
        {
            if(mode == LerpMode.LERP)
                transform.localPosition = Vector3.Lerp(pointA.localPosition, pointB.localPosition, tValue);
            if(mode == LerpMode.SLERP)
                transform.localPosition = Vector3.Slerp(pointA.localPosition, pointB.localPosition, tValue);

            if (lr)
                lr.SetPosition(1, transform.localPosition);
        }
    }
}
