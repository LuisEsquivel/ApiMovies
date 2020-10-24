using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Helpers
{
    public class Response
    {

        public object ResponseValues(int StatusCode,
                                     object Model = null,
                                     string Message = ""
                                    )
        {

            object result = new  
            {  
                statuscode = StatusCode,
                status = StatusCode == 200 ? "success" : "error",
                message = Message,
                model  = Model
            };


            object json = new { result };

            return JsonConvert.SerializeObject(json);

        }

    }
}
