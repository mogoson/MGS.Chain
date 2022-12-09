/*************************************************************************
 *  Copyright © 2017-2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  MonoCurveChain.cs
 *  Description  :  Define chain base on MonoCurve.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  7/4/2017
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.Curve;
using UnityEngine;

namespace MGS.Chain
{
    /// <summary>
    /// Chain base on MonoCurve.
    /// </summary>
    [ExecuteInEditMode]
    public class MonoCurveChain : MonoChain
    {
        /// <summary>
        /// Length of chain.
        /// </summary>
        public override float Length { get { return curve.Length; } }

        /// <summary>
        /// Center curve of chain.
        /// </summary>
        protected IMonoCurve curve;

        /// <summary>
        /// Rebuild chain base curve.
        /// </summary>
        /// <param name="curve"></param>
        public virtual void Rebuild(IMonoCurve curve)
        {
            this.curve = curve;
            Rebuild();
        }

        /// <summary>
        /// [MESSAGE] On mono curve rebuild.
        /// </summary>
        /// <param name="curve"></param>
        private void OnMonoCurveRebuild(IMonoCurve curve)
        {
            Rebuild(curve);
        }

        /// <summary>
        /// Anchor node to chain.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="differ"></param>
        protected override void AnchorNode(Node node, float differ)
        {
            var t = node.ID * differ;
            var nodePos = curve.Evaluate(t);

            var dt = (node.ID + 1) * differ;
            var deltaPos = curve.Evaluate(dt);

            var secant = (deltaPos - nodePos).normalized;
            var worldUp = Vector3.Cross(secant, transform.up);

            node.transform.position = nodePos;
            node.transform.LookAt(deltaPos, worldUp);
        }
    }
}