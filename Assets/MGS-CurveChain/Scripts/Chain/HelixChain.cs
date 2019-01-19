/*************************************************************************
 *  Copyright © 2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  HelixChain.cs
 *  Description  :  Define chain base on helix curve.
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
    /// Chain base on helix curve.
    /// </summary>
    [AddComponentMenu("Mogoson/CurveChain/HelixChain")]
    public class HelixChain : MonoCurveChain
    {
        #region Field and Property
        /// <summary>
        /// Top ellipse args of curve.
        /// </summary>
        public EllipseArgs topEllipse = new EllipseArgs(Vector3.up, 1, 1);

        /// <summary>
        /// Bottom ellipse args of curve.
        /// </summary>
        public EllipseArgs bottomEllipse = new EllipseArgs(Vector3.zero, 1, 1);

        /// <summary>
        /// Max around radian of helix.
        /// </summary>
        public float maxRadian = 6;

        /// <summary>
        /// Curve for chain.
        /// </summary>
        protected override ICurve Curve { get { return curve; } }

        /// <summary>
        /// Curve of chain.
        /// </summary>
        protected HelixCurve curve = new HelixCurve();
        #endregion

        #region Public Method
        /// <summary>
        /// Rebuild chain.
        /// </summary>
        public override void Rebuild()
        {
            curve.topEllipse = topEllipse;
            curve.bottomEllipse = bottomEllipse;
            curve.MaxKey = maxRadian * Mathf.PI;
            base.Rebuild();
        }
        #endregion
    }
}