using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CommandLine;

namespace CSharpGameClient
{
    // State object for receiving data from remote device.  
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 256;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class Program
    {
        public static void StartClient(string ipAddressArg, string port, string playerNameArg)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPAddress ipAddress = IPAddress.Parse(ipAddressArg);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, Int32.Parse(port));

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes($"{playerNameArg}<EOF>");

                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Response = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public class Options
        {
            [Option('i', "ipAddress", Required = true, HelpText = "IP address of game-server")]
            public string IpAddress { get; set; }
            [Option('p', "port", Required = true, HelpText = "Port of game-server")]
            public string Port { get; set; }
            [Option('n', "name", Required = true, HelpText = "Player name")]
            public string PlayerName { get; set; }
        }

        public static int Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o => { StartClient(o.IpAddress, o.Port, o.PlayerName); });

            return 0;
        }
    }
}
