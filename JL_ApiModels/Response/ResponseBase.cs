using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL_ApiModels.Response
{
    public class ResponseBase : IResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = String.Empty;
        public bool ShowMessage { get; set; } = false;
    }
}
