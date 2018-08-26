using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    public class GroupService : IGropService
    {
        public void Create<Group>(Group item)
        {
            var config = new MapperConfiguration(cfg => 

                cfg.CreateMap<Group, GroupDB>()

                   .ForMember(destination => destination.Address,

              map => map.MapFrom(

                  source => new Address

                  {

                      City = source.City,

                      State = source.State,

                      Country = source.Country

                  }));
            
        }

        public void Update<Group>(Group item)
        {

        }

        public void Delete(int id)
        {

        }
    }
}
