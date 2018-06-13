/*************************************************************************
 *  Copyright © 2017-2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  AnchorEditor.cs
 *  Description  :  Custom editor for chain anchor.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  7/3/2017
 *  Description  :  Initial development version.
 *************************************************************************/

using UnityEditor;
using UnityEngine;

#if UNITY_5_3_OR_NEWER
using UnityEditor.SceneManagement;
#endif

namespace Mogoson.Machinery
{
    public class AnchorEditor : EditorWindow
    {
        #region Field and Property
        protected static AnchorEditor instance;
        protected static Vector2 scrollPos;
        protected const float LeftAlign = 150;
        protected const float Paragraph = 2.5f;

        protected static Chain targetChain;
        protected static Material material;
        protected const string MaterialPath = "Assets/MGS-MechanicalDrive/Materials/Anchor.mat";

        protected static string prefix = "Anchor";
        protected const string RendererName = "AnchorRenderer";
        protected static float size = 0.05f;

        public static bool IsOpen { protected set; get; }

        public static Transform Center { protected set; get; }
        public static float Radius { protected set; get; }
        public static float From { protected set; get; }
        public static float To { protected set; get; }
        public static int CountC { protected set; get; }
        public static bool IsCircularSettingsReasonable
        {
            get { return Center && Radius > 0 && From < To && CountC > 0; }
        }

        public static Transform Start { protected set; get; }
        public static Transform End { protected set; get; }
        public static int CountL { protected set; get; }
        public static bool IsLinearSettingsReasonable
        {
            get { return Start && End && CountL > 0; }
        }
        #endregion

        #region Private Method
        [MenuItem("Tool/Anchor Editor &A")]
        private static void ShowEditor()
        {
            targetChain = GetChainFromSelection();
            ShowEditorWindow();
        }
        #endregion

        #region protected Method
        protected static void ShowEditorWindow()
        {
            material = (Material)AssetDatabase.LoadAssetAtPath(MaterialPath, typeof(Material));
            instance = GetWindow<AnchorEditor>("Anchor Editor", true);
            instance.Show();
            IsOpen = true;
        }

        protected static Chain GetChainFromSelection()
        {
            if (Selection.activeGameObject)
                return Selection.activeGameObject.GetComponent<Chain>();
            else
                return null;
        }

        protected virtual void OnSelectionChange()
        {
            targetChain = GetChainFromSelection(); ;
            Repaint();
        }

        protected virtual void OnGUI()
        {
            if (targetChain)
            {
                if (targetChain.anchorRoot)
                {
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

                    #region Circular Anchor Creater
                    GUILayout.BeginVertical("Circular Anchor Creater", "Window", GUILayout.Height(140));

                    EditorGUI.BeginChangeCheck();
                    Center = (Transform)EditorGUILayout.ObjectField("Center", Center, typeof(Transform), true);
                    Radius = EditorGUILayout.FloatField("Radius", Radius);
                    From = EditorGUILayout.FloatField("From", From);
                    To = EditorGUILayout.FloatField("To", To);
                    CountC = EditorGUILayout.IntField("Count", CountC);
                    if (EditorGUI.EndChangeCheck())
                        SceneView.RepaintAll();

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(LeftAlign);
                    if (GUILayout.Button("Create"))
                    {
                        if (IsCircularSettingsReasonable)
                            CreateCircularAnchors();
                        else
                            ShowNotification(new GUIContent("The parameter settings of circular anchor creater is not reasonable."));
                    }
                    if (GUILayout.Button("Reset"))
                        ResetCircularAnchorCreater();
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    #endregion

                    #region Linear Anchor Creater
                    GUILayout.BeginVertical("Linear Anchor Creater", "Window", GUILayout.Height(105));

                    EditorGUI.BeginChangeCheck();
                    Start = (Transform)EditorGUILayout.ObjectField("Start", Start, typeof(Transform), true);
                    End = (Transform)EditorGUILayout.ObjectField("End", End, typeof(Transform), true);
                    CountL = EditorGUILayout.IntField("Count", CountL);
                    if (EditorGUI.EndChangeCheck())
                        SceneView.RepaintAll();

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(LeftAlign);
                    if (GUILayout.Button("Create"))
                    {
                        if (IsLinearSettingsReasonable)
                            CreateLinearAnchors();
                        else
                            ShowNotification(new GUIContent("The parameter settings of linear anchor creater is not reasonable."));
                    }
                    if (GUILayout.Button("Reset"))
                        ResetLinearAnchorCreater();
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    #endregion

                    #region Unified Anchor Manager
                    GUILayout.BeginVertical("Unify Anchor Manager", "Window");
                    prefix = EditorGUILayout.TextField("Prefix", prefix);

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(LeftAlign);
                    if (GUILayout.Button("Rename"))
                    {
                        if (prefix.Trim() == string.Empty)
                            ShowNotification(new GUIContent("The value of prefix cannot be empty."));
                        else
                            RenameAnchors();
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(Paragraph);
                    size = EditorGUILayout.FloatField("Renderer", size);

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(LeftAlign);
                    if (GUILayout.Button("Attach"))
                    {
                        RemoveAnchorRenderer();
                        AttachAnchorRenderer();
                    }
                    if (GUILayout.Button("Remove"))
                        RemoveAnchorRenderer();
                    GUILayout.EndHorizontal();

                    GUILayout.Space(Paragraph);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Anchors", GUILayout.Width(LeftAlign - 4));
                    if (GUILayout.Button("Delete"))
                    {
                        var delete = EditorUtility.DisplayDialog(
                         "Delete Anchors",
                         "This operate will delete all of the chain anchors.\nAre you sure continue to do this?",
                         "Yes",
                         "Cancel");

                        if (delete)
                            DeleteAnchors();
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    #endregion

                    EditorGUILayout.EndScrollView();
                }
                else
                    EditorGUILayout.HelpBox("The anchor root of chain has not been assigned.", MessageType.Error);
            }
            else
                EditorGUILayout.HelpBox("No chain object is selected.", MessageType.Info);
        }

        protected virtual void OnDestroy()
        {
            targetChain = null;
            IsOpen = false;
            SceneView.RepaintAll();
        }

        protected void CreateCircularAnchors()
        {
            var space = (To - From) / (CountC == 1 ? 1 : CountC - 1);
            for (int i = 0; i < CountC; i++)
            {
                var direction = Quaternion.AngleAxis(From + space * i, targetChain.anchorRoot.forward) * Vector3.up;
                var tangent = -Vector3.Cross(direction, targetChain.anchorRoot.forward);
                var position = Center.position + direction * Radius;
                CreateAnchor("CircularAnchor" + " (" + i + ")", position, position + tangent, direction, Center.GetSiblingIndex());
            }
            ResetCircularAnchorCreater();
            RefreshChainCurve();
            MarkSceneDirty();
        }

        protected void ResetCircularAnchorCreater()
        {
            Center = null;
            Radius = From = To = CountC = 0;
            SceneView.RepaintAll();
        }

        protected void CreateLinearAnchors()
        {
            var direction = (End.position - Start.position).normalized;
            var space = Vector3.Distance(Start.position, End.position) / (CountL + 1);
            for (int i = 0; i < CountL; i++)
            {
                CreateAnchor("LinearAnchor" + " (" + i + ")", Start.position + direction * space * (i + 1),
                    End.position, Vector3.Cross(direction, targetChain.anchorRoot.forward), End.GetSiblingIndex());
            }
            ResetLinearAnchorCreater();
            RefreshChainCurve();
            MarkSceneDirty();
        }

        protected void ResetLinearAnchorCreater()
        {
            Start = End = null;
            CountL = 0;
            SceneView.RepaintAll();
        }

        protected void CreateAnchor(string anchorName, Vector3 position, Vector3 lookAtPos, Vector3 worldUp, int siblingIndex)
        {
            var newAnchor = new GameObject(anchorName).transform;
            newAnchor.position = position;
            newAnchor.LookAt(lookAtPos, worldUp);
            newAnchor.parent = targetChain.anchorRoot;
            newAnchor.SetSiblingIndex(siblingIndex);
            AttachRenderer(newAnchor);
        }

        protected void RefreshChainCurve()
        {
            if (targetChain.anchorRoot.childCount >= 2)
                targetChain.CreateCurve();
        }

        protected void RenameAnchors()
        {
            for (int i = 0; i < targetChain.anchorRoot.childCount; i++)
            {
                targetChain.anchorRoot.GetChild(i).name = prefix.Trim() + " (" + i + ")";
            }
            MarkSceneDirty();
        }

        protected void AttachAnchorRenderer()
        {
            foreach (Transform anchor in targetChain.anchorRoot)
            {
                AttachRenderer(anchor);
            }
            MarkSceneDirty();
        }

        protected void AttachRenderer(Transform anchor)
        {
            var renderer = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            DestroyImmediate(renderer.GetComponent<Collider>());
            renderer.GetComponent<Renderer>().sharedMaterial = material;
            renderer.name = RendererName;
            renderer.parent = anchor;
            renderer.localPosition = Vector3.zero;
            renderer.localRotation = Quaternion.identity;
            renderer.localScale = Vector3.one * size;
        }

        protected void RemoveAnchorRenderer()
        {
            foreach (Transform anchor in targetChain.anchorRoot)
            {
                var renderer = anchor.Find(RendererName);
                if (renderer)
                    DestroyImmediate(renderer.gameObject);
            }
            MarkSceneDirty();
        }

        protected void DeleteAnchors()
        {
            while (targetChain.anchorRoot.childCount > 0)
            {
                DestroyImmediate(targetChain.anchorRoot.GetChild(0).gameObject);
            }
            MarkSceneDirty();
        }

        protected void MarkSceneDirty()
        {
#if UNITY_5_3_OR_NEWER
            EditorSceneManager.MarkAllScenesDirty();
#else
            EditorApplication.MarkSceneDirty();
#endif
        }
        #endregion
    }
}