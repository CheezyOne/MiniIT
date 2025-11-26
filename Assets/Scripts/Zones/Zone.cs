using UnityEngine;

namespace MiniIT.Zones
{
    public abstract class Zone : MonoBehaviour
    {
        public virtual void OnPointerDown()
        {

        }

        public virtual void OnPointerUp(Character character)
        {

        }

        public virtual bool IsAbleToTake(Character character)
        {
            return true;
        }
    }
}