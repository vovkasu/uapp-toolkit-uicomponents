using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace UAppToolkit.UIComponents
{
    public class LocalizeStringParameterSetter : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _localizeStringEvent;
        [SerializeField] private string _parameterName = "value";

        public void SetString(string value)
        {
            _localizeStringEvent.StringReference.Add(_parameterName, new StringVariable { Value = value });
            _localizeStringEvent.StringReference.RefreshString();
        }
    }
}