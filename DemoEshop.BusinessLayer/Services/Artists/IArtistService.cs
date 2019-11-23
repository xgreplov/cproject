using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;

namespace DemoEshop.BusinessLayer.Services.Artists
{
    interface IArtistService
    {
        Task<ArtistDto> GetArtistAccordingToNameAsync(string name);

        Task<List<ArtistDto>> GetArtistsAccordingToCountryOfOriginAsync(string countryOfOrigin);
    }
}
