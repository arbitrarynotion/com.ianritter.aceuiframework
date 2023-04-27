using System;
using UnityEditor;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore
{
    [Serializable]
    public class ScriptThemeInfo
    {
        // public string Name { get; }
        public MonoScript script;
        public AceTheme theme;

        public ScriptThemeInfo( string name, MonoScript script, AceTheme theme )
        {
            // Name = name;
            this.script = script;
            this.theme = theme;
        }
    }
}