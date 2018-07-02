/*************************************************************************
 *  Copyright © 2017-2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  IChain.cs
 *  Description  :  Define interface of chain.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  7/3/2017
 *  Description  :  Initial development version.
 *************************************************************************/

namespace Mogoson.Machinery
{
    /// <summary>
    /// Interface of chain.
    /// </summary>
    public interface IChain : IMechanism
    {
        #region Method
        /// <summary>
        /// Rebuild the curve of chain base on anchors.
        /// </summary>
        /// <param name="close">Curve is close?</param>
        void RebuildCurve(bool close);

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