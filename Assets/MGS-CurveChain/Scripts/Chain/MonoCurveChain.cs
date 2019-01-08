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
        /// Prefab of chain node.
        /// </summary>
        [SerializeField]
        protected GameObject node;

        /// <summary>
        /// Prefab of chain roller.
        /// </summary>
        [SerializeField]
        protected GameObject link;

        /// <summary>
        /// Segment length of chain node.
        /// </summary>
        [SerializeField]
        protected float segment = 0.1f;

        /// <summary>
        /// Nodes of chain.
        /// </summary>
        [HideInInspector]
        [SerializeField]
        protected List<Node> nodes = new List<Node>();

        /// <summary>
        /// Prefab of chain node.
        /// </summary>
        public GameObject Node
        {
            set { node = value; }
            get { return node; }
        }

        /// <summary>
        /// Prefab of chain link.
        /// </summary>
        public GameObject Link
        {
            set { link = value; }
            get { return link; }
        }

        /// <summary>
        ///  Segment length of chain node (or link).
        /// </summary>
        public float Segment
        {
            set
            {
                if (value > 0)
                {
                    segment = value;
                }
            }
            get { return segment; }
        }

        /// <summary>
        /// Max key of chain center curve.
        /// </summary>
        public float MaxKey { get { return Curve.MaxKey; } }

        /// <summary>
        /// Length of chain center curve.
        /// </summary>
        public float Length { get { return Curve.Length; } }

        /// <summary>
        /// Center curve for chain.
        /// </summary>
        protected abstract ICurve Curve { get; }

        /// <summary>
        /// Delta to calculate tangent.
        /// </summary>
        protected const float Delta = 0.001f;

        /// <summary>
        /// Segment count of chain nodes.
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
        /// Add a node to chain last.
        /// </summary>
        protected void AddNodeToLast()
        {
            //Create node.
            var nodePrefab = node;
            if (Link != null && nodes.Count % 2 == 1)
            {
                nodePrefab = Link;
            }
            var nodeClone = Instantiate(nodePrefab);
            nodeClone.hideFlags = HideFlags.HideInHierarchy;
            nodeClone.transform.SetParent(transform);

            //Add new node to chain nodes.
            var newNode = nodeClone.GetComponent<Node>();
            newNode.ID = nodes.Count;
            nodes.Add(newNode);
        }

        /// <summary>
        /// Remove the last node of chain nodes.
        /// </summary>
        protected void RemoveLastNode()
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
            var worldUp = Vector3.Cross(secant, transform.up);

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
            segmentCount = (int)Math.Round(Length / segment, MidpointRounding.AwayFromZero);

#if UNITY_EDITOR
            if (!Application.isPlaying && node == null)
            {
                return;
            }
#endif
            while (nodes.Count < segmentCount)
            {
                AddNodeToLast();
            }

            while (nodes.Count > segmentCount)
            {
                RemoveLastNode();
            }

            var keySegment = MaxKey / (segmentCount - 1);
            foreach (var nodeItem in nodes)
            {
                TowNodeOnCurve(nodeItem.transform, keySegment * nodeItem.ID);
            }
        }

        /// <summary>
        /// Clear chain all nodes.
        /// </summary>
        public virtual void Clear()
        {
            foreach (var node in nodes)
            {
#if UNITY_EDITOR
                DestroyImmediate(node.gameObject);
#else
                Destroy(node.gameObject);
#endif
            }
            nodes.Clear();
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