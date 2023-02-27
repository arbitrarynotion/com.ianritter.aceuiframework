using System;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions
{
    public class ElementCondition
    {
        private readonly string _propVarName;
        private readonly ConditionOperator _conditionOperator;
        
        private readonly int _conditionValueInt;
        private readonly float _conditionValueFloat;
        

        private readonly bool _isBool;
        private readonly bool _isInt;
        // private readonly bool _isFloat;
        // private readonly bool _isEnum;
        
        private SerializedProperty _conditionProperty;

        
        private readonly Func<float, float, bool> _lessThan = ( propertyValue, conditionValue ) =>
            propertyValue < conditionValue;

        private readonly Func<float, float, bool> _lessThanOrEqualTo =
            ( propertyValue, conditionValue ) => propertyValue <= conditionValue;

        private readonly Func<int, int, bool> _equalToInt = ( propertyValue, conditionValue ) =>
            propertyValue == conditionValue;
        
        private readonly Func<int, int, bool> _notEqualToInt = ( propertyValue, conditionValue ) =>
            propertyValue != conditionValue;

        private readonly Func<float, float, bool> _equalToFloat = ( propertyValue, conditionValue ) =>
            Math.Abs( propertyValue - conditionValue ) > 0.001f;
        
        private readonly Func<float, float, bool> _notEqualToFloat = ( propertyValue, conditionValue ) =>
            Math.Abs( propertyValue - conditionValue ) < 0.001f;

        private readonly Func<float, float, bool> _greaterThanOrEqualTo =
            ( propertyValue, conditionValue ) => propertyValue >= conditionValue;

        private readonly Func<float, float, bool> _greaterThan = ( propertyValue, conditionValue ) =>
            propertyValue > conditionValue;


        public ElementCondition( string propBoolVarName )
        {
            _propVarName = propBoolVarName;
            _isBool = true;
        }
        
        public ElementCondition( string propIntVarName, ConditionOperator conditionOperator, int conditionValue )
        {
            _propVarName = propIntVarName;
            _isInt = true;
            _conditionOperator = conditionOperator;
            _conditionValueInt = conditionValue;
        }
        
        public ElementCondition( string propIntVarName, ConditionOperator conditionOperator, float conditionValue )
        {
            _propVarName = propIntVarName;
            _conditionOperator = conditionOperator;
            _conditionValueFloat = conditionValue;
        }

        public void Initialize( SerializedObject serializedObject )
        {
            _conditionProperty = serializedObject.FindProperty( _propVarName );
            if (_conditionProperty == null)
            {
                Debug.LogWarning( $"EC|I: Error! Failed to find condition property: {_propVarName};" );
            }
        }

        public bool Evaluate()
        {
            if (_conditionProperty == null)
            {
                Debug.LogWarning( "EC|E: Error! Can't evaluate condition on null condition property!" );
                return true;
            }
            
            if (_isBool)
            {
                return _conditionProperty.boolValue;
            }

            // if (_isInt || _isEnum)
            if (_isInt)
            {
                int propertyValue = _isInt ? _conditionProperty.intValue : _conditionProperty.enumValueIndex;
                return _conditionOperator switch
                {
                    ConditionOperator.LessThan => _lessThan
                        ( propertyValue, _conditionValueFloat ),
                    
                    ConditionOperator.LessThanOrEqualTo => _lessThanOrEqualTo
                        ( propertyValue, _conditionValueFloat ),
                    
                    ConditionOperator.EqualTo => _equalToInt
                        ( propertyValue, _conditionValueInt ),
                    
                    ConditionOperator.NotEqualTo => _notEqualToInt
                        ( propertyValue, _conditionValueInt ),
                    
                    ConditionOperator.GreaterThanOrEqualTo => _greaterThanOrEqualTo
                        ( propertyValue, _conditionValueFloat ),
                    
                    ConditionOperator.GreaterThan => _greaterThan
                        ( propertyValue, _conditionValueFloat ),
                    _ => false
                };
            }
            
            // if (_isFloat)
            // {
            //     float propertyValue = _conditionProperty.floatValue;
            //     return _conditionOperator switch
            //     {
            //         ConditionOperator.LessThan => _lessThan
            //             ( propertyValue, _conditionValueFloat ),
            //         
            //         ConditionOperator.LessThanOrEqualTo => _lessThanOrEqualTo
            //             ( propertyValue, _conditionValueFloat ),
            //         
            //         ConditionOperator.EqualTo => _equalToFloat
            //             ( propertyValue, _conditionValueInt ),
            //         
            //         ConditionOperator.NotEqualTo => _notEqualToFloat
            //             ( propertyValue, _conditionValueInt ),
            //         
            //         ConditionOperator.GreaterThanOrEqualTo => _greaterThanOrEqualTo
            //             ( propertyValue, _conditionValueFloat ),
            //         
            //         ConditionOperator.GreaterThan => _greaterThan
            //             ( propertyValue, _conditionValueFloat ),
            //         _ => false
            //     };
            // }

            return false;
        }
    }
}