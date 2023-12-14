using AutoMapper;
using EcomTest.Application.Dtos;
using EcomTest.Domain.DomainEntities;
//using EcomTest.Application.Dtos;
using System.Reflection;


namespace EcomTest.Application
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            DiscoverAndManageAutoMapperMappings();
        }

        /// <summary>
        /// Auto discovers and maps betwen entity and dtos across the different layers, for use with AutoMapper - standard Entity / Dto only, requires naming convention - dto to start with ClassNameDto 9e.g. Stock - StockDto, StockDtoCreateOrder etc
        /// </summary>
        private void DiscoverAndManageAutoMapperMappings()
        {
            var dtoTypes = GetTypesInNamespace("EcomTest.Application", "Dtos")
                .Where(type => type.IsClass && type.Name.Contains("Dto"));

            foreach (var dtoType in dtoTypes)
            {
                var classNameWithoutDto = dtoType.Name.Split("Dto")[0];
                var correspondingEntityType = GetTypesInNamespace("EcomTest.Domain", "DomainEntities")
                    .FirstOrDefault(type => type.Name == classNameWithoutDto);

                if (correspondingEntityType != null)
                {
                    CreateMap(correspondingEntityType, dtoType);
                    CreateMap(dtoType, correspondingEntityType);
                }
            }
        }

        private static IEnumerable<Type> GetTypesInNamespace(string projectName, string directoryName)
        {
            return Assembly.Load(projectName)
                .GetTypes()
                .Where(type => type.Namespace != null && type.Namespace.Contains(directoryName));
        }
    }
}
