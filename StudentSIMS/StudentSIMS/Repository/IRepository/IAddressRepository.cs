using StudentSIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSIMS.Repository.IRepository
{
    public interface IAddressRepository
    {
        ICollection<Address> GetAddresses();
        ICollection<Address> GetAddressesOfStudent(int studentId);
        Address GetAddress(int addressId);
        bool CreateAddress(Address address);
        bool UpdateAddress(Address address);
        bool DeleteAddress(Address address);
        bool AddressExists(int id);
        bool Save();
    }
}
