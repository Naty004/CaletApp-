using Services.Dtos;
using Infrastructure.Identity; // <-- Aquí está ApplicationUser
using Domain; // Asumiendo que Domain.Milk está bien

namespace Services.Common
{
    public class MProfile : AutoMapper.Profile
    {
        public MProfile()
        {
            MilkMapper();
            RegistroMapper();
            LoginMapper();
        }

        private void MilkMapper()
        {
            CreateMap<Domain.Milk, MilkModel>()
              .ReverseMap();
        }

        private void RegistroMapper()
        {
            CreateMap<ApplicationUser, RegistroModel>().ReverseMap();
        }

        private void LoginMapper()
        {
            CreateMap<ApplicationUser, LoginModel>().ReverseMap();
        }
    }
}

