﻿using Dribbly.Authentication.Models;
using Dribbly.Core.Enums.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Dribbly.Core.Extensions.EnumExtensions;

namespace Dribbly.Authentication.Enums
{
    public static class EnumFunctions
    {
        public static IEnumerable<UserPermissionModel> GenerateUserPermissions<TEnumType>(string userId)
        {
            return Enum.GetValues(typeof(TEnumType)).Cast<Enum>()
                            .Select(e => e.GetEnumAttribute<EnumAttribute>())
                            .Select(a => new UserPermissionModel { PermissionId = a.Value, UserId = userId });
        }

        public static IEnumerable<EnumAttribute> GetAllPermissions()
        {
            return GetPermissionByType<CourtPermission>()
                .Union(GetPermissionByType<GamePermission>());
        }

        private static IEnumerable<EnumAttribute> GetPermissionByType<TEnumType>()
        {
            return Enum.GetValues(typeof(TEnumType)).Cast<Enum>()
                            .Select(e => e.GetEnumAttribute<EnumAttribute>());
        }
    }
}
