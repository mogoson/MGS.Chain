/*************************************************************************
 *  Copyright © 2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  SinChain.cs
 *  Description  :  Define chain base on sin curve.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  8/26/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using Mogoson.Curve;
using UnityEngine;

namespace Mogoson.CurveChain
{
    /// <summary>
    /// Chain base on sin curve.
    /// </summary>
    [AddComponentMenu("Mogoson/CurveChain/SinChain")]
    public class SinChain : MonoCurveChain
    {
        #region Field and Property
        /// <summary>
        /// Args of sin curve.
        /// </summary>
        public SinArgs args = new SinArgs(1, 1, 0, 0);

        /// <summary>
        /// Max key of sin curve.
        /// </summary>
        public float maxKey = 2;

        /// <summary>
        /// Curve for chain.
        /// </summary>
        protected override ICurve Curve { get { return curve; } }

        /// <summary>
        /// Curve of chain.
        /// </summary>
        protected SinCurve curve = new SinCurve();
        #endregion

        #region Public Method
        /// <summary>
        /// Rebuild chain.
        /// </summary>
        public override void Rebuild()
        {
            curve.args = args;
            curve.MaxKey = maxKey * Mathf.PI;
            base.Rebuild();
        }
        #endregion
    }
}