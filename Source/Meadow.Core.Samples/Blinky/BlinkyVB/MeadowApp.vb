Imports System.Threading
Imports Meadow
Imports Meadow.Devices
Imports Meadow.Foundation
Imports Meadow.Foundation.Leds
Imports Meadow.Peripherals.Leds

Public Class MeadowApp
    'Change F7FeatherV2 to F7FeatherV1 for V1.x boards'
    Inherits App(Of F7FeatherV2)

    Private onboardLed As RgbPwmLed

    Public Overrides Function Run() As Task
        Console.WriteLine("Run... (VB.NET)")

        CycleColors(TimeSpan.FromMilliseconds(1000))

        Return MyBase.Run()
    End Function

    Public Overrides Function Initialize() As Task
        Console.WriteLine("Initialize... (VB.NET)")

        onboardLed = New RgbPwmLed(Device,
            Device.Pins.OnboardLedRed,
            Device.Pins.OnboardLedGreen,
            Device.Pins.OnboardLedBlue,
            CommonType.CommonAnode)

        Return MyBase.Run()
    End Function

    Private Sub CycleColors(ByVal duration As TimeSpan)
        Console.WriteLine("Cycle colors...")

        While True
            ShowColorPulse(Color.Blue, duration)
            ShowColorPulse(Color.Cyan, duration)
            ShowColorPulse(Color.Green, duration)
            ShowColorPulse(Color.GreenYellow, duration)
            ShowColorPulse(Color.Yellow, duration)
            ShowColorPulse(Color.Orange, duration)
            ShowColorPulse(Color.OrangeRed, duration)
            ShowColorPulse(Color.Red, duration)
            ShowColorPulse(Color.MediumVioletRed, duration)
            ShowColorPulse(Color.Purple, duration)
            ShowColorPulse(Color.Magenta, duration)
            ShowColorPulse(Color.Pink, duration)
        End While

    End Sub

    Private Sub ShowColorPulse(ByVal color As Color, ByVal duration As TimeSpan)
        onboardLed.StartPulse(color, duration / 2)
        Thread.Sleep(duration)
    End Sub

End Class