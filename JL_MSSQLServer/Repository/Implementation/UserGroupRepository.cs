﻿using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class UserGroupRepository : RepositoryBase<UserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
