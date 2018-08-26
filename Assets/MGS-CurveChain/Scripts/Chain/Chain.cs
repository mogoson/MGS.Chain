﻿/*************************************************************************
 *  Copyright © 2017-2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  Chain.cs
 *  Description  :  Define Chain component.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  6/21/2017
 *  Description  :  Initial development version.
 *************************************************************************/

using Mogoson.Curve;
using UnityEngine;

namespace Mogoson.CurveChain
{
    [AddComponentMenu("Mogoson/CurveChain/Chain")]
    public class Chain : MonoBehaviour
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
        public int count = 50;

        /// <summary>
        /// Space of nodes.
        /// </summary>
        public float space = 0.1f;

        /// <summary>
        /// Nodes of chain.
        /// </summary>
        [HideInInspector]
        public Node[] nodes;

        /// <summary>
        /// VectorAnimationCurve of nodes.
        /// </summary>
        public HermiteCurve Curve { protected set; get; }

        /// <summary>
        /// Timer for VectorAnimationCurve.
        /// </summary>
        protected float timer = 0;

        /// <summary>
        /// Delta time for VectorAnimationCurve
        /// </summary>
        protected const float delta = 0.01f;
        #endregion

        #region Private Method
        /// <summary>
        /// Tow node move and rotate base on VectorAnimationCurve.
        /// </summary>
        /// <param name="node">Target node to tow.</param>
        /// <param name="time">Time of current in curve.</param>
        protected void TowNodeBaseOnCurve(Transform node, float time)
        {
            //Calculate position and direction.
            var nodePos = anchorRoot.TransformPoint(Curve.GetPointAt(time));
            var deltaPos = anchorRoot.TransformPoint(Curve.GetPointAt(time + delta));
            var secant = (deltaPos - nodePos).normalized;
            var worldUp = Vector3.Cross(secant, transform.forward);

            //Update position and direction.
            node.position = nodePos;
            node.LookAt(deltaPos, worldUp);
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Initialize chain.
        /// </summary>
        public void Initialize()
        {
            RebuildCurve(true);
        }

        /// <summary>
        /// Drive chain.
        /// </summary>
        /// <param name="velocity">Linear velocity.</param>
        public virtual void Drive(float velocity)
        {
            timer += velocity * Mathf.Deg2Rad * Time.deltaTime;
            foreach (var node in nodes)
            {
                TowNodeBaseOnCurve(node.transform, node.ID * space + timer);
            }
        }

        /// <summary>
        /// Rebuild the curve of chain base on anchors.
        /// </summary>
        /// <param name="close">Curve is close?</param>
        public virtual void RebuildCurve(bool close)
        {
            Curve = new HermiteCurve();
            Curve.PreWrapMode = Curve.PostWrapMode = WrapMode.Loop;

            //Add frame keys to curve.
            float time = 0;
            for (int i = 0; i < anchorRoot.childCount - 1; i++)
            {
                Curve.AddKeyframe(time, anchorRoot.GetChild(i).localPosition);
                time += Vector3.Distance(anchorRoot.GetChild(i).position, anchorRoot.GetChild(i + 1).position);
            }

            //Add last key.
            Curve.AddKeyframe(time, anchorRoot.GetChild(anchorRoot.childCount - 1).localPosition);

            if (close)
            {
                //Add the loop key(the first key).
                time += Vector3.Distance(anchorRoot.GetChild(anchorRoot.childCount - 1).position, anchorRoot.GetChild(0).position);
                Curve.AddKeyframe(time, anchorRoot.GetChild(0).localPosition);
            }

            //Smooth the in and out tangents of curve keyframes.
            Curve.SmoothTangents(0);
        }

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
                TowNodeBaseOnCurve(nodeClone.transform, i * space);

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