using Services.Dtos;

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
            CreateMap<Domain.Usuarios, RegistroModel>().ReverseMap();
        }

        private void LoginMapper()
        {
            CreateMap<Domain.Usuarios, LoginModel>().ReverseMap();
        }
    }
}
