using Application.Common.Entities;

namespace Application.Common.Interfaces
{
    public interface IExternalAPIService
    {
        ColourEntity GetColourEntity(string apiUrl,Guid colourId);
        SizeEntity GetSizeEntity(string apiUrl,Guid sizeId);
    }
}
