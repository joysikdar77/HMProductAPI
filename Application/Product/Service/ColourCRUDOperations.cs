using Application.Common.Interfaces;
using Application.Product.Interface;
using Domain.Master;

namespace Application.Product.Service
{
    public class ColourCRUDOperations : IColourCRUDOperations
    {
        private readonly IApplicationDBContext _dbContext;
        public ColourCRUDOperations(IApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public void AddColour(Colour colour)
        {
            _dbContext.Colours.Add(colour);
            _dbContext.SaveChangesAsync();
        }

        public List<Colour> GetColour(string productCode)
        {
            return _dbContext.Colours.Where(x => x.productCode == productCode).ToList();
        }
    }
}
