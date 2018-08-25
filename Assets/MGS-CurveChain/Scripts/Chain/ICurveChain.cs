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

namespace Mogoson.CurveChain
{
    /// <summary>
    /// Interface of chain.
    /// </summary>
    public interface ICurveChain : ICurve
    {
        #region Property
        /// <summary>
        ///  Segment length of chain node.
        /// </summary>
        float Segment { set; get; }
        #endregion

        #region Method
        /// <summary>
        /// Rebuild chain nodes.
        /// </summary>
        void Rebuild();
        #endregion
    }
}