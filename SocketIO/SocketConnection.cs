using System;
using System.Collections;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace SocketIO
{
    class SocketConnection
    {
        //Socket info connection
        private string id { get; }
        Socket connection { get; }
        DateTime connected_at { get; }

        //Callbacks table
        Hashtable eventTable;

        public delegate void eventDelegate(object o);

        public SocketConnection(Socket s)
        {
            this.connection = s;
            this.connected_at = DateTime.Now;
            this.eventTable = new Hashtable();

        }

        public void Start()
        {
            Thread t = new Thread(Listen);
            t.Start();
        }

        public int timeConnected
        {
            get { return (connected_at - DateTime.Now).Milliseconds; }
        }

        /// <summary>
        /// Set a callback to execute when a event is required
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="callback">Callback to be excecuted</param>
        public void On(string eventName, eventDelegate callback)
        {
            this.eventTable.Add(eventName, callback);
        }

        /// <summary>
        /// Emit the event specified and send the data
        /// </summary>
        /// <param name="eventName">Event to emit</param>
        /// <param name="o">Object to be sended</param>
        /// <returns>If the event was emitted</returns>
        public bool Emit(string eventName, object o)
        {
            try
            {
                Message msg = SerializeMessage.Serialize(o);
                byte[] serializedEvent = new byte[1024 + 255];
                byte[] serializedEventName = new byte[255];
                byte[] encodedEventName = Encoding.ASCII.GetBytes(eventName);
                int j = 0;
                for (int i = 0; i < encodedEventName.Length; i++)
                    serializedEvent[i] = encodedEventName[i];
                for (int i = 255; i < (255 + msg.Data.Length); i++)
                {
                    serializedEvent[i] = msg.Data[j];
                    j++;
                }
                this.connection.Send(serializedEvent);
                return true;
            }
            catch (SocketException e)
            {
                return false;
            }
        }

        /// <summary>
        /// Listen for a server request
        /// </summary>
        public void Listen()
        {
            Message msg = new Message();
            object objReceived;
            string strEvent = "";
            byte[] eventName;
            byte[] serializedEvent;
            bool connected = true;
            int objectIndex;
            try
            {
                while (connected)
                {

                    serializedEvent = new byte[1024 + 255];
                    eventName = new byte[255];
                    msg.Data = new byte[1024];
                    objectIndex = 0;

                    this.connection.Receive(serializedEvent, 0, 1024 + 255, SocketFlags.None);
                    //Getting the event name buffer
                    for (int i = 0; i < 255; i++)
                        eventName[i] = serializedEvent[i];
                    strEvent = Utils.Buffer2String(eventName);

                    //Getting the object buffer
                    for (int i = 255; i < (1024 + 255); i++)
                    {
                        msg.Data[objectIndex] = serializedEvent[i];
                        objectIndex++;
                    }
                    objReceived = SerializeMessage.Deserialize(msg);
                    this.executeEvent(strEvent, objReceived);

                }

            }
            catch (SocketException e)
            {
                this.executeEvent("disconnect", e.Message);
            }
        }

        /// <summary>
        /// Execute a event callback
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <param name="o">Object received</param>
        private void executeEvent(string eventName, object o)
        {
            string dictionaryKey;
            eventDelegate m_event;
            foreach (DictionaryEntry d in this.eventTable)
            {
                dictionaryKey = (string)d.Key;
                if (dictionaryKey == eventName)
                {
                    m_event = (eventDelegate)d.Value;
                    m_event(o);
                }
            }
        }
    }
}
