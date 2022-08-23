namespace CommonLibrary.MessageBroker
{
    /// <summary>
    /// This class is responsible for receiving and sending messages to classes registered to receive those messages.
    /// In any application, you want to keep the coupling between any two or more objects as loose as possible.
    /// One popular design pattern to help with keeping objects loosely coupled is called the Mediator design pattern.
    /// The basics of this pattern are very simple; avoid one object directly talking to another object,
    /// and instead use another class to mediate between the two.
    /// Visit: https://weblogs.asp.net/psheriff/a-communication-system-for-xaml-applications
    /// </summary>
    public class MessageBroker
  {
    #region Delegate for MessageReceivedEventHandler
    /// <summary>
    /// The delegate declaration of the MessageReceived Event Handler
    /// </summary>
    /// <param name="sender">The object raising the event</param>
    /// <param name="e">A MessageBrokerEventArgs object that contains the message</param>
    public delegate void MessageReceivedEventHandler(object sender, MessageBrokerEventArgs e);

    /// <summary>
    /// Define the MessageReceived Event
    /// </summary>
    public event MessageReceivedEventHandler? MessageReceived;
    #endregion

    #region Instance Property
    private static MessageBroker? _instance;

    public static MessageBroker Instance
    {
      get { return _instance ??= new MessageBroker(); }
      set => _instance = value;
    }
    #endregion

    #region SendMessage Methods
    /// <summary>
    /// Call this method to send a message to any other objects that are asking to receive messages
    /// A null is passed for the message payload
    /// </summary>
    /// <param name="messageName">A message name</param>
    public void SendMessage(string messageName)
    {
      SendMessage(messageName, null);
    }

    /// <summary>
    /// Call this method to send a message to any other objects that are asking to receive messages
    /// </summary>
    /// <param name="messageName">A message name</param>
    /// <param name="payload">The payload to send with the message</param>
    public void SendMessage(string messageName, object? payload)
    {
        var arg = new MessageBrokerEventArgs(messageName, payload);

      RaiseMessageReceived(arg);
    }
    #endregion

    #region MessageReceived Event
    /// <summary>
    /// Raise the Message Received Event
    /// </summary>
    /// <param name="e">A MessageBrokerEventArgs object</param>
    protected void RaiseMessageReceived(MessageBrokerEventArgs e)
    {
      if (null != MessageReceived) {
        MessageReceived(this, e);
      }
    }
    #endregion
  }
}
