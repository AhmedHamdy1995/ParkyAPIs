using ParkyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository.IRepository
{
    public  interface INationalParkRepository
    {
        IEnumerable<NationalPark> GetNationalParks();
        NationalPark GetNationalPark(int Id);
        NationalPark GetNationalPark(string Name);
        bool CheckNationalParkExists(int Id);
        bool CheckNationalParkExists(string Name);
        bool CreateNationalPark(NationalPark nationalPark);
        bool UpdateNationalPark(NationalPark nationalPark);
        bool DeleteNationalPark(NationalPark nationalPark);
        bool Save();
    }
}
