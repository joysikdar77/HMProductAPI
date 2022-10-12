using Domain.Master;

namespace Application.Product.Interface
{
    public interface ISizeCRUDOperations
    {
        void AddSize(Size size);
        List<Size> GetSizeList(string productCode);
    }
}
