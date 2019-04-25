using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Mvc_Identity.ViewModels
{
    [DataContract]
    public class JsonDeSerializer
    {
        public string Name { get; set; }
    }
}
