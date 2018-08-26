/*************************************************************************
 *  Copyright © 2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  CircleChain.cs
 *  Description  :  Define chain base on circle curve.
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
    /// Chain base on circle curve.
    /// </summary>
    [AddComponentMenu("Mogoson/CurveChain/CircleChain")]
    public class CircleChain : MonoCurveChain
    {
        #region Field and Property
        /// <summary>
        /// Radius of circle curve.
        /// </summary>
        public float radius = 1.0f;

        /// <summary>
        /// Curve for chain.
        /// </summary>
        protected override ICurve Curve { get { return curve; } }

        /// <summary>
        /// Curve of chain.
        /// </summary>
        protected EllipseCurve curve = new EllipseCurve();
        #endregion

        #region Public Method
        /// <summary>
        /// Rebuild chain.
        /// </summary>
        public override void Rebuild()
        {
            curve.args.semiMinorAxis = radius;
            curve.args.semiMajorAxis = radius;
            base.Rebuild();
        }
        #endregion
    }
}