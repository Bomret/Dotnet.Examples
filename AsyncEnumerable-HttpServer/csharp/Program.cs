using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bomret.Dotnet.Examples.AsyncEnumerableHttpServer.CSharp
{
  static class Program
  {
    static readonly ReadOnlyMemory<byte> Message = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes("Hello World"));

    static void Main(string[] args)
    {
      using var cts = new CancellationTokenSource();
      Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs args) => cts.Cancel();

      Do(cts.Token).GetAwaiter().GetResult();
    }

    static async Task Do(CancellationToken cancellationToken)
    {
      var options = new HttpServer.Options
      {
        Hostname = "localhost",
        Port = 8080
      };

      await foreach (var ctx in HttpServer.Serve(options, cancellationToken))
      {
        using var outputStream = ctx.Response.OutputStream;

        await outputStream.WriteAsync(Message, cancellationToken);
      }
    }
  }
}
