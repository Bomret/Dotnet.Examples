namespace Bomret.Dotnet.Examples.AsyncEnumerableHttpServer.FSharp

open System.Net
open System.Threading

open FSharp.Control

module HttpServer =
    [<Struct>]
    type Options = { Hostname: string; Port: uint16 }

    let Serve (options: Options, cancellationToken: CancellationToken): IAsyncEnumerable<HttpListenerContext> =
        asyncSeq {
            use listener = new HttpListener()

            let url =
                sprintf "http://%s:%i/" options.Hostname options.Port

            listener.Prefixes.Add(url)
            listener.Start()

            while not cancellationToken.IsCancellationRequested do
                let! context = listener.GetContextAsync() |> Async.AwaitTask

                yield context
        }
