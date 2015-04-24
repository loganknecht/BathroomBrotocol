using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TweenSequence))]
public class TweenSequenceEditor : UITweenerEditor {
    public override void OnInspectorGUI() {
        GUILayout.Space(6f);
        NGUIEditorTools.SetLabelWidth(120f);
        
        TweenSequence tw = target as TweenSequence;
        GUI.changed = false;
        
        if(GUI.changed) {
            NGUIEditorTools.RegisterUndo("Tween Change", tw);
            UnityEditor.EditorUtility.SetDirty(tw);
        }
        
        this.DrawModifiedCommonProperties();
    }
    
    private void DrawModifiedCommonProperties() {
        UITweener tw = target as UITweener;
        
        // EditorGUIUtility.LookLikeInspector();
        EditorGUIUtility.LookLikeControls();
        
        SerializedProperty tweenersProperty = this.serializedObject.FindProperty("tweeners");
        while(true) {
            Rect propertyRect = GUILayoutUtility.GetRect(0f, 16f);
            
            bool showChildren = (tweenersProperty.name == "tweeners") ? EditorGUI.PropertyField(propertyRect, tweenersProperty, new GUIContent("Tweens")) : EditorGUI.PropertyField(propertyRect, tweenersProperty);
            if(!tweenersProperty.NextVisible(showChildren)) {
                break;
            }
        }
        
        this.serializedObject.ApplyModifiedProperties();
        
        GUI.changed = false;
        
        if(GUI.changed) {
            NGUIEditorTools.RegisterUndo("Tween Change", tw);
            UnityEditor.EditorUtility.SetDirty(tw);
        }
        
        NGUIEditorTools.SetLabelWidth(80f);
        NGUIEditorTools.DrawEvents("On Finished", tw, tw.onFinished);
    }
}