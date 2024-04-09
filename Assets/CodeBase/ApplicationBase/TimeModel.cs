using System;
using System.Threading.Tasks;
using CodeBase.Extentions.Time;
using UnityEngine;

namespace CodeBase.ApplicationBase
{
    public class TimeModel
    {
        public long? serverTime;

        private GetTimeFromTimeAPI _getTimeFromTimeAPI;

        public TimeModel(GetTimeFromTimeAPI getTimeFromTimeAPI)
        {
            _getTimeFromTimeAPI = getTimeFromTimeAPI;
        }
        
        public long GetUnixTimeSeconds()
        {
            if (serverTime == null)
                throw new Exception("Server time is not initialized");
            return (long)serverTime + (long)Time.realtimeSinceStartup;
        }
        
        public async Task<bool> TryToRefreshServerTime()
        {
            try
            {
                serverTime =
                    await _getTimeFromTimeAPI.ExecuteTimeApi() ??
                    await _getTimeFromTimeAPI.ExecuteWorldTimeApi();
                if (serverTime == null) return false;

                serverTime -= (int)Time.realtimeSinceStartup;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}