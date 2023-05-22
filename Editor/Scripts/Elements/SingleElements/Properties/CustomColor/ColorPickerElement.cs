using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.unityscriptingtools.Editor.PopupWindows.CustomColorPicker;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.CustomColor
{
    public class ColorPickerElement : PropertyElement
    {
        private ColorPickerElementLayout _colorPickerElementLayout;
        private ColorPickerElementDraw _colorPickerElementDraw;

        public override PropertyElementLayout PropertyElementLayout => _colorPickerElementLayout;
        protected override PropertyElementDraw PropertyElementDraw => _colorPickerElementDraw;

        // private SerializedObject _serializedObject;

        /// <summary>
        ///    Given a property containing a color value, this element will add a button to the right of the
        ///    color field which brings up a popup window where a preset color can be selected.
        /// </summary>
        public ColorPickerElement(
            string varName,
            GUIContent guiContent,
            SingleCustomSettings singleCustomSettings,
            Action changeCallBack,
            bool hideOnDisable = false,
            params ElementCondition[] conditions )
            : base( varName, guiContent, singleCustomSettings, changeCallBack, hideOnDisable, conditions )
        {
        }

        // protected override void InitializeElement( SerializedObject targetScriptableObject )
        // {
        //     // _serializedObject = targetScriptableObject;
        //     base.InitializeElement( targetScriptableObject );
        // }

        protected override void InitializeLayout() => _colorPickerElementLayout = new ColorPickerElementLayout( this );

        protected override void InitializeDraw() => _colorPickerElementDraw = new ColorPickerElementDraw( this );

        // public void OnColorSelection( unityscriptingtools.Runtime.Services.CustomColors.CustomColor color )
        // {
        //     // ColorPickerHandler.OnColorSelected -= OnColorSelection;
        //
        //     Debug.Log( $"Color picker returned color: {GetColoredString( color.name, color.GetHex() )}" );
        //     // ColorPickerHandler.Close();
        //     
        //     // The change to the property must be applied after it was modified by the color picker or it will be lost.
        //     // _serializedObject.ApplyModifiedProperties();
        //     
        //     // Bug: For now, had to disable change callbacks on colorPickerElements. Callback also triggers ApplyModifiedProperties which starts an endless cycle of update even calls. Not sure why just yet.
        //     // ChangeCallBack?.Invoke();
        //     // Debug.Log( $"Custom color property was changed to: {GetColoredString( "this", Property.colorValue )}" );
        // }
    }
}