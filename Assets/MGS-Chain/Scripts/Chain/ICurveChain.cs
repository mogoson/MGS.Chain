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
using Mogoson.Machinery;

namespace Mogoson.CurveChain
{
    /// <summary>
    /// Interface of chain.
    /// </summary>
    public interface ICurveChain : IMechanism, ICurve
    {
        #region Property
        /// <summary>
        /// Count of chain nodes.
        /// </summary>
        int Count { set; get; }

        /// <summary>
        /// Space of nodes.
        /// </summary>
        float Space { set; get; }
        #endregion

        #region Method
        /// <summary>
        /// Rebuild the curve of chain.
        /// </summary>
        void RebuildCurve();

        /// <summary>
        /// Estimate count of chain nodes.
        /// </summary>
        /// <returns>Count of chain nodes.</returns>
        int EstimateCount();

        /// <summary>
        /// Create chain nodes.
        /// </summary>
        void CreateNodes();

        /// <summary>
        /// Delete chain nodes.
        /// </summary>
        void DeleteNodes();
        #endregion
    }
}