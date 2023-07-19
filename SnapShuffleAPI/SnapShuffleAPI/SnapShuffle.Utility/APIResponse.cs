using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapShuffle.Utility
{
    public class APIResponse
    {
        public APIResponse()
        {

        }
        public APIResponse(ResponseCode code, string message, object data = null, bool isencrypted = false)
        {
            Code = code;
            Message = message;
            Document = data;
            IsEncrypted = isencrypted;
        }
        public ResponseCode Code { get; set; }
        public string Message { get; set; }
        public object Document { get; set; }
        public bool IsEncrypted { get; set; }
    }

    public enum ResponseCode
    {
        ERROR = 0,
        SUCCESS = 1
    }
}
