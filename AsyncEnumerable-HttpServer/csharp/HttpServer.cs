using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Bomret.Dotnet.Examples.AsyncEnumerableHttpServer.CSharp
{
  public static class HttpServer
  {
    public struct Options
    {
      public string Hostname;
      public ushort Port;
    }

    public static async IAsyncEnumerable<HttpListenerContext> Serve(Options options, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
      using var listener = new HttpListener();

      string url = $"http://{options.Hostname}:{options.Port}/";

      listener.Prefixes.Add(url);
      listener.Start();

      while (!cancellationToken.IsCancellationRequested)
      {
        var context = await listener.GetContextAsync().ConfigureAwait(false);

        yield return context;
      }
    }
  }
}