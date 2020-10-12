namespace ToolKit.Syslog
{
    /// <summary>
    /// Each message Priority also has a decimal Severity level indicator.
    /// </summary>
    public enum Level
    {
        /// <summary>
        /// Emergency! System Is Unusable
        /// </summary>
        Emergency = 0,

        /// <summary>
        /// Alert! Action Must Be Taken Immediately
        /// </summary>
        Alert = 1,

        /// <summary>
        /// Critical Conditions
        /// </summary>
        Critical = 2,

        /// <summary>
        /// Error Conditions
        /// </summary>
        Error = 3,

        /// <summary>
        /// Warning Conditions
        /// </summary>
        Warning = 4,

        /// <summary>
        /// Normal But Significant Condition
        /// </summary>
        Notice = 5,

        /// <summary>
        /// Informational Messages
        /// </summary>
        Information = 6,

        /// <summary>
        /// Debug-Level Messages
        /// </summary>
        Debug = 7
    }
}
