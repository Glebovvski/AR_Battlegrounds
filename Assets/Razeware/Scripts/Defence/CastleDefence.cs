using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defendable
{
    public class CastleDefence : Defence
    {
        protected override bool IsReady { get { return true; } set { IsReady = true; } }

        
    }
}