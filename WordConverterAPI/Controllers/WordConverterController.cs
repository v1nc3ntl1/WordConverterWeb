﻿using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WordConverterAPI.Models;
using WordConverterLibrary;

namespace WordConverterAPI.Controllers
{
    [EnableCors("http://localhost:59645", "*", "*")]
    public class WordConverterController : ApiController
    {
        private readonly IWordConverterProvider _provider;

        public WordConverterController(IWordConverterProvider provider)
        {
            this._provider = provider;
        }

        // GET: api/WordConverter
        public IHttpActionResult Get(decimal number)
        {
            // sanitize input
            if (number < 0)
            {
                return BadRequest("Invalid number");
            }

            return Ok(new WordModel() { Number = number, Word = this._provider.Convert(number) });
        }
    }
}
