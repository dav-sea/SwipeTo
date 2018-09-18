using UnityEngine;
using System.Collections.Generic;

namespace Ethers
{
    public class Ether
    {
        public delegate void SubscriberDelegate<INFO>(INFO info);
        public List<IChannel> Channels { private set; get; }

        public IChannel GetChannel(string name)
        {
            for (int i = Channels.Count; i >= 0; --i)
                if (Channels[i].Name == name)
                    return Channels[i];
            return null;
        }

        public Ether()
        {
            Channels = new List<IChannel>(5);
        }
    }
}