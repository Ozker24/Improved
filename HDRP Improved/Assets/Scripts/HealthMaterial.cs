using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMaterial : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] float actualLife;
    [SerializeField] float maxLife;
    [SerializeField] float percentageOfLife;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    public void Update()
    {
        percentageOfLife = Mathf.Clamp01(actualLife/maxLife);
    }
}
