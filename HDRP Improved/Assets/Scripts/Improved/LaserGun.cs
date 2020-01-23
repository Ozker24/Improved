using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    [SerializeField] bool isShoting;

    public void Shot()
    {
        isShoting = true;
    }

    public void ResetShot()
    {
        if (isShoting)
        {
            isShoting = false;
        }
    }
}
