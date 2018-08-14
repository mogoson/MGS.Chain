/*************************************************************************
 *  Copyright © 2017-2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  ChainEditor.cs
 *  Description  :  Custom editor for Chain.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  6/21/2017
 *  Description  :  Initial development version.
 *************************************************************************/

using Mogoson.UEditor;
using System;
using UnityEditor;
using UnityEngine;

namespace Mogoson.CurveChain
{
    [CustomEditor(typeof(Chain), true)]
    [CanEditMultipleObjects]
    public class ChainEditor : GenericEditor
    {
        #region Field and Property
        protected Chain Target { get { return target as Chain; } }
        protected const float Delta = 0.1f;
        #endregion

        #region Protected Method
        protected virtual void OnEnable()
        {
            if (Target.anchorRoot)
            {
                Target.anchorRoot.localPosition = Vector3.zero;
                Target.anchorRoot.localRotation = Quaternion.identity;
                if (Target.anchorRoot.childCount >= 2)
                    Target.RebuildCurve(true);
            }
            if (Target.nodeRoot)
            {
                Target.nodeRoot.localPosition = Vector3.zero;
                Target.nodeRoot.localRotation = Quaternion.identity;
            }
        }

        protected virtual void OnSceneGUI()
        {
            #region Coordinate System
            var horizontal = Target.transform.right * LineLength;
            var vertical = Target.transform.up * LineLength;

            Handles.color = Blue;
            Handles.DrawLine(Target.transform.position - horizontal, Target.transform.position + horizontal);
            Handles.DrawLine(Target.transform.position - vertical, Target.transform.position + vertical);
            DrawAdaptiveSphereCap(Target.transform.position, Quaternion.identity, NodeSize);
            #endregion

            #region Anchors And Curve
            if (Target.anchorRoot)
            {
                foreach (Transform anchor in Target.anchorRoot)
                {
                    DrawAdaptiveSphereCap(anchor.position, Quaternion.identity, NodeSize);
                }

                if (Target.anchorRoot.childCount >= 2)
                {
                    var maxTime = Target.Curve[Target.Curve.KeyframeCount - 1].key;
                    for (float timer = 0; timer < maxTime; timer += Delta)
                    {
                        var timerPoint = Target.anchorRoot.TransformPoint(Target.Curve.GetPointAt(timer));
                        var deltaPoint = Target.anchorRoot.TransformPoint(Target.Curve.GetPointAt(Mathf.Min(timer + Delta, maxTime)));
                        Handles.DrawLine(timerPoint, deltaPoint);
                    }
                }
            }
            #endregion

            if (AnchorEditor.IsOpen)
            {
                #region Circular Settings
                if (AnchorEditor.IsCircularSettingsValid)
                {
                    var from = Quaternion.AngleAxis(AnchorEditor.From, Target.transform.forward) * Vector3.up;
                    var to = Quaternion.AngleAxis(AnchorEditor.To, Target.transform.forward) * Vector3.up;
                    var angle = AnchorEditor.To - AnchorEditor.From;

                    Handles.color = Color.green;
                    Handles.DrawWireArc(AnchorEditor.Center.position, Target.transform.forward, from, angle, AnchorEditor.Radius);

                    DrawSphereArrow(AnchorEditor.Center.position, from, AnchorEditor.Radius, NodeSize);
                    DrawSphereArrow(AnchorEditor.Center.position, to, AnchorEditor.Radius, NodeSize);

                    if (AnchorEditor.CircularCount > 2)
                    {
                        var space = angle / (AnchorEditor.CircularCount - 1);
                        for (int i = 0; i < AnchorEditor.CircularCount - 2; i++)
                        {
                            var direction = Quaternion.AngleAxis(AnchorEditor.From + space * (i + 1), Target.transform.forward) * Vector3.up;
                            DrawSphereArrow(AnchorEditor.Center.position, direction.normalized, AnchorEditor.Radius, NodeSize);
                        }
                    }
                }
                #endregion

                #region Linear Settings
                if (AnchorEditor.IsLinearSettingsValid)
                {
                    var direction = (AnchorEditor.End.position - AnchorEditor.Start.position).normalized;
                    var space = Vector3.Distance(AnchorEditor.Start.position, AnchorEditor.End.position) / (AnchorEditor.LinearCount + 1);

                    Handles.color = Color.green;
                    Handles.DrawLine(AnchorEditor.Start.position, AnchorEditor.End.position);
                    for (int i = 0; i < AnchorEditor.LinearCount; i++)
                    {
                        DrawAdaptiveSphereCap(AnchorEditor.Start.position + direction * space * (i + 1), Quaternion.identity, NodeSize);
                    }
                }
                #endregion
            }
        }

        protected void EstimateCount()
        {
            var estimate = Target.Curve[Target.Curve.KeyframeCount - 1].key / Target.space;
            Target.count = (int)Math.Round(estimate, MidpointRounding.AwayFromZero);
            MarkSceneDirty();
        }

        protected void DeleteNodes()
        {
            while (Target.nodeRoot.childCount > 0)
            {
                DestroyImmediate(Target.nodeRoot.GetChild(0).gameObject);
            }
            MarkSceneDirty();
        }
        #endregion

        #region Public Method
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (Target.anchorRoot == null)
                return;

            Target.anchorRoot.localPosition = Vector3.zero;
            Target.anchorRoot.localRotation = Quaternion.identity;

            if (Target.anchorRoot.childCount < 2)
                return;

            if (Target.Curve == null)
                Target.RebuildCurve(true);

            if (Target.nodeRoot == null || Target.nodePrefab == null)
                return;

            GUILayout.Space(10);
            GUILayout.BeginHorizontal("Node Editor", "Window", GUILayout.Height(45));
            if (GUILayout.Button("Estimate"))
                EstimateCount();

            if (GUILayout.Button("Create"))
            {
                DeleteNodes();
                Target.CreateNodes();
            }

            if (GUILayout.Button("Delete"))
                DeleteNodes();
            GUILayout.EndHorizontal();
        }
        #endregion
    }
}