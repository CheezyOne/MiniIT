using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using MiniIT.Zones;
using MiniIT;

namespace MiniIT.InputSystem
{
    public class InputPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private Camera mainCamera = null;

        private Zone latestZone = null;
        private Character draggedCharacter = null;
        private IInputService inputService = null;

        public Character DraggedCharacter => draggedCharacter;

        [Inject]
        public void Construct(IInputService inputService)
        {
            this.inputService = inputService;
        }

        public void SetDraggedCharacter(Character character)
        {
            draggedCharacter = character;
            draggedCharacter.transform.SetParent(transform);
            EventBus.onCharacterTaken?.Invoke();
        }

        #region IPointerDownHandler
        public void OnPointerDown(PointerEventData eventData)
        {
            Zone zone = FindZoneUnderPointer(eventData);

            if (zone != null)
            {
                latestZone = zone;
                zone.OnPointerDown();
            }
        }
        #endregion

        #region IPointerUpHandler
        public void OnPointerUp(PointerEventData eventData)
        {
            if (draggedCharacter != null)
            {
                Zone zone = FindZoneUnderPointer(eventData);

                if (zone != null && zone.IsAbleToTake(draggedCharacter))
                {
                    zone.OnPointerUp(draggedCharacter);
                }
                else
                {
                    latestZone.OnPointerUp(draggedCharacter);
                }

                latestZone = null;
                draggedCharacter = null;
                EventBus.onCharacterDropped?.Invoke();
            }
        }
        #endregion

        #region IDragHandler
        public void OnDrag(PointerEventData eventData)
        {
            if (draggedCharacter != null)
            {
                MoveCharacter();
            }
        }
        #endregion

        private Zone FindZoneUnderPointer(PointerEventData eventData)
        {
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (RaycastResult result in results)
            {
                Zone zone = result.gameObject.GetComponent<Zone>();

                if (zone != null)
                {
                    return zone;
                }
            }

            return null;
        }

        private void MoveCharacter()
        {
            Vector2 targetPosition = inputService.GetDragPosition();
            Vector3 worldPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                draggedCharacter.RectTransform,
                targetPosition,
                mainCamera,
                out worldPosition
            );
            draggedCharacter.RectTransform.position = worldPosition;
        }
    }
}