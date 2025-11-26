using UnityEngine;

namespace MiniIT.InputSystem
{ 
    public class DesktopInputService : IInputService
    {
        #region IInputService
        public Vector2 GetDragPosition()
        {
            return Input.mousePosition;
        }
        #endregion
    }
}