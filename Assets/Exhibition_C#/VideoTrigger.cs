using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoTrigger : MonoBehaviour {
    //用于触发视频提示的UI
    public GameObject tip;
    public static bool istrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name=="Player")
        {
            tip.GetComponent<TweenPosition>().PlayForward();
            istrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            tip.GetComponent<TweenPosition>().PlayReverse();
            istrigger = false;
        }
    }

}
