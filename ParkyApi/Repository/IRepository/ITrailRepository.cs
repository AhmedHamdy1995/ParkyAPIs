using ParkyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository.IRepository
{
    public  interface ITrailRepository
    {
        IEnumerable<Trail> GetTrails();
        IEnumerable<Trail> GetTrailsInNationalPark(int npId);
        Trail GetTrail(int Id);
        Trail GetTrail(string Name);
        bool CheckTrailExists(int Id);
        bool CheckTrailExists(string Name);
        bool CreateTrail(Trail Trail);
        bool UpdateTrail(Trail Trail);
        bool DeleteTrail(Trail Trail);
        bool Save();
    }
}
