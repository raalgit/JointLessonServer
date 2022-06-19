using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL_ApiModels.Response
{
    public interface IResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool ShowMessage { get; set; } 
    }
}
