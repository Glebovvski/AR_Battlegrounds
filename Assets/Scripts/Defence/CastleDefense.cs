using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defendable
{
    public class CastleDefense : Defense
    {
        protected override bool IsReady { get { return true; } set { IsReady = true; } }

        public event Action OnLose;

        protected override void Death()
        {
            base.Death();
            OnLose?.Invoke();
        }
    }
}