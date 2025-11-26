using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MiniIT.Windows
{
    public class WindowsManager : Singleton<WindowsManager>
    {
        [SerializeField] private Transform windowsCanvas = null;

        [Inject] private DiContainer container = null;

        private List<BaseWindow> openedWindows = new();

        public void OpenWindow(BaseWindow window)
        {
            if (IsOpened(window.GetType()))
            {
                CloseWindow(window.GetType());
                return;
            }

            if (openedWindows.Count != 0)
            {
                return;
            }

            BaseWindow newWindow = Instantiate(window, windowsCanvas);
            container.InjectGameObject(newWindow.gameObject);
            newWindow.Init();
            openedWindows.Add(newWindow);
        }

        public T OpenWindow<T>(T window) where T : BaseWindow
        {
            if (IsOpened(window.GetType()))
            {
                CloseWindow(window.GetType());
                return null;
            }

            if (openedWindows.Count != 0)
            {
                return null;
            }

            T newWindow = Instantiate(window, windowsCanvas);
            container.InjectGameObject(newWindow.gameObject);
            newWindow.Init();
            openedWindows.Add(newWindow);
            return newWindow;
        }

        public void CloseWindow(Type type)
        {
            var window = openedWindows.FirstOrDefault(x => x.GetType() == type);

            if (window == null)
            {
                return;
            }

            openedWindows.Remove(window);
            window.OnClose();
            Destroy(window.gameObject);
        }

        public bool IsOpened(Type type)
        {
            return openedWindows.Any(x => x.GetType() == type);
        }
    }
}