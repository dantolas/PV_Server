using _17._04.Commands;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace _17._04
{
    internal class Server 
    {
        private TcpListener myServer;

        private bool isRunning;

        public Dictionary<string, string> users;

        private Dictionary<string, ICommand> myCommands;
        
        private List<string> bannedAddresses;

        public ConcurrentBag<string> connectedUsers;

        public DateTime timeStarted;

        public int nOfConnectionAttempts;
        public int successConnections;
        public int failedConnections;

        public int commandsExecuted;

        public Server(int port)
        {
            #region <Init>
            this.users = new Dictionary<string, string>();
            myCommands = new Dictionary<string, ICommand>();
            bannedAddresses = new List<string>();
            connectedUsers = new ConcurrentBag<string>();
            timeStarted = DateTime.Now;
            nOfConnectionAttempts = 0;
            successConnections = 0;
            failedConnections = 0;
            commandsExecuted = 0;

            UpdateUsers();

            myCommands.Add("uptime",new UptimeCommand(this));
            myCommands.Add("who", new WhoCommand(this));
            myCommands.Add("help", new HelpCommand());
            myCommands.Add("time", new TimeCommand());
            
           

            #endregion


            myServer = new TcpListener(System.Net.IPAddress.Any, port);
            myServer.Start();
            isRunning = true;
            Console.WriteLine("Server running. | Working directory:" + Directory.GetCurrentDirectory()+ " | Running on localhost port:"+port);
            
            ServerLoop();
        }

        private void ServerLoop()
        {
            
            while (isRunning)
            {
                Console.WriteLine("Server waiting for connection.");
                TcpClient client = myServer.AcceptTcpClient();
                nOfConnectionAttempts++;
                ClientLoop(client);
            }
        }

        private void WriteLog(string username)
        {
            using (StreamWriter writer = new StreamWriter("../../../logs.json",true))
            {
                if(File.ReadAllText("../../../logs.json").Length <= 0)
                {
                    writer.WriteLine("{" + "\"Username\"" + username + ",\"DateTime\"" + DateTime.Now.ToString() + "}");
                    return;
                }

                writer.WriteLine(",{"+"\"Username\""+username+ ",\"DateTime\"" +DateTime.Now.ToString()+ "}");
                writer.Close();
            }
            
        }

        private void UpdateUsers()
        {
            Console.WriteLine("Updating users.");
            string jsonText = File.ReadAllText("../../../users.json");

            UserList users = JsonSerializer.Deserialize<UserList>(jsonText)!;

            Dictionary<string,string> keyValuePairs = new Dictionary<string,string>();

            users.users.ForEach(user =>
            {
                keyValuePairs.Add(user.Username, user.Password);
            });

            this.users = keyValuePairs;
        }


        private bool ClientLogin(TcpClient client, StreamReader reader, StreamWriter writer,string user)
        {

            IPEndPoint ip = client.Client.RemoteEndPoint as IPEndPoint;
            if (bannedAddresses.Contains(ip.ToString())) return false;
            Console.WriteLine("ipNotBanned");

            int loginAttempts = 3;
            
            try
            {
                
            }catch(Exception e)
            {
                writer.WriteLine("Unable to login at this time, please try again later.");
                writer.Flush();
                return false;
            }

            bool repeat;

            do
            {
                if(loginAttempts <= 0)
                {
                    return false;
                }

                repeat = false;

                writer.WriteLine("Username:");
                writer.Flush();
                string? data = null;
                data = reader.ReadLine().ToLower();
                Console.WriteLine("data:" + data);

                string username = data;

                if (!users.Keys.Contains(username))
                {
                    writer.WriteLine("Username not in userbase.");
                    writer.Flush();
                    repeat = true;
                    continue;
                }

                writer.WriteLine("Password:");
                writer.Flush();
                data = null;
                data = reader.ReadLine();
                Console.WriteLine("data:" + data);

                string password = data;

                if (!users[username].Equals(password))
                {
                    writer.WriteLine("Incorrect password.");
                    writer.Flush();
                    repeat = true;
                    continue;
                }


            } while (repeat);
            

         
            return true;

        }

        

        private void ClientLoop(TcpClient client)
        {

            try
            {

                StreamReader reader = new StreamReader(client.GetStream(), Encoding.UTF8);
                StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.UTF8);

                string username = "";

                if (!ClientLogin(client, reader, writer,username))
                {
                    IPEndPoint ip = client.Client.RemoteEndPoint as IPEndPoint;
                    bannedAddresses.Add(ip.ToString());
                    failedConnections++;
                    return;
                }

                successConnections++;
               
                // WriteLog(username);

                bool clientConnect = true;

                writer.WriteLine("Connected!!!");


                string? data = null;

                //while (true)
                //{
                //    try
                //    {
                //        writer.WriteLine("Please enter your nickname:");
                //        data = reader.ReadLine().ToLower();
                        
                //        if (connectedUsers.Contains(data)) {
                //            writer.WriteLine("Nickname taken.");
                //            continue;
                //        }

                //        connectedUsers.Add(data);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("Exception caught in nickname select");
                //    }
                //}

                while (clientConnect)
                {
                    
                    data = reader.ReadLine().ToLower();

                    writer.WriteLine("------------------------");
                    writer.Flush();

                    if (data.Equals("exit")) { clientConnect = false; break; }

                    if(!myCommands.Keys.Contains(data))
                    {
                        writer.WriteLine("Command not known.");
                        writer.Flush();
                        continue;
                    }

                    writer.WriteLine(myCommands[data].Exec());
                    writer.Flush();

                    


                }
                writer.WriteLine("Byl jsi odpojen.");
                writer.Flush();
                Console.WriteLine("Client disconnected.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
                ServerLoop();
            }


        }
    }
}
