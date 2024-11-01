using System;
using System.Collections.Generic;
using DialogSystem.Runtime.Attributes;
namespace DialogSystem.Runtime.Structure.ScriptableObjects
{
    [Serializable]
    public class DialogEvent
    {
        [DialogTagSelector] public List<string> EventTargets;
        public string EventMessage;
    }
}