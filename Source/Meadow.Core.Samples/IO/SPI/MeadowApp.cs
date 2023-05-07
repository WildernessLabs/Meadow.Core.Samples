namespace SPI
{
    //public class SampleSpi : ISpiCommunications
    //{
    //    public SampleSpi(ISpiBus bus, IDigitalOutputPort chipSelect) : base(bus, chipSelect)
    //    {
    //    }
    //}

    //public class MeadowApp : App<F7FeatherV2>
    //{
    //    private IDigitalOutputPort _chipSelect;
    //    private ISpiBus _spiBus;
    //    private SampleSpi _peripheral;

    //    public override Task Initialize()
    //    {
    //        Console.WriteLine($"SPI TEST");

    //        _chipSelect = Device.CreateDigitalOutputPort(Device.Pins.D04);
    //        _spiBus = Device.CreateSpiBus();
    //        _peripheral = new SampleSpi(_spiBus, _chipSelect);

    //        return Task.CompletedTask;
    //    }

    //    public override Task Run()
    //    {
    //        var result = _peripheral.ReadRegister(0x42);

    //        Console.WriteLine($"Read result: {result}");

    //        return Task.CompletedTask;
    //    }
    //}
}
