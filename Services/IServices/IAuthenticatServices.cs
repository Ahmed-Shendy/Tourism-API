﻿using Tourism_Api.Entity.user;

namespace Tourism_Api.Services.IServices;

public interface IAuthenticatServices
{
    Task<Result<UserRespones>> RegisterAsync(UserRequest userRequest, CancellationToken cancellationToken = default);
    Task<Result<UserRespones>> LoginAsync(userLogin userLogin, CancellationToken cancellationToken = default);
    Task<Result<UserRespones>> GetRefreshToken(UserRefreshToken request, CancellationToken cancellationToken = default);
    
}
