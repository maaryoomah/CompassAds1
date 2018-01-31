using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompassAds1.Models
{
    public class PhotoViewImage
    {
        public string Name { get; set; }
        public string AlternateText { get; set; }
        public byte[] ActualImage { get; set; }
        public string ContentType { get; set; }
        

    }
}