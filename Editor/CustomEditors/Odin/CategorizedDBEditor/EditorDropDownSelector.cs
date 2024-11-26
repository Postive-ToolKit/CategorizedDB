#if ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;

namespace Postive.CategorizedDB.Editor.CustomEditors.Odin.CategorizedDBEditor
{
    public class EditorDropDownSelector : OdinSelector<string>
    {
        public class Content
        {
            public string Title;
            public Action OnSelect;
        }
        private readonly Content[] _dropdownContents;
        public EditorDropDownSelector(Content[] dropdownContents) {
            _dropdownContents = dropdownContents;
            SelectionConfirmed += InvokeMenuEvent;
        }

        private void InvokeMenuEvent(IEnumerable<string> selection) {
            string selected = selection.FirstOrDefault();
            if (string.IsNullOrEmpty(selected)) return;
            var content = _dropdownContents.FirstOrDefault(x => x.Title == selected);
            content?.OnSelect?.Invoke();
        }
        protected override void BuildSelectionTree(OdinMenuTree tree) {

            tree.Selection.SupportsMultiSelect = false;
            tree.Config.DrawSearchToolbar = true;
            tree.AddRange(_dropdownContents.Select(x => x.Title), x => x);
        }
    }
}
#endif