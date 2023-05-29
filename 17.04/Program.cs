namespace _17._04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "MyTCPServer";
            Server server = new Server(8080);
        }
    }
}