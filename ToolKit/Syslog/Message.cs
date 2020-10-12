namespace ToolKit.Syslog
{
    /// <summary>
    /// A SYSLOG message based on RFC3164.
    /// </summary>
    public class Message : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        public Message()
        {
            Facility = Facility.User;
            Level = Level.Information;
            Text = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="text">The text of the message.</param>
        public Message(string text)
        {
            Facility = Facility.User;
            Level = Level.Information;
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="facility">The facility of the message.</param>
        /// <param name="level">The level of the message.</param>
        /// <param name="text">The text of the message.</param>
        public Message(Facility facility, Level level, string text)
        {
            Facility = facility;
            Level = level;
            Text = text;
        }

        /// <summary>
        /// Gets or sets the facility of the message.
        /// </summary>
        /// <value>The facility of the message.</value>
        public Facility Facility { get; set; }

        /// <summary>
        /// Gets or sets the level of the message.
        /// </summary>
        /// <value>The level of the message.</value>
        public Level Level { get; set; }

        /// <summary>
        /// Gets or sets the text of the message.
        /// </summary>
        /// <value>The text of the message.</value>
        public string Text { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => Text;
    }
}
