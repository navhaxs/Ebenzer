using Ebenezer.Data;
using GrpcDotNetNamedPipes;

namespace Ebenezer;

public static class PowerModule
{
    public static async void RequestShutdown()
    {
        Console.WriteLine("Request shutdown");
        var channel = new NamedPipeChannel(".", Constants.PipeName);
        var client = new Greeter.GreeterClient(channel);

        try
        {

            var response = await client.SayHelloAsync(
                new HelloRequest { Name = "World" });
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public static async void CancelShutdown()
    {
        Console.WriteLine("Cancel shutdown");
        var channel = new NamedPipeChannel(".", Constants.PipeName);
        var client = new Greeter.GreeterClient(channel);

        try
        {

            client.Abort(
                new HelloRequest { Name = "World" });
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }}