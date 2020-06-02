using Dna;

namespace TWETTY_CHAT.Core
{
    public static class CoreDI
    {
        /// <summary>
        /// A shortcut to access the <see cref="ITaskManager"/>
        /// </summary>
        public static ITaskManager TaskManager => Framework.Service<ITaskManager>();
    }
}
