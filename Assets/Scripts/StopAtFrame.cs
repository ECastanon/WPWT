using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAtFrame : MonoBehaviour
{
    public Animator anim;

    public void StopFrame()
    {
        anim.speed = 0;
    }
}
