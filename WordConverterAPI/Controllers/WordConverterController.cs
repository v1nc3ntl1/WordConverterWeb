using System.Web.Http;
using System.Web.Http.Cors;
using WordConverterAPI.Models;
using WordConverterLibrary;

namespace WordConverterAPI.Controllers
{
    /// <summary>
    /// Controller to WordConverter which would call the underlying interface to convert number into words.
    /// </summary>
    [EnableCors("http://localhost:4200", "*", "*")]
    public class WordConverterController : ApiController
    {
        private readonly IWordConverterProvider _provider;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="provider"><see cref="IWordConverterProvider"/> underlying word converter provider that do the magic</param>
        public WordConverterController(IWordConverterProvider provider)
        {
            this._provider = provider;
        }

        /// <summary>
        /// Convert a number to a word in number
        /// For example, 
        /// from 100 to one hundred dollar
        /// </summary>
        /// <param name="number">input number</param>
        /// <returns><see cref="IHttpActionResult"/> that wrap the word number</returns>
        // GET: api/WordConverter
        [Route("api/Converter/{number}")]
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
