using System;

namespace MiniIT
{
    public static class EventBus
    {
        public static Action onSavesLoaded;

        public static Action onMoneyChanged;

        public static Action onCharacterTaken;
        public static Action onCharacterDropped;
    }
}