using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;

class SerializeMessage
{
    public static Message Serialize(object toSerialize)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(memoryStream, toSerialize);
        return new Message { Data = memoryStream.ToArray() };
        /*
         * using (var memoryStream = new MemoryStream())
        {
            (new BinaryFormatter()).Serialize(memoryStream, toSerialize);
            return new Message { Data = memoryStream.ToArray() };
        }
        */
    }

    public static object Deserialize(Message msg)
    {
        MemoryStream memoryStream = new MemoryStream(msg.Data);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Binder = new CurrentAssemblyDeserializationBinder();
        return formatter.Deserialize(memoryStream);

        /*
         using (var memoryStream = new MemoryStream(msg.Data))
            return (new BinaryFormatter()).Deserialize(memoryStream);
        */
    }
}

public sealed class CurrentAssemblyDeserializationBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        return Type.GetType(String.Format("{0}, {1}", typeName, Assembly.GetExecutingAssembly().FullName));
    }
}
