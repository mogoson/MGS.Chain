/*************************************************************************
 *  Copyright © 2017-2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  DynamicRollerChain.cs
 *  Description  :  Define DynamicRollerChain component.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  6/27/2017
 *  Description  :  Initial development version.
 *************************************************************************/

using UnityEngine;

namespace Mogoson.CurveChain
{
    [AddComponentMenu("Mogoson/CurveChain/DynamicRollerChain")]
    public class DynamicRollerChain : RollerChain
    {
        #region Public Method
        /// <summary>
        /// Drive chain.
        /// </summary>
        /// <param name="velocity">Linear velocity.</param>
        public override void Drive(float velocity)
        {
            RebuildCurve(true);

            var maxTime = Curve[Curve.KeyframeCount - 1].key;
            if (Mathf.Abs(timer) >= maxTime)
                timer -= maxTime;

            base.Drive(velocity);
        }
        #endregion
    }
}