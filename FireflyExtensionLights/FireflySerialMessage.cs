using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireflyExtensionLights
{
    public class FireflySerialMessage
    {
        public bool Buzzer { get; set; }
        public bool RightGround { get; set; }
        public bool LeftGround { get; set; }
        public bool RightInvalid { get; set; }
        public bool LeftInvalid { get; set; }
        public bool RightValid { get; set; }
        public bool LeftValid { get; set; }
        public int Message { get; set; }
        public int Value { get; set; }
    }
}
