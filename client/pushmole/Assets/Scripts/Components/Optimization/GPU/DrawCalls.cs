using UnityEngine;
using System.Collections;

    public class DrawCalls : GPU
    {
        //---------------------------------------------------------
        //---------------------关于优化--------------------------
        //---------------优化中的几个典型的数字--------------
        //---------------------------------------------------------
        //---------------------------------------------------------
        /// <summary>
        /// <1	移动平台，DrawCalls少于200， 模型面数通常少于5W，顶点数少于10W。>
        /// <2	PC平台，Draw Calls为1000左右，面数少于50W，顶点数小于100W。>
        /// </summary>

        public override void Limit()
        {
            base.Limit();
        }

        public override void Definition()
        {
            base.Definition();
        }
    }
