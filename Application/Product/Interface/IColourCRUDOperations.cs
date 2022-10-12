using Domain.Master;

namespace Application.Product.Interface
{
    public interface IColourCRUDOperations
    {
        void AddColour(Colour colour);
        List<Colour> GetColour(string productCode);
    }
}
