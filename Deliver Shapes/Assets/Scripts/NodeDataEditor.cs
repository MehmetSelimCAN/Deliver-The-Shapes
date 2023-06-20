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
            case NodeType.ConstantGeneratorNode:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredIngredientsToUnlock"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("earnedLinkCount"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("outputResourceType"));
                break;
            case NodeType.DependentGeneratorNode:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredIngredientsToUnlock"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("inputIngredients"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("earnedLinkCount"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("outputResourceType"));
                break;
            case NodeType.FinalNode:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredIngredientsToUnlock"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

}
