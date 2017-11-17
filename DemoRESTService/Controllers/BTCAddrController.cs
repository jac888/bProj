using DemoRESTService.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static DemoRESTService.DAL.DAC_BTCAddr;

namespace DemoRESTService.Controllers
{
    public class BTCAddrController : ApiController
    {
        DAC_BTCAddr dac = new DAC_BTCAddr();

        public IEnumerable<string> Get()
        {
            return new string[] { "Illigal Entry!" };
        }

        // GET: api/Person/5  single person
        public string Get(string apikey)
        {
            var vaild = 0;
            var addressList = "";
            if (!string.IsNullOrEmpty(apikey))
            {
                vaild = dac.verifyKey(apikey);
                if (vaild > 0)
                {
                    var list = dac.SelectAllTAddress();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (addressList == "")
                            addressList += list[i].ADDRESSV;
                        else
                            addressList += "," + list[i].ADDRESSV;
                    }
                } else
                    addressList = "Your Key is invalid!";
            }
            return addressList;
        }

        [ActionName("newGet")]
        [HttpGet]
        public string newGet(string apikey)
        {
            var vaild = 0;
            var addressList = "";
            if (!string.IsNullOrEmpty(apikey))
            {
                vaild = dac.verifyKey(apikey);
                if (vaild > 0)
                {
                    var list = dac.SelectAllTAddress();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (addressList == "")
                            addressList += list[i].ADDRESSV;
                        else
                            addressList += "," + list[i].ADDRESSV;
                    }
                }
                else
                    addressList = "Your Key is invalid!";
            }
            return addressList;
        }

        // POST: api/BTCAddr
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/BTCAddr/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/BTCAddr/5
        public void Delete(int id)
        {
        }
    }
}
