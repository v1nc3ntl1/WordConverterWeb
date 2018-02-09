
using System.Runtime.Serialization;

namespace WordConverterAPI.Models
{
    [DataContract]
    public class WordModel
    {
        [DataMember(Name = "noString")]
        public string Word { get; set; }

        [DataMember(Name = "no")]
        public decimal Number { get; set; }
    }
}