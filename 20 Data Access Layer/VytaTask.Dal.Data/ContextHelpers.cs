﻿using System.Data.Entity;
using VytaTask.Business.Contracts.DataAccess;

namespace VytaTask.Dal.Data
{
    public static class ContextHelpers
    {
        //Only use with short lived contexts 
        public static void ApplyStateChanges(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries<IObjectWithState>())
            {
                IObjectWithState stateInfo = entry.Entity;
                entry.State = StateHelpers.ConvertState(stateInfo.State);
            }
        }
    }
}
