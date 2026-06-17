using ITravelApp.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITravelApp.Data.Models.Transfer
{
    public class TransferImgReq : transfer_img
    {
        public List<IFormFile>? imgs { get; set; }
    }
}
