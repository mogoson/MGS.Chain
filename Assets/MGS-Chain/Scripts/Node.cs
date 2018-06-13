/*************************************************************************
 *  Copyright © 2017-2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  Node.cs
 *  Description  :  Define Node component.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  6/21/2017
 *  Description  :  Initial development version.
 *************************************************************************/

using UnityEngine;

namespace Mogoson.Machinery
{
    [AddComponentMenu("Mogoson/Machinery/Node")]
    public class Node : MonoBehaviour
    {
        #region Field and Property
        /// <summary>
        /// ID of node in the chain.
        /// </summary>
        public int ID;
        #endregion
    }
}