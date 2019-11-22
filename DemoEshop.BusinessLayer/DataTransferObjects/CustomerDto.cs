using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class CustomerDto : DtoBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string MobilePhoneNumber { get; set; }

        public string Address { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
