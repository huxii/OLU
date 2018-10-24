﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ObjectControl))]
public class ObjectControlEditor : Editor
{
    public static ObjectControl objectController;

    void Awake()
    {
        objectController = (ObjectControl)target;
    }
}

[CustomEditor(typeof(PropControl))]
public class PropControlEditor : ObjectControlEditor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Add Slot"))
        {
            GameObject newSlot = ((PropControl)objectController).AddSlot();
            Selection.activeGameObject = newSlot;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}

[CustomEditor(typeof(PropClickControl))]
public class PropClickControlEditor : PropControlEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(PropDragControl))]
public class PropDragControlEditor : PropControlEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(PropAutoHolderControl))]
public class PropAutoHolderControlEditor : PropControlEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(PropAutoControl))]
public class PropAutoControlEditor : PropControlEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(PropAutoBasicControl))]
public class PropAutoBasicControlEditor : PropAutoControlEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(PropAutoDeactivateControl))]
public class PropAutoDeactivateControlEditor : PropAutoControlEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(PropAutoLoopControl))]
public class PropAutoLoopControlEditor : PropAutoControlEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(PropOneTimeSwipeControl))]
public class PropOneTimeSwipeControlEditor : PropAutoControlEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(PropDragTransformControl))]
public class PropDragTransformControlEditor : PropDragControlEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(MultiplePropControl))]
public class MultiplePropControlEditor : ObjectControlEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}