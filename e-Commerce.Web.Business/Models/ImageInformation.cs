using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models
{
    public class ImageInformation
    {
        public string FileName { get; set; }
        public byte[] ImageByteData { get; set; }
        public long Length { get; set; }
    }
}
