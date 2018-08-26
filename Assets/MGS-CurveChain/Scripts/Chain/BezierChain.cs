/*************************************************************************
 *  Copyright © 2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  BezierChain.cs
 *  Description  :  Define chain base on cubic bezier curve.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  8/25/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using Mogoson.Curve;
using UnityEngine;

namespace Mogoson.CurveChain
{
    /// <summary>
    /// Chain base on cubic bezier curve.
    /// </summary>
    [AddComponentMenu("Mogoson/CurveChain/BezierChain")]
    public class BezierChain : MonoCurveChain
    {
        #region Field and Property
        /// <summary>
        /// Anchor points of chain curve.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        protected CubicBezierAnchor anchor = new CubicBezierAnchor(Vector3.one,
            new Vector3(3, 1, 3), new Vector3(1, 1, 2), new Vector3(3, 1, 2));

        /// <summary>
        /// Start point of chain curve.
        /// </summary>
        public Vector3 StartPoint
        {
            set { anchor.start = transform.InverseTransformPoint(value); }
            get { return transform.TransformPoint(anchor.start); }
        }

        /// <summary>
        /// End point of chain curve.
        /// </summary>
        public Vector3 EndPoint
        {
            set { anchor.end = transform.InverseTransformPoint(value); }
            get { return transform.TransformPoint(anchor.end); }
        }

        /// <summary>
        /// Start tangent point of chain curve.
        /// </summary>
        public Vector3 StartTangentPoint
        {
            set { anchor.startTangent = transform.InverseTransformPoint(value); }
            get { return transform.TransformPoint(anchor.startTangent); }
        }

        /// <summary>
        /// End tangent point of chain curve.
        /// </summary>
        public Vector3 EndTangentPoint
        {
            set { anchor.endTangent = transform.InverseTransformPoint(value); }
            get { return transform.TransformPoint(anchor.endTangent); }
        }

        /// <summary>
        /// Curve for chain.
        /// </summary>
        protected override ICurve Curve { get { return curve; } }

        /// <summary>
        /// Curve of chain.
        /// </summary>
        protected CubicBezierCurve curve = new CubicBezierCurve();
        #endregion

        #region Public Method
        /// <summary>
        /// Rebuild chain.
        /// </summary>
        public override void Rebuild()
        {
            curve.anchor = anchor;
            base.Rebuild();
        }
        #endregion
    }
}