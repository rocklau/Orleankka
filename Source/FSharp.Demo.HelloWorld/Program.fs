﻿open System
open System.Reflection

open Orleankka
open Orleankka.FSharp
open Orleankka.Playground

type Message = 
   | Greet of string
   | Hi

type Greeter() = 
   inherit Actor<Message>()   

   override this.Receive message reply = task {
      match message with
      | Greet who -> printfn "Hello %s" who
      | Hi -> printfn "Hello from F#!"           
   }

[<EntryPoint>]
let main argv = 

    printfn "Running demo. Booting cluster might take some time ...\n"

    use system = ActorSystem.Configure()
                            .Playground()
                            .Register(Assembly.GetExecutingAssembly())
                            .Done()
                  
    let actor = system.ActorOf<Greeter>(Guid.NewGuid().ToString())

    let job() = task {
      do! actor <! Hi
      do! actor <! Greet "Yevhen"
      do! actor <! Greet "AntyaDev"
    }
    
    Task.run(job) |> ignore
    
    Console.ReadLine() |> ignore    
    0