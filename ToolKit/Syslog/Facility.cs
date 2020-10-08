namespace ToolKit.Syslog
{
    /// <summary>
    /// The Facilities of the messages are numerically coded with decimal values. Some of the
    /// operating system daemons and processes have been assigned Facility values. Processes and
    /// daemons that have not been explicitly assigned a Facility may use any of the "local use"
    /// facilities or they may use the "user-level" Facility.
    /// </summary>
    public enum Facility
    {
        /// <summary>
        /// Kernel Messages
        /// </summary>
        Kernel = 0,

        /// <summary>
        /// User Level Messages
        /// </summary>
        User = 1,

        /// <summary>
        /// Mail System
        /// </summary>
        Mail = 2,

        /// <summary>
        /// System Daemons
        /// </summary>
        Daemon = 3,

        /// <summary>
        /// Security/Authorization Messages
        /// </summary>
        Auth = 4,

        /// <summary>
        /// Messages generated internally by SYSLOGD
        /// </summary>
        Syslog = 5,

        /// <summary>
        /// Line Printer Subsystem
        /// </summary>
        Lpr = 6,

        /// <summary>
        /// Network News Subsystem
        /// </summary>
        News = 7,

        /// <summary>
        /// UUCP Subsystem
        /// </summary>
        UUCP = 8,

        /// <summary>
        /// Clock Daemon
        /// </summary>
        Cron = 9,

        /// <summary>
        /// System Use 0
        /// </summary>
        System0 = 10,

        /// <summary>
        /// System Use 1
        /// </summary>
        System1 = 11,

        /// <summary>
        /// System Use 2
        /// </summary>
        System2 = 12,

        /// <summary>
        /// System Use 3
        /// </summary>
        System3 = 13,

        /// <summary>
        /// System Use 4
        /// </summary>
        System4 = 14,

        /// <summary>
        /// System Use 5
        /// </summary>
        System5 = 15,

        /// <summary>
        /// Local Use 0
        /// </summary>
        Local0 = 16,

        /// <summary>
        /// Local Use 1
        /// </summary>
        Local1 = 17,

        /// <summary>
        /// Local Use 2
        /// </summary>
        Local2 = 18,

        /// <summary>
        /// Local Use 3
        /// </summary>
        Local3 = 19,

        /// <summary>
        /// Local Use 4
        /// </summary>
        Local4 = 20,

        /// <summary>
        /// Local Use 5
        /// </summary>
        Local5 = 21,

        /// <summary>
        /// Local Use 6
        /// </summary>
        Local6 = 22,

        /// <summary>
        /// Local Use 7
        /// </summary>
        Local7 = 23,
    }
}
