namespace CommonLibrary.MessageBroker
{
    /// <summary>
    /// Event args for passing data when raising event in MessageBroker
    /// </summary>
    public class MessageBrokerEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        /// Get/Set a Unique Message Name.
        /// This property will help the receiver of the message know what is in the MessagePayload property.
        /// </summary>
        public string MessageName { get; set; } = string.Empty;

        /// <summary>
        /// Get/Set the payload for the message
        /// </summary>
        public object? MessagePayload { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for MessageBrokerEventArgs class
        /// </summary>
        public MessageBrokerEventArgs() : base()
        {
        }

        /// <summary>
        /// Constructor for MessageBrokerEventArgs class
        /// </summary>
        /// <param name="messageName">A Message Name</param>
        /// <param name="payload">The Payload for the Message</param>
        public MessageBrokerEventArgs(string messageName, object? payload) : base()
        {
            MessageName = messageName;
            MessagePayload = payload;
        }

        #endregion
    }
}
