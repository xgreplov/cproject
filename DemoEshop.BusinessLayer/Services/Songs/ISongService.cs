﻿using System;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;

namespace DemoEshop.BusinessLayer.Services.Songs
{
    public interface ISongService
    {
        /// <summary>
        /// Gets products according to given filter
        /// </summary>
        /// <param name="filter">The products filter</param>
        /// <returns>Filtered results</returns>
        Task<QueryResultDto<SongDto, SongFilterDto>> ListSongsAsync(SongFilterDto filter);

        /// <summary>
        /// Gets DTO representing the entity according to ID
        /// </summary>
        /// <param name="entityId">entity ID</param>
        /// <param name="withIncludes">include all entity complex types</param>
        /// <returns>The DTO representing the entity</returns>
        Task<SongDto> GetAsync(Guid entityId, bool withIncludes = true);

        /// <summary>
        /// Gets product with given name
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>product with given name</returns>
        Task<SongDto> GetSongByNameAsync(string name);

        /// <summary>
        /// Creates new entity
        /// </summary>
        /// <param name="entityDto">entity details</param>
        Guid Create(SongDto entityDto);

        /// <summary>
        /// Updates entity
        /// </summary>
        /// <param name="entityDto">entity details</param>
        Task Update(SongDto entityDto);

        /// <summary>
        /// Deletes entity with given Id
        /// </summary>
        /// <param name="entityId">Id of the entity to delete</param>
        void Delete(Guid entityId);

        /// <summary>
        /// Gets all DTOs (for given type)
        /// </summary>
        /// <returns>all available dtos (for given type)</returns>
        Task<QueryResultDto<SongDto, SongFilterDto>> ListAllAsync();
    }
}