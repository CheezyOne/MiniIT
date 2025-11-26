using UnityEngine;

namespace MiniIT.InputSystem
{
    public class MobileInputService : IInputService
    {
        private MobileInputConfig inputConfig = null;

        public MobileInputService(MobileInputConfig inputConfig)
        {
            this.inputConfig = inputConfig;
        }

        #region IInputService
        public Vector2 GetDragPosition()
        {
            if (Input.touchCount > 0)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                return touchPosition + (Vector2)inputConfig.MobileDragOffset;
            }

            return Vector2.zero;
        }
        #endregion
    }
}