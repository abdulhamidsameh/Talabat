using AdminDashboard.Models;
using AutoMapper;
using Talabat.Core.Entities.Product;

namespace AdminDashboard.Helpers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product,ProductViewModel>().ReverseMap(); 
        }
    }
}
