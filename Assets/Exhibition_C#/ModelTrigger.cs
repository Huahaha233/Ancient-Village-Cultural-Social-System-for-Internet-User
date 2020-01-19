using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTrigger : MonoBehaviour {
    //用于触发展示模型提示的UI
    public GameObject tip;
    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.transform.childCount > 1)
        {
            tip.GetComponent<TweenPosition>().PlayForward();
            ControlPre.model = this.transform.GetChild(1).gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        tip.GetComponent<TweenPosition>().PlayReverse();
        ControlPre.model = null;
    }

}
