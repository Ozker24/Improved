using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator anim;
    public PlayerController player;
    public HitArea hit;

    [Header("Raycast")]
    public Vector3 origin;
    public Vector3 direction;
    public float distance;
    public LayerMask mask;

    [Header("Climb")]
    public int dir;
    public Rigidbody rigid;

    public void Initialize()
    {
        player = GetComponentInParent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    public void MyFixedUpdate()
    {
        origin = new Vector3 (transform.position.x, 1 ,transform.position.z);

        direction = transform.TransformDirection(0, 0, 1);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(origin, direction, out hit, distance, mask))
        {
            //Debug.Log(hit.normal);
            if (hit.normal.z <= -1)
            {
                dir = -1;
            }
            else if (hit.normal.z >= 1)
            {
                dir = 1;
            }
        }
    }

    public void ForwardAnimation()
    {
        //anim.SetFloat("WalkForward", player.axis.y);
    }

    public void ClimbAnimation()
    {
        rigid.isKinematic = true;
        player.climb = true;
        player.GM.ableToInput = false;
        player.axis = new Vector2(0, 0);

        if (dir == -1)
        {
            Debug.Log("Climb up");
            anim.SetTrigger("Climb");
        }
        if (dir == 1)
        {
            Debug.Log("Climb back");
            anim.SetTrigger("ClimbBack");
        }
    }

    public void AfterClimb()
    {
        Debug.Log("after");
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
        rigid.isKinematic = false;
        player.climb = false;
        player.GM.ableToInput = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, direction * distance);
    }

    public void CrouchAnimation(bool crouch)
    {
        //anim.SetBool("Crouch", crouch);
    }

    public void RunAnimation(bool run)
    {
        //anim.SetBool("Run", run);
    }

    public void WalkAnimation(bool walk)
    {
        //anim.SetBool("Walk", walk);
    }

    public void HitAnimations()
    {
        /*anim.SetInteger("Hit", player.CC.ActualHit);
        player.anims.anim.SetBool("Hitting", player.CC.triggerAnim);*/
    }

    public void StopHit()
    {
        Debug.Log(player.CC.triggerAnim);
        player.CC.triggerAnim = false;
        player.CC.canHit = true;
        HitAnimations();

        player.GM.ableToInput = true;
    }

    public void ActiveHit()
    {
        hit.AppearHit();
    }
}
