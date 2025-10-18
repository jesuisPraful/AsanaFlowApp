using System;
using System.Collections.Concurrent;
using AsanaFlowDataAccessLayer.Models;

namespace AsanaFlowWebServices.Services
{
    public static class TempStore
    {
        // Stores OTPs for emails
        public static ConcurrentDictionary<string, (string Otp, DateTime Expiry)> EmailOtps
            = new ConcurrentDictionary<string, (string, DateTime)>();

        // Temporarily store user data before email verification
        public static ConcurrentDictionary<string, User> PendingUsers
            = new ConcurrentDictionary<string, User>();

     
    }
}
