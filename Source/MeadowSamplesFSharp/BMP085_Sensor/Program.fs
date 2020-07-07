// Learn more about F# at http://fsharp.org

open System
open Meadow
open Meadow.Devices
open Meadow.Foundation.Sensors.Atmospheric
open System.Threading


//based on the c# example here: http://developer.wildernesslabs.co/docs/api/Meadow.Foundation/Meadow.Foundation.Sensors.Atmospheric.Bmp085.html
type SensorApp () =
    inherit App<F7Micro, SensorApp>()
    do printfn "Initialising..."
    let i2c = SensorApp.Device.CreateI2cBus()
    let bmp085 = new Bmp085(i2c)
    do bmp085.Updated.Add (fun e -> 
            printfn "Temp: %f°C, Pressure: %fhPa" e.New.Temperature.Value e.New.Pressure.Value
        )

    // get an initial reading
    do bmp085.Read() 
    |> Async.AwaitTask 
    |> Async.RunSynchronously 
    |> (fun condition -> printfn "Initial reading is Temp: %f°C, Pressure: %fhPa" condition.Temperature.Value condition.Pressure.Value)
    
    // start updating
    do bmp085.StartUpdating()


[<EntryPoint>]
let main argv =
    let app = new SensorApp()
    Thread.Sleep Timeout.Infinite
    0 // return an integer exit code
