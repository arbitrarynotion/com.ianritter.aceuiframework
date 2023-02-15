using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements
{
    /// <summary>
    ///     Manages the position rect for a single element. Root single elements follow no special rules, using the default
    ///     Unity control rect and wide mode adjustments. Non-root single elements simply use the rect assigned to them by
    ///     their parent section.
    /// </summary>
    public abstract class SingleElementLayout : ElementLayout
    {
        protected abstract SingleElement SingleElement { get; }
        protected override Element Element => SingleElement;

        protected virtual float GetFieldMinWidth() => 0;

        /// <summary>
        ///     A property element is composed of a label and a field. This is width assigned to the field part of this element.
        /// </summary>
        public float FieldMinWidth =>
            SingleElement.SingleCustomSettings.FieldMinWidth > 0
                ? SingleElement.SingleCustomSettings.FieldMinWidth
                : GetFieldMinWidth();

        /// <summary>
        ///     A property element is composed of a label and a field. This is width assigned to the label part of this element.
        ///     If this property is part of a section the width is set to the width of the text plus settings.labelEndPadding.
        ///     Otherwise, the width is set the Unity's default label width
        ///     minus the element's indent.
        /// </summary>
        public float LabelWidth
        {
            get
            {
                if ( !SingleElement.HasLabel() )
                    return 0;

                if ( SingleElement.HasParent() && !SingleElement.HasOwnLine() )
                    return SingleElement.PropertiesSettings.propertyChildLabelWidth;

                return EditorGUIUtility.labelWidth - GetTotalLeftIndentAmount();
            }
        }

        /// <summary>
        ///     True if the usable width of the assigned position rect has enough room for the required width of this element.
        /// </summary>
        public bool HasRoom =>
            SingleElement.HasParent()
                ? RequiredWidth < GetUsableWidth()
                : EditorGUIUtility.wideMode;

        public float RequiredWidth =>
            LabelWidth + SingleElement.PropertiesSettings.propertyLabelEndPadding + FieldMinWidth;


        protected SingleElementLayout( Element element ) : base( element )
        {
        }


        public override float GetElementHeight( bool updating = false ) => EditorGUIUtility.singleLineHeight;

        public override bool ShouldShowFrame()
        {
            if ( SingleElement.IsRootElement() )
                return false;

            if ( Element.ParentIsCompositeGroup() )
                return false;

            if ( Element.CustomSettings.BlockFrame() )
                return false;

            if ( !Element.ParentElement.GroupCustomSettings.ChildSingleElementsHaveFrames )
                return false;

            if ( Element.HasOwnLine() && SingleElement.SingleElementFrameSettings.skipSingleLineFrames )
                return false;

            return SingleElement.SingleElementFrameSettings.applyFraming;
        }

        public override bool ShouldApplyGlobalLeftPadding() => ShouldApplyPaddingToAnySide();
        public override bool ShouldApplyGlobalRightPadding() => ShouldApplyPaddingToAnySide();
        public override bool ShouldApplyGlobalTopPadding() => ShouldApplyPaddingToAnySide();
        public override bool ShouldApplyGlobalBottomPadding() => ShouldApplyPaddingToAnySide();

        public override bool ShouldApplyFramePadding() => ShouldApplyPaddingToAnySide();

        private bool ShouldApplyPaddingToAnySide()
        {
            if ( SingleElement.IsRootElement() )
                return false;

            if ( Element.ParentIsCompositeGroup() )
                return false;

            if ( Element.HasOwnLine() && SingleElement.SingleElementFrameSettings.skipSingleLineFrames )
                return false;

            return true;
        }


        public override void AssignNewPositionRect( bool updateRequired )
        {
            Rect newPositionRect = EditorGUILayout.GetControlRect( LabelHasContent( SingleElement.GUIContent ),
                GetRequiredHeight( false ) );
            SetPositionRect( newPositionRect, newPositionRect.width );
        }

        /// <summary>
        ///     Copied from internal source code. Detects if the guiContent has content.
        /// </summary>
        private static bool LabelHasContent( GUIContent label )
        {
            if ( label == null )
                return true;
            return label.text != string.Empty || label.image != null;
        }
    }
}