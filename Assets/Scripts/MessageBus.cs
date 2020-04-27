using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace Engine7
{
    //public class MessageType

    /// <summary>
    /// A class of basic message types, you can add to this by creating your own class and inheriting MessageType
    /// you can then add a new member with the following code
    /// <code>public static readonly MessageType MySuperDuperMessageType = new MessageType();</code>
    /// </summary>
    public class MessageType
    {
        /// <summary>
        /// name displayed during debugging
        /// </summary>
        public string Name = "NOT SET";
        /// <summary>
        /// creates a new message type and sets a debugging title
        /// </summary>
        /// <param name="name">The name to display when debugging is active for messages</param>
        public MessageType(string name) { this.Name = name; }
        /// <summary>
        /// creates a new message without a debug title
        /// </summary>
        public MessageType() { }
        /// <summary>
        /// no message ??
        /// </summary>
        public static readonly MessageType NONE = new MessageType("NONE");
        /// <summary>
        /// Level has started (you can use this to mean anything)
        /// </summary>
        public static readonly MessageType LevelStart = new MessageType("LevelStart");
        /// <summary>
        /// Level has ended
        /// </summary>
        public static readonly MessageType LevelEnd = new MessageType("LevelEnd");
        /// <summary>
        /// player position is being broadcast
        /// </summary>
        public static readonly MessageType PlayerPosition = new MessageType("PlayerPosition");
        /// <summary>
        /// Timer information is available
        /// </summary>
        public static readonly MessageType Timer = new MessageType("Timer");
        /// <summary>
        /// reached a goal
        /// </summary>
        public static readonly MessageType Goal = new MessageType("Goal");
    }


    /// <summary>
    /// defines a message to be sent and consumed
    /// </summary>
    public struct Message
    {
        /// <summary>
        /// the type of message being sent
        /// </summary>
        public MessageType Type;
        /// <summary>
        /// an integer value
        /// </summary>
        public int IntValue;
        /// <summary>
        /// a real number value
        /// </summary>
        public float FloatValue;
        /// <summary>
        /// a vector 3 value
        /// </summary>
        public Vector3 Vector3Value;
        /// <summary>
        /// a vector 2 value
        /// </summary>
        public Vector2 Vector2Value;
        /// <summary>
        /// any object can be placed inside this the handler needs to be aware of what to cast the object to to access the message data
        /// </summary>
        public System.Object ObjectValue;
        /// <summary>
        /// a string value
        /// </summary>
        public string StringValue;
    }

    /// <summary>
    /// defines a method that will accept a message that on object has subscribed to
    /// </summary>
    /// <param name="message">the message information that was subscribed to</param>
    public delegate void MessageHandler(Message message);

    /// <summary>
    /// holds message subscription information
    /// </summary>
    public struct MessageSubscriber
    {
        /// <summary>
        /// a list of message types that this subscriber wishes to listen out for
        /// </summary>
        public List<MessageType> MessageTypes;
        /// <summary>
        /// the method that will be invoked (called) when one of the messagetypes stated is broadcast
        /// </summary>
        public MessageHandler Handler;
    }

   
    /// <summary>
    /// broadcasting system for message
    /// </summary>
    public class MessageBus
    {
        /// <summary>
        /// holds the singleton instance of the messagebus object
        /// </summary>
        static MessageBus instance;

        /// <summary>
        /// use MessageBus.Instance to access the methods of MessageBus
        /// </summary>
        public static MessageBus Instance
        {
            get
            {
                // if first time use then create an instance of this object
                //otherwise return the previously created instance
                if (instance == null)
                    instance = new MessageBus();

                return instance;
            }
        }

        /// <summary>
        /// private constructor as we have created a singleton
        /// </summary>
        private MessageBus() { }

        /// <summary>
        /// holds the lists of subscribers
        /// </summary>
        Dictionary<MessageType, MessageHandler> subscriberLists =
                            new Dictionary<MessageType, MessageHandler>();
        
        /// <summary>
        /// removes all message subscribers
        /// </summary>
        public void Unsubscribe()
        {
            subscriberLists.Clear();
        }

        /// <summary>
        /// remove any subscriptions for a particular messagetype
        /// </summary>
        /// <param name="messageType">type of message to remove subscribers of</param>
        public void Unsubscribe(MessageType messageType)
        {
            if (subscriberLists.ContainsKey(messageType))
                subscriberLists.Remove(messageType);
        }

        ///// <summary>
        ///// a dummy handler to assign
        ///// </summary>
        //MessageHandler deadHandler = null;

        /// <summary>
        /// Unsubscribe a handler from a message type
        /// </summary>
        /// <param name="messageType">messagetype to drop subscription from</param>
        /// <param name="handler">handler to remove from listening list</param>
        public void Unsubscribe(MessageType messageType, MessageHandler handler)
        {
            //if messagetype has subscriptions attempt to remove handler from multicast delegate
            if (subscriberLists.ContainsKey(messageType))
            {
                /*
                if (subscriberLists[messageType].GetInvocationList().Length > 1)
                {
                    MessageHandler handlersToKeep = null;
                    //handlersToKeep += handler;
                    //create a new list excluding this handler
                    
                    for (int i = 0; i < subscriberLists[messageType].GetInvocationList().Length; i++)
                    {
                        if (subscriberLists[messageType].GetInvocationList()[i].Method != handler.Method)
                        {
                            handlersToKeep += (MessageHandler)subscriberLists[messageType].GetInvocationList()[i];
                            
                        }
                    }
                    subscriberLists.Remove(messageType);
                    if (handlersToKeep != null)
                        subscriberLists.Add(messageType, handlersToKeep);

                }
                else
                    ClearMessageTypeSubscriptions(messageType);
                //Delegate.Remove(subscriberLists[messageType], handler);
                 * */
                subscriberLists[messageType] -= handler;

                ////remove key if no delegates left
                //bool remove = false;
                //for (int i = 0; i < subscriberLists[messageType].GetInvocationList().Length; i++)
                //{
                //    remove |= subscriberLists[messageType].GetInvocationList()[i] == deadHandler;
                //}
                if (subscriberLists[messageType] == null)
                    subscriberLists.Remove(messageType);
            }
        }
        /// <summary>
        /// unsubscribe a messagesubscriber from it's defined subscriptions
        /// </summary>
        /// <param name="subscriber">The message subscription information to remove</param>
        public void Unsubscribe(MessageSubscriber subscriber)
        {
            //run through each messagetype
            for (int i = 0; i < subscriber.MessageTypes.Count; i++)
                Unsubscribe(subscriber.MessageTypes[i], subscriber.Handler);
        }

        /// <summary>
        /// Setup a subscription to multiple messages using a messageSubscriber object
        /// </summary>
        /// <param name="subscriber">the message subscription information</param>
        public void Subscribe(MessageSubscriber subscriber)
        {
            
            //add the individual subscriptions
            for (int i = 0; i < subscriber.MessageTypes.Count; i++)
                Subscribe(subscriber.MessageTypes[i], subscriber.Handler);
                
        }
        /// <summary>
        /// setup a subscription to a single messagetype
        /// </summary>
        /// <param name="messageType">the messagetype to subscribe to</param>
        /// <param name="handler">the method which will process the message</param>
        public void Subscribe(MessageType messageType, MessageHandler handler)
        {
            //build a new subscriber list if this is the only attempt to subscribe to 
            //this specific messagetype
            if (!subscriberLists.ContainsKey(messageType))
                subscriberLists.Add(messageType, handler);
            else
                subscriberLists[messageType] += handler;
        }

        /// <summary>
        /// allow any object to send a message to anyone who's subscribed to this message type
        /// MessageBus.Instance.SendMessage(message)
        /// </summary>
        /// <param name="message">Message data to broadcast</param>
        public void BroadcastMessage(Message message)
        {
            //if messagetype has not been subscribed to don't bother
            //with the broadcast
            if (!subscriberLists.ContainsKey(message.Type))
                return;
            
            //trigger invoking of the multicast delegate
            subscriberLists[message.Type](message);

        }
        /// <summary>
        /// quick and easy broadcast a message of a particular type with an object value
        /// </summary>
        /// <param name="type">Type of message to broadcast</param>
        /// <param name="data">data to transmit - subscribers will need to cast this to the correct type in order to access it</param>
        public void BroadcastMessage(MessageType type, System.Object data)
        {
            Message m = new Message();
            m.Type = type;
            m.ObjectValue = data;
            BroadcastMessage(m);
        }
        /// <summary>
        /// Broadcasts a message with no content
        /// </summary>
        /// <param name="type">The type of message to broadcast</param>
        public void BroadcastMessage(MessageType type)
        {
            BroadcastMessage(type, null);
        }

        /// <summary>
        /// provides a text formatted list of all subscriptions with your delimeter as a separator between each
        /// </summary>
        /// <param name="delimeter">string to place between each subscription (use ~ for new line in my engine)</param>
        /// <returns>formatted subscriptions</returns>
        public string Subscriptions(string delimeter)
        {
            StringBuilder text = new StringBuilder();
            foreach (var item in subscriberLists)
            {
                text.Append(item.Key.Name);
                text.Append("-");
                Delegate[] d = item.Value.GetInvocationList();
                foreach (Delegate thing in d)
                {
                    text.Append(thing.Method.Name + "()");
                    text.Append(" "); 
                }
                text.Append(delimeter);
            }
            return text.ToString();
        }
        /// <summary>
        /// provides a text formatted list of a specific message type, with your delimeter as a separator between each
        /// </summary>
        /// <param name="type">type of message to retrieve subscriptions for</param>
        /// <param name="delimeter">string to place between each subscription (use ~ for new line in my engine)</param>
        /// <returns>formatted subscriptions</returns>
        public string Subscriptions(MessageType type)
        {
            StringBuilder text = new StringBuilder();
            foreach (var item in subscriberLists)
            {
                if (item.Key == type)
                {
                    text.Append(item.Key.Name);
                    text.Append("-");
                    Delegate[] d = item.Value.GetInvocationList();
                    foreach (Delegate thing in d)
                    {
                        text.Append(thing.Method.Name + "()");
                        text.Append(" ");
                    }
                }
            }
            return text.ToString();
        }

    }
}
