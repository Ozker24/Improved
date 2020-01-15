using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float timeMagnitude;

    public float returnTime;

    public float injectedTime;

    public bool resetTime;

    public void MyUpdate()
    {
        if (resetTime)
        {
            Time.timeScale += (1 / returnTime) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
        }
    }

    public void ZaWarudo()
    {
        resetTime = false;
        Time.timeScale = timeMagnitude;
        //Time.fixedDeltaTime = timeMagnitude * 0.02f;
        StartCoroutine(ReturnTime());
    }

    IEnumerator ReturnTime()
    {
        yield return new WaitForSeconds(injectedTime);
        resetTime = true;
    }
}
