using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WordConverterAPI.Models;
using WordConverterLibrary;

namespace WordConverterAPI.Controllers
{
    public class WordConverterController : ApiController
    {
        private readonly IWordConverterProvider _provider = new SimpleWordConverterProvider();

        public WordConverterController(IWordConverterProvider provider)
        {
            this._provider = provider;
        }

        // GET: api/WordConverter
        public IEnumerable<WordModel> Get()
        {
            return new List<WordModel>(){ new WordModel() { Number = 0, Word = this._provider.Convert(0)}};
        }

        // GET: api/WordConverter/5
        //public WordModel Get(int id)
        //{
        //    return new WordModel() { Number = id, Word = this._provider.Convert(id) };
        //}

        public WordModel Get(decimal id)
        {
            return new WordModel() { Number = id, Word = this._provider.Convert(id) };
        }

        // POST: api/WordConverter
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/WordConverter/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/WordConverter/5
        public void Delete(int id)
        {
        }
    }
}
