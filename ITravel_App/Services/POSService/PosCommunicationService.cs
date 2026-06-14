using ITravel_App.Models;
using Microsoft.Extensions.Options;
using System.IO.Ports;
using System.Text;

namespace ITravel_App.Services.POSService
{
    using System.IO.Ports;
    using System.Text;

    namespace YourProject.Services.POS
    {
        public class PosCommunicationService : IPosCommunicationService, IDisposable
        {
            private readonly SerialPort _serialPort;
            private readonly ILogger<PosCommunicationService> _logger;
            private readonly SemaphoreSlim _lock = new(1, 1);

            public PosCommunicationService(
                IConfiguration configuration,
                ILogger<PosCommunicationService> logger)
            {
                _logger = logger;

                string portName = configuration["PosSettings:PortName"] ?? "COM3";
                int baudRate = int.Parse(configuration["PosSettings:BaudRate"] ?? "9600");

                _serialPort = new SerialPort
                {
                    PortName = portName,
                    BaudRate = baudRate,
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    Encoding = Encoding.UTF8,
                    ReadTimeout = 120000,
                    WriteTimeout = 10000
                };
                //_serialPort = new SerialPort(
                //    "COM3",
                //    9600,
                //    Parity.None,
                //    8,
                //    StopBits.One,
                //    ReadTimeout = 120000,
                //WriteTimeout = 10000);

                OpenPort();
            }

            private void OpenPort()
            {
                try
                {
                    if (!_serialPort.IsOpen)
                    {
                        _serialPort.Open();

                        _logger.LogInformation(
                            "POS Port Opened : {Port}",
                            _serialPort.PortName);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Unable to open POS port");
                }
            }

            public bool IsConnected()
            {
                return _serialPort.IsOpen;
            }

            public async Task<string> ConnectionTestAsync()
            {
                string xml =
                    @"<CONNTOPOS><ConnTest>800</ConnTest></CONNTOPOS>";

                return await SendAsync(xml);
            }

            public async Task<string> SendAsync(string xml)
            {
                await _lock.WaitAsync();

                try
                {
                    if (!_serialPort.IsOpen)
                        OpenPort();

                    _serialPort.DiscardInBuffer();
                    _serialPort.DiscardOutBuffer();

                    _logger.LogInformation(
                        "Sending XML To POS: {Xml}",
                        xml);

                   // _serialPort.Write(xml);
                    _serialPort.Write(xml + "\r\n");

                    var response = await ReadResponseAsync();

                    _logger.LogInformation(
                        "Response From POS: {Response}",
                        response);

                    return response;
                }
                finally
                {
                    _lock.Release();
                }
            }

            private async Task<string> ReadResponseAsync()
            {
                var sb = new StringBuilder();

                var startTime = DateTime.UtcNow;

                while ((DateTime.UtcNow - startTime).TotalSeconds < 120)
                {
                    try
                    {
                        string data = _serialPort.ReadExisting();

                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            sb.Append(data);

                            string response = sb.ToString();

                            if (response.Contains("</CONNFROMPOS>"))
                                return response;

                            if (response.Contains("</SALE_RESPONSE>"))
                                return response;

                            if (response.Contains("</PAYMENT_RESPONSE>"))
                                return response;

                            if (response.Contains("</RESPONSE>"))
                                return response;
                        }
                    }
                    catch
                    {
                    }

                    await Task.Delay(250);
                }

                throw new TimeoutException(
                    "Timeout waiting for POS response.");
            }

            public void Dispose()
            {
                try
                {
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                    }

                    _serialPort.Dispose();
                }
                catch
                {
                }
            }
        }
    }
}
