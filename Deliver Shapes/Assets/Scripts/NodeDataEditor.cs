using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Net.NetworkInformation;

[CustomEditor(typeof(NodeData))]
public class NodeDataEditor : Editor {

    public override void OnInspectorGUI() {
        serializedObject.Update();

        NodeData nodeData = (NodeData)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("nodeType"));

        switch (nodeData.NodeType) {
            case NodeType.Main:
                break;
            case NodeType.Other:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredIngredientsToUnlock"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("inputIngredients"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumResourceCapacity"));
                break;
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("earnedLinkCount"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("outputResourceType"));

        serializedObject.ApplyModifiedProperties();
    }

}
