using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]

    public class DataModel
    {
        [DataMember]
        public string Ime { get; set; } = null!;
        [DataMember]
        public string Prezime { get; set; } = null!;
        [DataMember]
        public int IdKnjige { get; set; }
        [DataMember]
        public int KolicinaKnjige { get; set; }
        [DataMember]
        public int BrojRacuna { get; set; }
    }
}
