using System.Collections.Generic;
using System.Linq;

namespace ganntproj1
{
    public class Channels
    {
        public Channels()
        {
            Channel = ListOfChannels().First().Channel;
        }
        private Channels(string name)
        {
            Channel = name;
        }       
        public string Channel { get; }
        public List<Channels> ListOfChannels()
        {
            var lst = new List<Channels>
            {
                new Channels("TESSITURA"),
                new Channels("CONFEZIONE"),
                new Channels("STIRO")
            };
            return lst;
        }
    }
}