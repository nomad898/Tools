using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Entities
{
    class Entity
    {
        [DataContract]
        public int Id { get; set; }
        [DataContract]
        public string Name { get; set; }
    }
}