using ITravel_App.Services.POSService;
using Microsoft.AspNetCore.Mvc;
using System.IO.Ports;

namespace ITravel_App.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PosController : ControllerBase
    {
        private readonly IPosCommunicationService _posService;

        public PosController(
            IPosCommunicationService posService)
        {
            _posService = posService;
        }
        [HttpGet("ports")]
        public IActionResult Ports()
        {
            return Ok(System.IO.Ports.SerialPort.GetPortNames());
        }
        [HttpGet("scan")]
        public async Task<IActionResult> ScanPorts()
        {
            var results = new List<object>();

            foreach (var portName in SerialPort.GetPortNames().Distinct())
            {
                try
                {
                    using var port = new SerialPort(
                        portName,
                        9600,
                        Parity.None,
                        8,
                        StopBits.One);

                    port.ReadTimeout = 3000;
                    port.WriteTimeout = 3000;

                    port.Open();

                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();

                    string xml =
        @"<CONNTOPOS>
<ConnTest>800</ConnTest>
</CONNTOPOS>";

                    port.Write(xml);

                    await Task.Delay(3000);

                    string response = port.ReadExisting();

                    results.Add(new
                    {
                        Port = portName,
                        Response = response
                    });

                    port.Close();
                }
                catch (Exception ex)
                {
                    results.Add(new
                    {
                        Port = portName,
                        Error = ex.Message
                    });
                }
            }

            return Ok(results);
        }
        [HttpGet("status")]
        public IActionResult Status()
        {
            return Ok(new
            {
                Connected = _posService.IsConnected()
            });
        }

        [HttpGet("test")]
        public async Task<IActionResult> ConnectionTest()
        {
            try
            {
                var response =
                    await _posService.ConnectionTestAsync();

                return Ok(new
                {
                    Success = true,
                    Response = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendXml(
            [FromBody] PosXmlRequest request)
        {
            try
            {
                var response =
                    await _posService.SendAsync(
                        request.Xml);

                return Ok(new
                {
                    Success = true,
                    Response = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }

    public class PosXmlRequest
    {
        public string Xml { get; set; } = string.Empty;
    }

}
