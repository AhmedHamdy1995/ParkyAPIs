using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext db;

        public NationalParkRepository(ApplicationDbContext db)
        {
            this.db = db;
        }


        public bool CheckNationalParkExists(int Id)
        {         
            return db.NationalParks.Any(item=>item.Id==Id);
        }

        public bool CheckNationalParkExists(string Name)
        {
            return db.NationalParks.Any(item => item.Name.ToLower().Equals(Name.ToLower()));
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int Id)
        {
            return db.NationalParks.Find(Id);
        }

        public NationalPark GetNationalPark(string Name)
        {
            return db.NationalParks.FirstOrDefault(t => t.Name.ToLower().Equals(Name.ToLower()));
        }

        public IEnumerable<NationalPark> GetNationalParks()
        {
            return db.NationalParks.ToList();
        }

        public bool Save()
        {
            return db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            db.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
