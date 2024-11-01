using UnityEngine;

namespace DialogSystem.Runtime.Attributes
{
    public class SDAMReadOnlyAttribute : PropertyAttribute {
        public readonly bool runtimeOnly;
        public SDAMReadOnlyAttribute(bool runtimeOnly = false)
        {
            this.runtimeOnly = runtimeOnly;
        }
    }
}