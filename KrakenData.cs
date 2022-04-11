namespace cun83.NzxtKrakenSensorUnlocker
{
    /// <summary>
    /// Measurements read from the Kraken device.
    /// </summary>
    public class KrakenData
    {
        public KrakenData() { }

        public float LiquidTempC { get; set; }
        public int PumpSpeedRpm { get; set; }
        public int PumpDutyPercent { get; set; }

        public int FanSpeedRpm { get; set; }
        public int FanDutyPercent { get; set; }

        public KrakenData(byte[] data) {
            //offsets taken from src: https://github.com/liquidctl/liquidctl/blob/0fc13d8ef1d5f0fe5c55e886b775f45412e57b0c/liquidctl/driver/kraken3.py#L28
            
            LiquidTempC = data[15] + (data[16] / 10f);

            PumpDutyPercent = data[19];
            PumpSpeedRpm = data[18] << 8 | data[17];

            FanDutyPercent = data[25];
            FanSpeedRpm = data[24] << 8 | data[23];
        }

    }
}