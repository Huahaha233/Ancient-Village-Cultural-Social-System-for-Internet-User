using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlChat : MonoBehaviour {
    public GameObject Chat;//聊天框
    private bool ischat = false;//弹出聊天框
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return) && (ControlVisiter.motion == ControlVisiter.Motion.None || ControlVisiter.motion == ControlVisiter.Motion.Chat))
        {
            if (ControlVisiter.motion == ControlVisiter.Motion.None)
            {
                ControlVisiter.motion = ControlVisiter.Motion.Chat;
                control(true);
                Chat.transform.GetChild(0).GetComponent<UITextList>().textLabel = Chat.transform.GetChild(4).GetComponent<UITextList>().textLabel;
            }
            else
            {
                ControlVisiter.motion = ControlVisiter.Motion.None;
                control(false);
            }
        }
	}
    private void control(bool b)
    {
        Chat.transform.GetChild(0).gameObject.SetActive(b);
        Chat.transform.GetChild(1).gameObject.SetActive(b);
        Chat.transform.GetChild(2).gameObject.SetActive(b);
        Chat.transform.GetChild(3).gameObject.SetActive(b);
    }
}
