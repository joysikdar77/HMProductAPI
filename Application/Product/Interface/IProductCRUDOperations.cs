using Application.Common.ViewModel;
using System.Net;

namespace Application.Product.Interface
{
    public interface IProductCRUDOperations
    {
        StatusVM AddProduct(ProductVM product);
        string GetProduct(Guid productId);
    }
}
