using Application.Common.Interfaces;
using Application.Product.Interface;
using Domain.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Service
{
    public class SizeCRUDOperations : ISizeCRUDOperations
    {
        private readonly IApplicationDBContext _dbContext;
        public SizeCRUDOperations(IApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddSize(Size size)
        {
            _dbContext.Sizes.Add(size);
            _dbContext.SaveChangesAsync();
        }

        public List<Size> GetSizeList(string productCode)
        {
            return _dbContext.Sizes.Where(x=>x.productCode==productCode).ToList();
        }
    }
}
