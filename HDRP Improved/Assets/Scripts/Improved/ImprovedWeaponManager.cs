using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedWeaponManager : MonoBehaviour
{
    public LaserGun laser;

    // Start is called before the first frame update
    public void Initialize()
    {
        laser = gameObject.GetComponentInChildren<LaserGun>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
