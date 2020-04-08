using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CommandLine;

namespace CSharpGameServer
{
    class Program
    {
        // Incoming data from the client.
        public static string data = null;

        public static void StartListening(string port)
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // Dns.GetHostName returns the name of the host running the application.
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Int32.Parse(port));
            Console.WriteLine($"Local endpoint identified {ipAddress}:{port}");

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                Console.WriteLine("Binding to local endpoint.");
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }

                    // Show the data on the console.
                    Console.WriteLine("Text received : {0}", data);
                    
                    // Roll a d20 for the player.
                    Random rnd = new Random();
                    int diceRoll = rnd.Next(1, 21);
                    Console.WriteLine($"{data} rolled a {diceRoll}!");

                    // Echo the data back to the client.
                    byte[] msg = Encoding.ASCII.GetBytes(data + $", you rolled a {diceRoll}!");

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public class Options
        {
            [Option('p', "port", Required = true, HelpText = "Port to listen on")]
            public string Port { get; set; }
        }

        public static int Main(String[] args)
        {
            Console.WriteLine("CSharpGameServer started with arguments: {0}", args);

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o => { StartListening(o.Port); });

            return 0;
        }
    }
}
