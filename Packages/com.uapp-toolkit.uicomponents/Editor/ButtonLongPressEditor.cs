using UnityEditor;
using UnityEditor.UI;

namespace UAppToolkit.UIComponents.Editor
{
    [CustomEditor(typeof(ButtonLongPress), true)]
    [CanEditMultipleObjects]
    public class ButtonLongPressEditor : ButtonEditor
    {
        SerializedProperty HoldDurationSecProperty;
        SerializedProperty RepeatProperty;
        SerializedProperty RepeatPeriodSecProperty;
        SerializedProperty OnLongPressActionProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            HoldDurationSecProperty = serializedObject.FindProperty("HoldDurationSec");
            RepeatProperty = serializedObject.FindProperty("Repeat");
            RepeatPeriodSecProperty = serializedObject.FindProperty("RepeatPeriodSec");
            OnLongPressActionProperty = serializedObject.FindProperty("OnLongPressAction");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();

            EditorGUILayout.PropertyField(HoldDurationSecProperty);
            EditorGUILayout.PropertyField(RepeatProperty);
            if (RepeatProperty.boolValue)
            {
                EditorGUILayout.PropertyField(RepeatPeriodSecProperty);
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(OnLongPressActionProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
