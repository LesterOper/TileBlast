

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CodeBase.Extentions.Time
{
    public class GetTimeFromTimeAPI
    {
        public async Task<long?> ExecuteTimeApi()
        {
            try
            {
                var request = UnityWebRequest.Get("https://timeapi.io/api/Time/current/zone?timeZone=Universal");
                await request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success) return null;
                var server = JsonUtility.FromJson<Response>(request.downloadHandler.text);
                return DateTime.Parse(server.dateTime).ToUnixTimeSeconds();
            }
            catch
            {
                return null;
            }
        }
        
        public async Task<long?> ExecuteWorldTimeApi()
        {
            try
            {
                var request = UnityWebRequest.Get("https://worldtimeapi.org/api/timezone/Etc/UTC");
                await request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success) return null;
                var server = JsonUtility.FromJson<Response>(request.downloadHandler.text);
                return server.unixtime;
            }
            catch
            {
                return null;
            }
        }

        private struct Response
        {
            public string dateTime;
            public long unixtime;
        }
        
        
    }
}
