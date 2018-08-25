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

using Mogoson.Curve;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mogoson.CurveChain
{
    /// <summary>
    /// Chain base on curve.
    /// </summary>
    public abstract class MonoCurveChain : MonoBehaviour, ICurveChain
    {
        #region Field and Property
        /// <summary>
        /// Prefab of nodes.
        /// </summary>
        public GameObject nodePrefab;

        /// <summary>
        /// Segment length of chain node.
        /// </summary>
        [SerializeField]
        protected float segment = 0.1f;

        /// <summary>
        /// Nodes of chain.
        /// </summary>
        [HideInInspector]
        public List<Node> nodes = new List<Node>();

        /// <summary>
        /// Segment length of chain node.
        /// </summary>
        public float Segment
        {
            set
            {
                if (value > 0)
                    segment = value;
            }
            get { return segment; }
        }

        /// <summary>
        /// Max key of chain center curve.
        /// </summary>
        public virtual float MaxKey { get { return Curve.MaxKey; } }

        /// <summary>
        /// Length of chain center curve.
        /// </summary>
        public virtual float Length { get { return length; } }

        /// <summary>
        /// Center curve for chain.
        /// </summary>
        protected abstract ICurve Curve { get; }

        /// <summary>
        /// Delta to calculate tangent.
        /// </summary>
        protected const float Delta = 0.001f;

        /// <summary>
        /// Length of chain center curve.
        /// </summary>
        protected float length = 0.0f;

        /// <summary>
        /// Segment count of chain.
        /// </summary>
        protected int segmentCount = 0;
        #endregion

        #region Protected Method
        protected virtual void Reset()
        {
            Rebuild();
        }

        protected virtual void Awake()
        {
            Rebuild();
        }

        /// <summary>
        /// Add a node to chain nodes.
        /// </summary>
        protected virtual void AddNode()
        {
            //Create node.
            var nodeClone = Instantiate(nodePrefab);
            nodeClone.transform.SetParent(transform);

            //Add new node to chain nodes.
            var newNode = nodeClone.GetComponent<Node>();
            newNode.ID = nodes.Count;
            nodes.Add(newNode);
        }

        /// <summary>
        /// Remove the last node of chain nodes.
        /// </summary>
        protected virtual void RemoveNode()
        {
            if (nodes.Count > 0)
            {
                var node = nodes[nodes.Count - 1];
#if UNITY_EDITOR
                DestroyImmediate(node.gameObject);
#else
                Destroy(node.gameObject);
#endif
                nodes.Remove(node);
            }
        }

        /// <summary>
        /// Tow node move and rotate base on curve.
        /// </summary>
        /// <param name="node">Target node to tow.</param>
        /// <param name="key">Key of node in curve.</param>
        protected void TowNodeOnCurve(Transform node, float key)
        {
            key = Math.Min(key, MaxKey - Delta);

            var nodePos = GetPointAt(key);
            var deltaPos = GetPointAt(key + Delta);
            var secant = (deltaPos - nodePos).normalized;
            var worldUp = Vector3.Cross(secant, transform.forward);

            node.position = nodePos;
            node.LookAt(deltaPos, worldUp);
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Rebuild chain nodes.
        /// </summary>
        public virtual void Rebuild()
        {
            length = Curve.Length;
            segmentCount = (int)Math.Round(length / segment, MidpointRounding.AwayFromZero);

#if UNITY_EDITOR
            if (!Application.isPlaying && nodePrefab == null)
            {
                return;
            }
#endif
            while (nodes.Count < segmentCount)
            {
                AddNode();
            }

            while (nodes.Count > segmentCount)
            {
                RemoveNode();
            }

            var keySegment = MaxKey / (segmentCount - 1);
            foreach (var node in nodes)
            {
                TowNodeOnCurve(node.transform, keySegment * node.ID);
            }
        }

        /// <summary>
        /// Get point from center curve of chain at key.
        /// </summary>
        /// <param name="key">Key of chain center curve.</param>
        /// <returns>Point on chain curve at key.</returns>
        public Vector3 GetPointAt(float key)
        {
            return transform.TransformPoint(Curve.GetPointAt(key));
        }
        #endregion
    }
}