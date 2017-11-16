using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using DemoRESTService.DAL;
using DemoRESTService.Models;

namespace DemoRESTService.Controllers
{
    public class PersonController : ApiController
    {
        DAC_Person dac = new DAC_Person();
        // GET: api/Person    all persons
        public IEnumerable<string> Get()
        {
            return new string[] { "Person1", "Person2" };
        }

        // GET: api/Person/5  single person
        public Person Get(int id)
        {
            var item = dac.SelectOne(id).FirstOrDefault();
            return item;
        }

        // POST: api/Person   save person
        public HttpResponseMessage Post([FromBody]Models.Person value)
        {           
            var id = dac.InsertOne(value);
            HttpResponseMessage respond = Request.CreateResponse(HttpStatusCode.Created);
            respond.Headers.Location = new Uri(Request.RequestUri + id.ToString());
            return respond;
        }

        // PUT: api/Person/5   update person
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Person/5   delete person
        public void Delete(int id)
        {
        }
    }
}
