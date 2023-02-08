namespace MeadowApp

open System
open Meadow.Devices
open Meadow
open Meadow.Foundation.Leds
open Meadow.Foundation
open Meadow.Peripherals.Leds

type MeadowApp() =
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    inherit App<F7FeatherV2>()

    let mutable led : RgbPwmLed = 
        null

    let ShowColorPulse (color : Color) (duration : TimeSpan) = 
        led.StartPulse(color, duration.Divide(2)) |> ignore
        Threading.Thread.Sleep (duration) |> ignore
        led.Stop |> ignore
    
    let CycleColors (duration : TimeSpan)  = 
        do Console.WriteLine "Cycle colors..."

        while true do
            ShowColorPulse Color.Blue duration 
            ShowColorPulse Color.Cyan duration
            ShowColorPulse Color.Green duration
            ShowColorPulse Color.GreenYellow duration
            ShowColorPulse Color.Yellow duration
            ShowColorPulse Color.Orange duration
            ShowColorPulse Color.OrangeRed duration
            ShowColorPulse Color.Red duration
            ShowColorPulse Color.MediumVioletRed duration
            ShowColorPulse Color.Purple duration
            ShowColorPulse Color.Magenta duration
            ShowColorPulse Color.Pink duration

    override this.Initialize() =
        do Console.WriteLine "Initialize... (F#)"

        led <- new RgbPwmLed(
            MeadowApp.Device.Pins.OnboardLedRed,
            MeadowApp.Device.Pins.OnboardLedGreen, 
            MeadowApp.Device.Pins.OnboardLedBlue, 
            CommonType.CommonAnode)

        base.Initialize()
        
    override this.Run () =
        do Console.WriteLine "Run... (F#)"

        do CycleColors (TimeSpan.FromSeconds(1))

        base.Run()