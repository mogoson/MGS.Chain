/*************************************************************************
 *  Copyright © 2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  EllipseChain.cs
 *  Description  :  Define chain base on ellipse curve.
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
    /// Chain base on ellipse curve.
    /// </summary>
    [AddComponentMenu("Mogoson/CurveChain/EllipseChain")]
    public class EllipseChain : MonoCurveChain
    {
        #region Field and Property
        /// <summary>
        /// Semi minor axis of ellipse.
        /// </summary>
        public float semiMinorAxis = 1.0f;

        /// <summary>
        /// Semi major axis of ellipse.
        /// </summary>
        public float semiMajorAxis = 1.5f;

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
            curve.args.semiMinorAxis = semiMinorAxis;
            curve.args.semiMajorAxis = semiMajorAxis;
            base.Rebuild();
        }
        #endregion
    }
}