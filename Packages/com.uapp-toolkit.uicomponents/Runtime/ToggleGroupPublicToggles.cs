using System.Collections.Generic;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents
{
    public class ToggleGroupPublicToggles : ToggleGroup
    {
        public IEnumerable<Toggle> GetToggles()
        {
            return m_Toggles;
        }
    }
}