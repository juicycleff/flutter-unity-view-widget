using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageHandler
{
    public int id;
    public string seq;

    public String name;
    private JToken data;

    public static MessageHandler Deserialize(string message)
    {
        JObject m = JObject.Parse(message);
        MessageHandler handler = new MessageHandler(
            m.GetValue("id").Value<int>(),
            m.GetValue("seq").Value<string>(),
            m.GetValue("name").Value<string>(),
            m.GetValue("data")
        );
        return handler;
    }

    public T getData<T>()
    {
        return data.Value<T>();
    }

    public MessageHandler(int id, string seq, string name, JToken data)
    {
        this.id = id;
        this.seq = seq;
        this.name = name;
        this.data = data;
    }

    public void send(object data)
    {
        JObject o = JObject.FromObject(new
        {
            id = id,
            seq = "end",
            name = name,
            data = data
        });
        UnityMessageManager.Instance.SendMessageToFlutter(UnityMessageManager.MessagePrefix + o.ToString());
    }
}

public class UnityMessage
{
    public String name;
    public JObject data;
    public Action<object> callBack;
}

public class UnityMessageManager : SingletonMonoBehaviour<UnityMessageManager>
{

    public const string MessagePrefix = "@UnityMessage@";
    private static int ID = 0;

    private static int generateId()
    {
        ID = ID + 1;
        return ID;
    }

    public delegate void MessageDelegate(string message);
    public event MessageDelegate OnMessage;

    public delegate void MessageHandlerDelegate(MessageHandler handler);
    public event MessageHandlerDelegate OnFlutterMessage;

    private Dictionary<int, UnityMessage> waitCallbackMessageMap = new Dictionary<int, UnityMessage>();

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        NativeAPI.OnSceneLoaded(scene, mode);

    }

    public void ShowHostMainWindow()
    {
        NativeAPI.ShowHostMainWindow();
    }

    public void UnloadMainWindow()
    {
        NativeAPI.UnloadMainWindow();
    }


    public void QuitUnityWindow()
    {
        NativeAPI.QuitUnityWindow();
    }


    public void SendMessageToFlutter(string message)
    {
        NativeAPI.SendMessageToFlutter(message);
    }

    public void SendMessageToFlutter(UnityMessage message)
    {
        int id = generateId();
        if (message.callBack != null)
        {
            waitCallbackMessageMap.Add(id, message);
        }

        JObject o = JObject.FromObject(new
        {
            id = id,
            seq = message.callBack != null ? "start" : "",
            name = message.name,
            data = message.data
        });
        UnityMessageManager.Instance.SendMessageToFlutter(MessagePrefix + o.ToString());
    }

    void onMessage(string message)
    {
        if (OnMessage != null)
        {
            OnMessage(message);
        }
    }

    void onFlutterMessage(string message)
    {
        if (message.StartsWith(MessagePrefix))
        {
            message = message.Replace(MessagePrefix, "");
        }
        else
        {
            return;
        }

        MessageHandler handler = MessageHandler.Deserialize(message);
        if ("end".Equals(handler.seq))
        {
            // handle callback message
            UnityMessage m;
            if (waitCallbackMessageMap.TryGetValue(handler.id, out m))
            {
                waitCallbackMessageMap.Remove(handler.id);
                if (m.callBack != null)
                {
                    m.callBack(handler.getData<object>()); // todo
                }
            }
            return;
        }

        if (OnFlutterMessage != null)
        {
            OnFlutterMessage(handler);
        }
    }
}