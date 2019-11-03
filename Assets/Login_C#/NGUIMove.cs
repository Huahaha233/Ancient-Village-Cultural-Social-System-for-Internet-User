using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NGUI
{
    interface INGUIMove
    {
        void Name(string name,int p);//传入需要移动的动画的tag，常量p表示UI的移动顺序，0表示正向播放，1表示反向播放
        void BackGround();
    }

    public class NGUIMove : INGUIMove
    {
        
        public void Name(string name,int p)
        {
            if (p == 0)
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag(name);
                foreach (GameObject gameObject in objects)
                {
                    gameObject.GetComponent<TweenPosition>().PlayForward(); // NGUI动画正向播放
                }
            }
            else
            {
                for (int i = 0; i < name.Length; i++)
                {
                    GameObject[] objects = GameObject.FindGameObjectsWithTag(name);
                    foreach (GameObject gameObject in objects)
                    {
                        gameObject.GetComponent<TweenPosition>().PlayReverse();//NGUI动画回放
                    }
                }
            }
        }

        public void BackGround()
        {
            GameObject bg1 = GameObject.Find("BackGround1").gameObject;
            GameObject bg2 = GameObject.Find("BackGround2").gameObject;
            bg1.GetComponent<TweenAlpha>().PlayForward();
            bg1.GetComponent<TweenScale>().PlayForward();
        }
    }
}
