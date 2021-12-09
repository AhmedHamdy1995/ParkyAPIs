using Microsoft.EntityFrameworkCore;
using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext db;

        public TrailRepository(ApplicationDbContext db)
        {
            this.db = db;
        }


        public bool CheckTrailExists(int Id)
        {         
            return db.Trails.Any(item=>item.Id==Id);
        }

        public bool CheckTrailExists(string Name)
        {
            return db.Trails.Any(item => item.Name.ToLower().Equals(Name.ToLower()));
        }

        public bool CreateTrail(Trail Trail)
        {
            db.Trails.Add(Trail);
            return Save();
        }

        public bool DeleteTrail(Trail Trail)
        {
            db.Trails.Remove(Trail);
            return Save();
        }

        public Trail GetTrail(int Id)
        {
            return db.Trails.Include(m => m.NationalPark).FirstOrDefault(n => n.Id == Id);
        }

        public Trail GetTrail(string Name)
        {
            return db.Trails.FirstOrDefault(t => t.Name.ToLower().Equals(Name.ToLower()));
        }

        public IEnumerable<Trail> GetTrails()
        {
            return db.Trails.Include(a => a.NationalPark).ToList();
        }

        public IEnumerable<Trail> GetTrailsInNationalPark(int npId)
        {
            return db.Trails.Include(a => a.NationalPark).Where(a => a.NationalParkId == npId).ToList();
        }

        public bool Save()
        {
            return db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail Trail)
        {
            db.Trails.Update(Trail);
            return Save();
        }
    }
}
