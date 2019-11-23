using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure.Query;

namespace DemoEshop.BusinessLayer.Config
{
    public class MappingConfig
    {
        public static void ConfigureMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<Album, AlbumDto>().ReverseMap();
            config.CreateMap<Artist, ArtistDto>().ReverseMap();
            config.CreateMap<RateSong, RateSongDto>().ReverseMap();
            config.CreateMap<Song, SongDto>().ReverseMap();
            config.CreateMap<User, UserDto>().ReverseMap();
            config.CreateMap<User, UserCreateDto>().ReverseMap();
            config.CreateMap<QueryResult<Album>, QueryResultDto <AlbumDto, AlbumFilterDto>>();
            config.CreateMap<QueryResult<Artist>, QueryResultDto<ArtistDto, ArtistFilterDto>>();
            config.CreateMap<QueryResult<RateSong>, QueryResultDto<RateSongDto, RateSongFilterDto>>();
            config.CreateMap<QueryResult<Song>, QueryResultDto<SongDto, SongFilterDto>>();            
            config.CreateMap<QueryResult<User>, QueryResultDto<UserDto, UserFilterDto>>();
        }

    }
}
