﻿using Tourism_Api.Entity.Admin;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Programs;
using Tourism_Api.Entity.Tourguid;

namespace Tourism_Api.Services.IServices;

public interface IAdminServices
{
    Task<Result<PlacesDetails>> AddPlace(AddPlaceRequest request, CancellationToken cancellationToken = default);
    Task<Result<AllNotActiveTourguid>> NotActiveTourguid(CancellationToken cancellationToken = default);
    Task<Result> ActiveTourguid(string id, CancellationToken cancellationToken = default);
    Task<Result> UpdatePlace(string PlaceName, UpdatePlaceRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeletePlace(string name, CancellationToken cancellationToken = default);
    Task<Result> DeleteTourguid(string id, CancellationToken cancellationToken = default);
    //Task<Result> AddTourguidPlace(string tourguidId, string placeName, CancellationToken cancellationToken = default);
    Task<Result> DeleteTourguidPlace(string tourguidId, string placeName, CancellationToken cancellationToken = default);
    Task<Result<Dashboard>> DisplayAll(CancellationToken cancellationToken = default);
    Task<Result<TransferRequests>> TransferRequest(CancellationToken cancellationToken = default);
    Task<Result> TransferRequestDecline(string tourguidId, CancellationToken cancellationToken = default);
    Task<Result<List<string>>> DisplayAllPrograms(CancellationToken cancellationToken = default);
    Task<Result> MoveTourguidAccapt(TourguidTrips tourguidTrips, CancellationToken cancellationToken = default);
    Task<Result<ContactUsProblemsResponse>> GetAllContactUsProblems(CancellationToken cancellationToken = default);
    Task<Result> ReplyToContactUs(AdminReplyContactUsRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteContactUsProblem(int problemId, CancellationToken cancellationToken = default);
    Task<Result<ContactUsProblemsResponse>> GetAllResolvedContactUsProblems(CancellationToken cancellationToken = default);
    Task<Result> AddTrip(AddTripRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateTrip(string tripName, UpdateTripRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteTrip(string tripName, CancellationToken cancellationToken = default);
    Task<Result> DeleteAnyComment(int commentId, CancellationToken cancellationToken = default);
}
