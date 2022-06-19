﻿using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;

namespace JL_Service.Abstraction.User
{
    public interface IGetRemoteAccessListAsyncPoint : IPoint<int, GetRemoteAccessListResponse>
    {
    }
}
