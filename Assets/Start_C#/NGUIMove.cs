using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NGUI
{
    interface INGUIMove
    {
        void Name(string[] name,int p);//传入需要移动的动画的开头英文，常量p表示UI的移动顺序，0表示正向播放，1表示反向播放
    }

    public class NGUIMove : INGUIMove
    {
        
        public void Name(string[] name,int p)
        {
            if (p == 0)
            {
                for(int i = 0; i < name.Length; i++)
                {
                    PlayForward(name[i]);
                }
            }
            else
            {
                for (int i = 0; i < name.Length; i++)
                {
                    PlayReverse(name[i]);
                }
            }
        }

        

        // NGUI动画正向播放
        private void PlayForward(string str)
        {
            GameObject.Find(str).GetComponent<TweenPosition>().PlayForward();
        }
        //NGUI动画回放
        private void PlayReverse(string str)
        {
            GameObject.Find(str).GetComponent<TweenPosition>().PlayReverse();
        }

    }
}
