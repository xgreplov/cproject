using System;
using System.Threading.Tasks;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;

namespace DemoEshop.BusinessLayer.Services.RateSongs
{
    public interface IRateSongService
    {
        void RateSong(RateSongDto songRateDTO);

        Task<RateSongDto> GetRatingAccordingToUserIdAsync(Guid userId);
        
        Task<RateSongDto> GetAsync(Guid entityId, bool withIncludes = true);

        Guid Create(RateSongDto entityDto);

        void Delete(Guid entityId);

        Task<QueryResultDto<RateSongDto, RateSongFilterDto>> ListAllAsync();
    }
}