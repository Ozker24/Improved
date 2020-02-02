using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisileInpact : MonoBehaviour
{
    public Misil misil;
    public float percentageScale;
    //public float actualScale;
    public float maxScale;
    public float initScale;

    public void Start()
    {
        transform.localScale = new Vector3(initScale, initScale, initScale);
    }

    public void Update()
    {
        if (misil.setPos)
        {
            percentageScale = maxScale * (misil.percentageToGround - 1) * (-1) + initScale;
            transform.localScale = new Vector3(percentageScale, percentageScale, percentageScale);
        }
    }
}
