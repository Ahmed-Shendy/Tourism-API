using Mapster;
using MapsterMapper;
namespace Tourism_Api.DTO;
public class ProductMappingConfig : IRegister
{

    public void Register(TypeAdapterConfig config)
    {
        //config.NewConfig<Product, ProductDTO>()
        //    .Map(dest => dest.FullName, src => $"{src.Name} - {src.Price}$")
        //    .Map(dest => dest.PriceWithTax, src => src.Price * 1.14m); // مثال آخر

        //// يمكنك إضافة تحويلات عكسية إذا احتجت
        //config.NewConfig<ProductDTO, Product>();
    }
}
