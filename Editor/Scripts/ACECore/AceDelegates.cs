using System;
using System.Reflection;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Editors;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore
{
    public class AceDelegates : MonoBehaviour
    {
        public delegate void DataUpdated();
        public delegate void UIStateUpdated();
        public delegate void DataUpdateRequired();
        public delegate void ColorsUpdated();
        
        public static void PrintMySubscribers( string callingType, Delegate subscribedEvent, string eventName )
        {
            if ( subscribedEvent == null ) return;

            Debug.Log( $"ATMD|PS: {callingType}:{eventName} subscribers:" );
            // Get list of subscribers.
            foreach ( Delegate @delegate in subscribedEvent.GetInvocationList() )
            {
                // Get list of attributes.
                foreach ( Attribute attribute in @delegate.Target.GetType().GetCustomAttributes() )
                {
                    // If the Custom Editor attribute is found, this is an editor script.
                    if ( attribute.GetType() != typeof( CustomEditor ) ) continue;
                    
                    // Todo: At least for now, all editors will be monobehaviour roots. I'll need to update this if I include the scriptable object root.
                    var monobehaviourRoot = (AceMonobehaviourRootEditor) @delegate.Target;
                    Debug.Log( $"    {monobehaviourRoot.GetTargetName()}" );
                }
            }
        }
        
        
        public static void PrintEventSubscribers( string callingType, Delegate subscribedEvent )
        {
            if ( subscribedEvent == null ) return;

            Debug.Log( $"ATMD|PS: {callingType}'s subscribers:" );
            // Get list of subscribers.
            foreach ( Delegate @delegate in subscribedEvent.GetInvocationList() )
            {
                // Get list of attributes.
                foreach ( Attribute attribute in @delegate.Target.GetType().GetCustomAttributes() )
                {
                    // If the Custom Editor attribute is found, this is an editor script.
                    if ( attribute.GetType() != typeof( CustomEditor ) ) continue;
                    
                    // Todo: At least for now, all editors will be monobehaviour roots. I'll need to update this if I include the scriptable object root.
                    var monobehaviourRoot = (AceMonobehaviourRootEditor) @delegate.Target;
                    Debug.Log( $"    {monobehaviourRoot.GetTargetName()}" );
                }
            }
        }
        
        // Keeping this code for now since it digs further into the event subscribers. Will come in handy with non-editor classes.
        // private void PrintMySubscribers()
        // {
        //     if ( OnThemeAssignmentChanged == null ) return;
        //
        //     Debug.Log( $"ATMD|TACN: ThemeAssignmentChangedNotify subscribers:" );
        //     foreach ( Delegate @delegate in OnThemeAssignmentChanged.GetInvocationList() )
        //     {
        //         // Debug.Log( $"    {@delegate.Method.Name}" );
        //         IEnumerable<Attribute> attributes = @delegate.Target.GetType().GetCustomAttributes();
        //         // Debug.Log( $"    {@delegate.Target.GetType().Name}'s attributes:" );
        //         foreach ( Attribute attribute in attributes )
        //         {
        //             // Debug.Log( $"        {attribute.GetType().Name}" );
        //             if ( attribute.GetType() == typeof( CustomEditor ) )
        //             {
        //                 // var customEditorInspectedType = (Type) GetPropertyValue( attribute, "m_InspectedType" );
        //                 
        //                 Type customEditorType = attribute.GetType();
        //                 
        //                 MemberInfo[] customEditorMembers = customEditorType.GetMembers( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
        //
        //                 // Debug.Log( $"        Custom Editor Members:" );
        //                 foreach ( MemberInfo customEditorMember in customEditorMembers )
        //                 {
        //                     // Debug.Log( $"            {customEditorMember.Name}" );
        //                     if ( customEditorMember.Name.Equals( "m_InspectedType" ) )
        //                     {
        //                         // Debug.Log( $"            Found custom editor's inspected type property!" );
        //                         object inspectedType = ( (FieldInfo) customEditorMember ).GetValue( attribute );
        //                         if ( inspectedType != null )
        //                         {
        //                             var type = (Type) inspectedType;
        //                             var monobehaviourRoot = (AceMonobehaviourRootEditor) @delegate.Target;
        //                             
        //                             // Debug.Log( $"            Inspected object value is {type}" );
        //                             // Debug.Log( $"                Base class is {monobehaviourRoot.GetTargetName()}" );
        //                             
        //                             Debug.Log( $"    {monobehaviourRoot.GetTargetName()}" );
        //
        //                         }
        //                     }
        //                 }
        //                 return;
        //             }
        //         }
        //     }
        // }
    }
}
