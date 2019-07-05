using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NGUI
{
    interface INGUIMove
    {
        void Start(GameObject ngui);//第一步执行时赋值，将需要移动的NGUI赋值给ngui
        void From(float x, float y);//移动的起始位置
        void To(float x, float y);//移动的结束位置
        void Speed(int s);//移动速度：Duration
    }

    public class NGUIMove : INGUIMove
    {
        private GameObject ui;//定义公共变量
        public void Start(GameObject ngui)
        {
            ui = ngui;
        }

        public void From(float x, float y)
        {
            ui.GetComponent<TweenPosition>().from = new Vector3(x, y, 0);
        }

        public void To(float x,float y)
        {
            ui.GetComponent<TweenPosition>().to = new Vector3(x, y, 0);
        }

        public void Speed(int s)
        {
            ui.GetComponent<TweenPosition>().duration = s;
        }
        
    }
}
