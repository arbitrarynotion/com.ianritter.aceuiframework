using Packages.com.ianritter.aceuiframework.Runtime.Scripts;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties
{
    public abstract class PropertyElementDraw : SingleElementDraw
    {
        protected abstract PropertyElement PropertyElement { get; }
        protected override SingleElement SingleElement => PropertyElement;

        
        /// <summary>
        ///     Draws the property in the provided rect.
        /// </summary>
        protected override void DrawElementContents()
        {
            if ( !PropertyElement.IsVisible )
                return;

            if ( !PropertyIsValid() )
                return;
            
            // Handle ConvertFieldToLabel custom setting.
            if ( PropertyElement.SingleCustomSettings.ConvertFieldToLabel )
            {
                DrawAlignedLabelField( new GUIContent( GetFieldValueAsString() ), PropertyElement.PropertyElementLayout.GetDrawRect() );
                return;
            }

            PropertyElementLayout propertyElementLayout = PropertyElement.PropertyElementLayout;

            using ( new EditorGUI.DisabledScope( !PropertyElement.IsEnabled ) )
            {
                if ( !PropertyElement.HasLabel() )
                {
                    DrawNoLabelProperty( propertyElementLayout.GetDrawRect() );
                    return;
                }

                if ( PropertyElement.IsBool && propertyElementLayout.NumberOfNeighbors != 0 )
                {
                    DrawColumnBoolProperty( propertyElementLayout.GetDrawRect() );
                    return;
                }

                if ( PropertyElement.IsRootElement() || ( PropertyElement.HasOwnLine() && propertyElementLayout.HasRoom ) )
                {
                    DrawPropertyFieldRespectSeparator();
                    return;
                }

                if ( PropertyElement.PropertyTypeShouldUseCustomWideMode() && !propertyElementLayout.HasRoom )
                {
                    DrawHeightAdjustingPropertyField();
                    if ( ShouldShowWideModeBoxes() )
                        DrawWidthMeasurementLinesForColumn();
                    return;
                }

                DrawPropertyFieldWithMinSpacing();
            }

            if ( ShouldShowWideModeBoxes() )
                DrawWidthMeasurementLinesForColumn();
        }
        
        protected virtual void DrawPropertyFieldWithOutLabel( Rect fieldRect )
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField( fieldRect, PropertyElement.Property, GUIContent.none );
            if ( EditorGUI.EndChangeCheck() )
                PropertyElement.ChangeCallBack?.Invoke();
        }

        protected virtual void DrawPropertyFieldWithLabel( Rect drawRect )
        {
            if ( !PropertyElement.PropertyTypeShouldUseCustomWideMode() )
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField( drawRect, PropertyElement.Property, PropertyElement.GUIContent );
                if ( EditorGUI.EndChangeCheck() )
                    PropertyElement.ChangeCallBack?.Invoke();

                return;
            }

            // Draw label side of property manually to correct issue of labels with text longer than their
            // allocated space writing beyond that space.
            // Bug: Manually taking over the label results in losing the ability to perform controls via the label like with number fields.
            var labelRect = new Rect( drawRect )
            {
                // width = EditorGUIUtility.labelWidth - Element.PropertySettings.propertyLabelEndPadding,
                width = EditorGUIUtility.labelWidth - PropertyElement.PropertyElementLayout.LabelEndPadding,
                height = EditorGUIUtility.singleLineHeight
            };
            DrawLabelField( labelRect );

            // Todo: Allow constant field width in custom settings? Would be complicated as it would affect the already complex label width logic.
            drawRect.xMin += EditorGUIUtility.labelWidth;
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField( drawRect, PropertyElement.Property, GUIContent.none );
            if ( EditorGUI.EndChangeCheck() )
                PropertyElement.ChangeCallBack?.Invoke();
        }


        private string GetFieldValueAsString()
        {
            SerializedProperty property = PropertyElement.Property;
            return property.propertyType switch
            {
                SerializedPropertyType.Boolean => property.boolValue ? "True" : "False",
                SerializedPropertyType.Integer => property.intValue.ToString("N0"),
                SerializedPropertyType.Float => property.floatValue.ToString( "N" ),
                SerializedPropertyType.Vector2 => $"{property.vector2Value.x.ToString( "F" )}, {property.vector2Value.y.ToString( "F" )}",
                _=> "Type Not Supported"
            };
        }
        
        

        private bool PropertyIsValid()
        {
            if ( SingleElement.ElementIsValid() ) return true;

            Debug.LogWarning( $"SE|DE: Error!! \"{Element.GetName()}\"'s property is null!" );
            DrawSolidRect( Element.Layout.GetDrawRect(), new Color( 0.5f, 0.5f, 0.1f ) );
            EditorGUI.LabelField( Element.Layout.GetDrawRect(), "=(" );
            return false;
        }

        private bool ShouldShowWideModeBoxes() =>
            PropertyElement.PropertiesSettings.showWideModeBoxes
            && PropertyElement.PropertyElementLayout.ShouldApplyVariableHeight
            && PropertyElement.PropertyTypeShouldUseCustomWideMode();

        /// <summary>
        ///     Draws a bool property where the space allocated to the bool checkbox is minimized, leaving the rest of
        ///     the space to the label.
        /// </summary>
        private void DrawColumnBoolProperty( Rect drawRect )
        {
            const float boolBoxWidth = 15;
            float labelWidth = drawRect.width - boolBoxWidth;
            
            var labelDrawRect = new Rect( drawRect );
            labelDrawRect.width -= PropertyElement.PropertyElementLayout.LabelEndPadding;
            // labelDrawRect.width -= Element.PropertySettings.propertyLabelEndPadding;
            DrawAlignedLabelField( labelDrawRect );

            var fieldRect = new Rect( drawRect );
            fieldRect.xMin += labelWidth;
            DrawNoLabelProperty( fieldRect );
        }

        private void DrawNoLabelProperty( Rect fieldRect )
        {
            // Draw property with negated label width, since we drew it manually.
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 0.01f; // Can't be zero or it will default back to normal.
            DrawPropertyFieldWithOutLabel( fieldRect );
            EditorGUIUtility.labelWidth = labelWidth;
        }

        /// <summary>
        ///     Draw a property field with the height adjusted to fit a shorter position width.
        /// </summary>
        private void DrawHeightAdjustingPropertyField()
        {
            var labelRect = new Rect( PropertyElement.PropertyElementLayout.GetDrawRect() )
            {
                height = EditorGUIUtility.singleLineHeight
            };
            DrawLabelField( labelRect );

            var fieldRect = new Rect( PropertyElement.PropertyElementLayout.GetDrawRect() )
            {
                height = EditorGUIUtility.singleLineHeight
            };
            fieldRect.xMin += 15f;
            fieldRect.y += EditorGUIUtility.singleLineHeight + 2f;

            // Draw property with negated label width, since we drew it manually.
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 0.01f; // Can't be zero or it will default back to normal.
            DrawPropertyFieldWithOutLabel( fieldRect );
            EditorGUIUtility.labelWidth = labelWidth;
        }

        /// <summary>
        ///     Draw a property field providing only the minimum required space for the label part of the element.
        /// </summary>
        private void DrawPropertyFieldWithMinSpacing()
        {
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = PropertyElement.PropertiesSettings.propertyChildLabelWidth + PropertyElement.PropertyElementLayout.LabelEndPadding;
                                          // PropertyElement.PropertiesSettings.propertyLabelEndPadding;
            DrawPropertyFieldWithLabel( PropertyElement.PropertyElementLayout.GetDrawRect() );
            EditorGUIUtility.labelWidth = labelWidth;
        }

        /// <summary>
        ///     Draws the inspector element ensuring that its field lines up with Unity's default label/field separator.
        /// </summary>
        private void DrawPropertyFieldRespectSeparator()
        {
            float labelWidth = EditorGUIUtility.labelWidth;
            float indentedLabelWidth = PropertyElement.PropertyElementLayout.LabelWidth - 3f;
            EditorGUIUtility.labelWidth = indentedLabelWidth;
            DrawPropertyFieldWithLabel( PropertyElement.PropertyElementLayout.GetDrawRect() );
            EditorGUIUtility.labelWidth = labelWidth;
        }

        

        /// <summary>
        ///     Draws colored lines in the provided rect designating the total width, required width, and wideMode width (if
        ///     selected).
        /// </summary>
        private void DrawWidthMeasurementLinesForColumn()
        {
            // Set colors used to visualize the wide mode areas.
            const float alpha = 0.25f;
            var usableAreaColor = new Color( 0, .3f, 0, alpha );
            var requiredAreaLabelColor = new Color( 0.2f, 0.6f, 0.9f, alpha );
            var requiredAreaFieldColor = new Color( 1, 0.92f, 0.016f, alpha );
            var wideModeAreaColor = new Color( 1, 0, 0, alpha );
            var dividerColor = new Color( 0.9f, 0.6f, 0.05f, alpha );

            Color statusColor = Color.green;
            if ( !PropertyElement.PropertyElementLayout.HasRoom )
                statusColor = Color.yellow;


            var drawRect = new Rect( PropertyElement.Layout.GetDrawRect() );

            // Visualize the wide mode width.
            const float defaultWideModeWidth = 330f;
            var wideModeRect = new Rect
            {
                x = EditorGUIUtility.currentViewWidth - defaultWideModeWidth,
                width = defaultWideModeWidth
            };
            DrawRectOutline( wideModeRect, wideModeAreaColor );


            // Draw a colored outline of the draw rect to show when wide mode changes.
            DrawRectOutline( drawRect, statusColor );

            // If the draw rect width doesn't match the usable width, draw the area defined by the usable width. This
            // helps highlight when there may be an issue. However, note that this is expected when width truncation is applied
            // due to the presence of the scrollbar.
            float usableWidth = PropertyElement.PropertyElementLayout.GetUsableWidth();
            if ( drawRect.width < usableWidth )
            {
                var usableWidthOutline = new Rect( drawRect )
                {
                    width = usableWidth
                };
                DrawRectOutline( usableWidthOutline, Color.magenta );
            }

            // Color the draw rect to highlight it as the usable area.
            DrawSolidRect( drawRect, usableAreaColor );

            // Draw the area required to show the label, and field.
            var requiredArea = new Rect( drawRect ) { width = PropertyElement.PropertyElementLayout.RequiredWidth };
            requiredArea.xMax = Mathf.Min( requiredArea.xMax, drawRect.xMax );
            DrawSolidRect( requiredArea, requiredAreaFieldColor );

            // Highlight the label part of the required area.
            var labelRect = new Rect( drawRect )
            {
                width = PropertyElement.PropertyElementLayout.LabelWidth
            };
            labelRect.xMax = Mathf.Min( labelRect.xMax, drawRect.xMax );
            DrawSolidRect( labelRect, requiredAreaLabelColor );

            // Highlight the label end padding used in the required area.
            var labelEndPadding = new Rect( labelRect );
            labelEndPadding.x += labelRect.width;
            labelEndPadding.width = PropertyElement.PropertyElementLayout.LabelEndPadding;
            // labelEndPadding.width = PropertyElement.PropertiesSettings.propertyLabelEndPadding;
            labelEndPadding.xMax.AtMost( drawRect.xMax );
            if ( PropertyElement.PropertyElementLayout.LabelWidth < usableWidth )
                DrawSolidRect( labelEndPadding, dividerColor );

            // Highlight the field part of the required area.
            var fieldRect = new Rect( labelEndPadding );
            fieldRect.x += labelEndPadding.width;
            fieldRect.width = PropertyElement.PropertyElementLayout.FieldMinWidth;
            labelEndPadding.xMax = Mathf.Min( labelEndPadding.xMax, drawRect.xMax );
        }
    }
}