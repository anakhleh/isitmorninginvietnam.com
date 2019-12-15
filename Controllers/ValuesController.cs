using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace isitmorninginvietnam.com.Controllers
{
    public class ValuesController : ControllerBase
    {

        private readonly Manager _Manager = new Manager();

        [Route("/getifitismorninginvietnam")]
        [HttpGet]
        public IsItMorningInVietnamResponse IsMorningInVietnam()
        {
            return _Manager.IsItMorningInVietnam();
        }

    }
}
