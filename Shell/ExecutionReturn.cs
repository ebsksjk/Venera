namespace Venera.Shell
{
    public enum ExecutionReturn
    {
        /// <summary>
        /// User entered an empty command (just pressed enter).
        /// </summary>
        Empty,

        /// <summary>
        /// The executed built-in or program finished happy.
        /// </summary>
        Success,

        /// <summary>
        /// The executed built-in or program finished angry.
        /// </summary>
        Failure,

        /// <summary>
        /// User wants to exit the shell ("exit" command entered).
        /// </summary>
        Exit,

        /// <summary>
        /// The entered command cannot be found (not found in built-in and path).
        /// </summary>
        NotFound
    }
}
