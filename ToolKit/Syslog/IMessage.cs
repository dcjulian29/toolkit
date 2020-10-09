namespace ToolKit.Syslog
{
    /// <summary>
    /// Interface for A SYSLOG message based on RFC3164.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets or sets the facility of the message.
        /// </summary>
        Facility Facility { get; set; }

        /// <summary>
        /// Gets or sets the level of the message.
        /// </summary>
        Level Level { get; set; }

        /// <summary>
        /// Gets or sets the text of the message.
        /// </summary>
        string Text { get; set; }
    }
}
