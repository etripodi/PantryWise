using AutoMapper;
using PantryWise.Pantry;

namespace PantryWise;

public class PantryWiseApplicationAutoMapperProfile : Profile
{
    public PantryWiseApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<PantryItem, PantryItemDto>();
        CreateMap<Product, ProductDto>();
    }
}
