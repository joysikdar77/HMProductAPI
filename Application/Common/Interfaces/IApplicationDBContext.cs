using Domain.Master;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDBContext
    {
        DbSet<Products> Products { get; set; }
        DbSet<Colour> Colours { get; set; }
        DbSet<Size> Sizes { get; set; }
        Task<int> SaveChangesAsync();
    }
}
