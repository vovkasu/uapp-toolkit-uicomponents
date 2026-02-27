using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents.Editor
{
    [CustomEditor(typeof(ButtonLongPressToggle), true)]
    [CanEditMultipleObjects]
    public class ButtonLongPressToggleEditor : ButtonEditor
    {
        SerializedProperty HoldDurationSecProperty;
        SerializedProperty RepeatProperty;
        SerializedProperty RepeatPeriodSecProperty;
        SerializedProperty OnLongPressActionProperty;
        SerializedProperty ToggledSpriteState;
        SerializedProperty ToggledColorBlock;
        SerializedProperty ToggledAnimationTriggers;

        private ButtonLongPressToggle _targetCasted;

        protected override void OnEnable()
        {
            base.OnEnable();
            _targetCasted = (ButtonLongPressToggle)target;

            HoldDurationSecProperty = serializedObject.FindProperty(nameof(ButtonLongPressToggle.HoldDurationSec));
            RepeatProperty = serializedObject.FindProperty(nameof(ButtonLongPressToggle.Repeat));
            RepeatPeriodSecProperty = serializedObject.FindProperty(nameof(ButtonLongPressToggle.RepeatPeriodSec));
            OnLongPressActionProperty = serializedObject.FindProperty(nameof(ButtonLongPressToggle.OnLongPressAction));
            ToggledSpriteState = serializedObject.FindProperty(nameof(ButtonLongPressToggle.ToggledSpriteState));
            ToggledColorBlock = serializedObject.FindProperty(nameof(ButtonLongPressToggle.ToggledColorBlock));
            ToggledAnimationTriggers = serializedObject.FindProperty(nameof(ButtonLongPressToggle.ToggledAnimationTriggers));
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

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Toggle Settings", EditorStyles.boldLabel);
            switch (_targetCasted.transition)
            {
                case Selectable.Transition.ColorTint:
                    EditorGUILayout.PropertyField(ToggledColorBlock);
                    break;
                case Selectable.Transition.SpriteSwap:
                    EditorGUILayout.PropertyField(ToggledSpriteState);
                    break;
                case Selectable.Transition.Animation:
                    EditorGUILayout.PropertyField(ToggledAnimationTriggers);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}