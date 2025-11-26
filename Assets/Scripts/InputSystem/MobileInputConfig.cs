using UnityEngine;

namespace MiniIT.InputSystem
{
    [CreateAssetMenu(menuName = "Game/Mobile Input Config")]

    public class MobileInputConfig : ScriptableObject
    {
        [SerializeField] private Vector3 mobileDragOffset = Vector3.zero;
        public Vector3 MobileDragOffset => mobileDragOffset;
    }
}