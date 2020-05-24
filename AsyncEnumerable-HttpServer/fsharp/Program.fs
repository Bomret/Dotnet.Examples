namespace Bomret.Dotnet.Examples.AsyncEnumerableHttpServer.FSharp

open System
open System.Text
open System.Threading
open FSharp.Control

module Program =
    let Message =
        ReadOnlyMemory(Encoding.UTF8.GetBytes("Hello World"))

    let Do (cancellationToken: CancellationToken) =
        let options: HttpServer.Options =
            { Hostname = "localhost"
              Port = 8080us }

        HttpServer.Serve(options, cancellationToken)
        |> AsyncSeq.iterAsync (fun ctx ->
            async {
                use output = ctx.Response.OutputStream
                do! output.WriteAsync(Message, cancellationToken).AsTask()
                    |> Async.AwaitTask
            })

    [<EntryPoint>]
    let main _ =
        use cts = new CancellationTokenSource()
        Console.CancelKeyPress.Add(fun _ -> cts.Cancel())

        Do(cts.Token) |> Async.RunSynchronously
        0
