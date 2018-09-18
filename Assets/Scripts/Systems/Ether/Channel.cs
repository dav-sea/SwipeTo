using UnityEngine;
using System.Collections.Generic;

namespace Ethers
{
    public class Channel : IChannel
    {
        private List<Ether.SubscriberDelegate<Channel.Info>> Subscribers = new List<Ether.SubscriberDelegate<Channel.Info>>(1);

        public bool Enabled { protected set; get; }
        public string Name { private set; get; }

        public int CountSubscribers { get { return Subscribers.Count; } }
        public void Push(object source, object sender)
        {
            if (!Enabled) return;

            var info = new Info(this, source, sender);

            for (int i = Subscribers.Count - 1; i >= 0; --i)
                Subscribers[i].Invoke(info);
        }

        public void Subscribe(Ether.SubscriberDelegate<Channel.Info> subscriber)
        {
            Subscribers.Add(subscriber);
        }

        public void Unsubscribe(Ether.SubscriberDelegate<Channel.Info> subscriber)
        {
            Subscribers.Remove(subscriber);
        }

        private Channel() { }
        public Channel(string name)
        {

            Name = name;
            Enabled = true;
        }

        public class Info
        {
            public Channel Channel { private set; get; }
            public object Source;
            public object Sender;

            private Info() { }
            public Info(Channel channel, object source, object sender)
            {
                Channel = channel;
                Source = source;
                Sender = sender;
            }
        }
    }

    public interface IChannel
    {
        bool Enabled { get; }
        string Name { get; }
        int CountSubscribers { get; }
        void Push(object source, object sender);
        void Subscribe(Ether.SubscriberDelegate<Channel.Info> subscriber);
        void Unsubscribe(Ether.SubscriberDelegate<Channel.Info> subscriber);
    }
}