using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos.Facades
{
    static class NetworkFacade
    {
        static public MainHandler MainHandling = new MainHandler();
        static public DataHandler DataHandling = new DataHandler();

        static public Queue Sending = new Queue();
        static public Queue Receiving = new Queue();
    }
}
