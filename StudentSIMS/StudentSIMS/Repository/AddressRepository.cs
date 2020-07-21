using Microsoft.EntityFrameworkCore;
using StudentSIMS.Data;
using StudentSIMS.Models;
using StudentSIMS.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSIMS.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly StudentContext _db;

        public AddressRepository(StudentContext db)
        {
            _db = db;
        }

        public bool CreateAddress(Address address)
        {
            _db.Address.Add(address);
            return Save();
        }

        public bool DeleteAddress(Address address)
        {
            _db.Address.Remove(address);
            return Save();
        }

        public Address GetAddress(int addressId)
        {
            return _db.Address.Include(a => a.Student).FirstOrDefault(a => a.addressId == addressId); ;
        }

        public ICollection<Address> GetAddresses()
        {
            return _db.Address.Include(a => a.Student).OrderBy(a => a.addressId).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool AddressExists(int id)
        {
            return _db.Address.Any(a => a.addressId == id);
        }

        public bool UpdateAddress(Address address)
        {
            _db.Address.Update(address);
            return Save();
        }

        public ICollection<Address> GetAddressesOfStudent(int studentId)
        {
            return _db.Address.Include(a => a.Student).Where(a => a.studentId == studentId).ToList();
        }
    }
}