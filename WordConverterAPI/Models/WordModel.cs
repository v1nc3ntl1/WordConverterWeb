
using System.Runtime.Serialization;

namespace WordConverterAPI.Models
{
    /// <summary>
    /// Number in word Controller model 
    /// </summary>
    [DataContract]
    public class WordModel
    {
        /// <summary>
        /// Word of a number. For example, one hundred and thirty dollar
        /// </summary>
        [DataMember(Name = "noString")]
        public string Word { get; set; }

        /// <summary>
        /// number. For example, 100
        /// </summary>
        [DataMember(Name = "no")]
        public decimal Number { get; set; }
    }
}