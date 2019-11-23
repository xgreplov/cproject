using System;
using System.Threading.Tasks;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;

namespace DemoEshop.BusinessLayer.Services.RateSongs
{
    public interface IRateSongService
    {
        void RateSong(RateSongDto songRateDTO);

        Task<RateSongDto> GetRatingAccordingToUserIdAsync(Guid userId);
    }
}