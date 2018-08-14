/*************************************************************************
 *  Copyright © 2017-2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  MonoCurveChain.cs
 *  Description  :  Define chain base on curve.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  7/4/2017
 *  Description  :  Initial development version.
 *************************************************************************/

using Mogoson.Machinery;
using UnityEngine;

namespace Mogoson.CurveChain
{
    /// <summary>
    /// Chain base on curve.
    /// </summary>
    public abstract class MonoCurveChain : Mechanism, ICurveChain
    {
        #region Field and Property
        /// <summary>
        /// Root of chain track anchors.
        /// </summary>
        public Transform anchorRoot;

        /// <summary>
        /// Root of chain nodes.
        /// </summary>
        public Transform nodeRoot;

        /// <summary>
        /// Prefab of nodes.
        /// </summary>
        public GameObject nodePrefab;

        /// <summary>
        /// Count of chain nodes.
        /// </summary>
        [SerializeField]
        protected int count = 50;

        /// <summary>
        /// Space of nodes.
        /// </summary>
        [SerializeField]
        protected float space = 0.1f;

        /// <summary>
        /// Nodes of chain.
        /// </summary>
        [HideInInspector]
        public Node[] nodes;

        /// <summary>
        /// Count of chain nodes.
        /// </summary>
        public int Count
        {
            set
            {
                if (value > 0)
                    count = value;
            }
            get { return count; }
        }

        /// <summary>
        /// Space of nodes.
        /// </summary>
        public float Space
        {
            set
            {
                if (value > 0)
                    space = value;
            }
            get { return space; }
        }

        public float Length { get { return 0; } }

        public float MaxKey { get { return 0; } }

        /// <summary>
        /// Delta time for curve
        /// </summary>
        protected const float delta = 0.01f;
        #endregion

        #region Protected Method
        /// <summary>
        /// Tow node move and rotate base on curve.
        /// </summary>
        /// <param name="node">Target node to tow.</param>
        /// <param name="time">Time of current in curve.</param>
        protected void TowNodeOnCurve(Transform node, float time)
        {
            //Calculate position and direction.
            var nodePos = GetPointAt(time);
            var deltaPos = GetPointAt(time + delta);
            var secant = (deltaPos - nodePos).normalized;
            var worldUp = Vector3.Cross(secant, transform.forward);

            //Update position and direction.
            node.position = nodePos;
            node.LookAt(deltaPos, worldUp);
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Get point on curve at key.
        /// </summary>
        /// <param name="key">Key of curve.</param>
        /// <returns>The point on curve at key.</returns>
        public abstract Vector3 GetPointAt(float key);

        /// <summary>
        /// Rebuild the curve of chain.
        /// </summary>
        public abstract void RebuildCurve();

        /// <summary>
        /// Estimate count of chain nodes.
        /// </summary>
        /// <returns>Count of chain nodes.</returns>
        public abstract int EstimateCount();

        /// <summary>
        /// Create chain nodes.
        /// </summary>
        public virtual void CreateNodes()
        {
            nodes = new Node[count];
            for (int i = 0; i < count; i++)
            {
                //Create node.
                var nodeClone = Instantiate(nodePrefab);
                nodeClone.transform.SetParent(nodeRoot);

                //Tow node.
                TowNodeOnCurve(nodeClone.transform, i * space);

                //Set node ID.
                nodes[i] = nodeClone.GetComponent<Node>();
                nodes[i].ID = i;
            }
        }

        /// <summary>
        /// Delete chain nodes.
        /// </summary>
        public virtual void DeleteNodes()
        {
            while (nodeRoot.childCount > 0)
            {
                Destroy(nodeRoot.GetChild(0).gameObject);
            }
        }
        #endregion
    }
}