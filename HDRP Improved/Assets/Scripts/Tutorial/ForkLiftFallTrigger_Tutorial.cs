using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkLiftFallTrigger_Tutorial : MonoBehaviour
{
    public ForkLift_Tutorial forkLift;
    public CheckPoint checkPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            //checkPoint.SetNewSpawnPoint();
            forkLift.AudioFinishSet();
            Destroy(gameObject);
        }
    }
}
