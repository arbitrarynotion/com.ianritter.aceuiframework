using UnityEngine;

namespace ACEPackage.Editor.Scripts.ACECore
{
    public class AceDelegates : MonoBehaviour
    {
        public delegate void DataUpdated();
        public delegate void UIStateUpdated();
        public delegate void DataUpdateRequired();
        public delegate void ColorsUpdated();
    }
}
