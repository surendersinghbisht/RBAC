using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    public class AllAddresses
    {
        public int AddressId { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }

        public int StateId { get; set; }
        public string State { get; set; }

        public int CityId { get; set; }
        public string City { get; set; }

        public string FullAddress { get; set; }

    }
}
