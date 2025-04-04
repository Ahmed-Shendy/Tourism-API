﻿using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using System.Linq;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.Entity.user;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Pagnations;
using Tourism_Api.Services.IServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Tourism_Api.Services;
public class PlaceService(TourismContext Db , HybridCache cache) : IPlaceService
{
    private readonly TourismContext db = Db;
    private readonly HybridCache cache = cache;

   
    public async Task<Result<List<ALLPlaces>>> DisplayAllPlaces(CancellationToken cancellationToken = default)
    {
        string cacheKey = $"AllPlaces";

        
        var Places = await cache.GetOrCreateAsync<List<ALLPlaces>>(cacheKey, async cacheEntery =>
        {
            
            var result =  await db.Places
                .Select(i => new ALLPlaces { Name = i.Name, Photo = i.Photo ?? "" })
                .ToListAsync(cancellationToken);

             return result;
        });

        // use this to remove the cache
        //await cache.RemoveAsync($"AllPlaces");


        return Places is not null && Places.Any()
            ? Result.Success(Places)
            : Result.Failure<List<ALLPlaces>>(PlacesErrors.PlacesNotFound);
    }

    public async Task<PaginatedList<ALLPlaces>> DisplayAllPlacesByPagnation(RequestFilters requestFilters , CancellationToken cancellationToken = default)
    {
        IQueryable<ALLPlaces> query = db.Places.ProjectToType<ALLPlaces>();
                
        if (!string.IsNullOrWhiteSpace(requestFilters.SearchValue))
        {
            query = query.Where(i => i.Name!.Contains(requestFilters.SearchValue));
        }

        // use this to sort data by column or Rate
        query = query.OrderByDescending(i => i.Rate);

        var result = await PaginatedList<ALLPlaces>.CreateAsync(query, requestFilters.PageNumber, requestFilters.PageSize);
        return result!;

    }
    public async Task<PlacesDetails> PlacesDetails(string name  ,CancellationToken cancellationToken = default)
    {
        var result = await db.Places
            .Include(i => i.GovernmentNameNavigation)
            .Include(i => i.Comments).ThenInclude(i => i.User).Include(i => i.TourismNames)    
            .SingleOrDefaultAsync(i => i.Name == name, cancellationToken);
        
        var place = result.Adapt<PlacesDetails>();
        place.comments = result!.Comments.Select(i => new UserComment
        {
            Content = i.Content,
            UserName = i.User.Name,
            UserId = i.UserId,
            id = i.Id,
        }
        ).ToList();

       var AllTourguid =  await db.TourguidAndPlaces
            .Include(i => i.Touguid)
            .Where(i => i.PlaceName == result.Name)
            .ToListAsync(cancellationToken);
        // place.Tourguids = AllTourguid.Adapt<List<Tourguids>>();

        place.Tourguids = AllTourguid.Select(i => new Tourguids
        {
            Id = i.Touguid.Id,
            Name = i.Touguid.Name,
            Email = i.Touguid.Email,    
            Phone = i.Touguid.Phone,
            Gender = i.Touguid.Gender,
            Photo = i.Touguid.Photo ?? "",
        }).ToList();
        place.TypeOfTourism = result.TourismNames.Select(i => i.Name).ToList();

        //place.Tourguids = result.Tourguids!.Select(i => new Tourguids
        //{
        //    Id = i.Id,
        //    Name = i.Name,
        //    Email = i.Email,
        //    Phone = i.Phone,
        //    Photo = i.Photo ?? "",
        //    Gender = i.Gender,
        //}).ToList();

        return place;
    }

}
