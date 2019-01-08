/*************************************************************************
 *  Copyright © 2017-2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  ICurveChain.cs
 *  Description  :  Define interface of chain.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  7/3/2017
 *  Description  :  Initial development version.
 *************************************************************************/

using Mogoson.Curve;
using UnityEngine;

namespace Mogoson.CurveChain
{
    /// <summary>
    /// Interface of chain.
    /// </summary>
    public interface ICurveChain : ICurve
    {
        #region Property
        /// <summary>
        /// Prefab of chain node.
        /// </summary>
        GameObject Node { set; get; }

        /// <summary>
        /// Prefab of chain link.
        /// </summary>
        GameObject Link { set; get; }

        /// <summary>
        ///  Segment length of chain node (or link).
        /// </summary>
        float Segment { set; get; }
        #endregion

        #region Method
        /// <summary>
        /// Rebuild chain nodes.
        /// </summary>
        void Rebuild();

        /// <summary>
        /// Clear chain all nodes.
        /// </summary>
        void Clear();
        #endregion
    }
}