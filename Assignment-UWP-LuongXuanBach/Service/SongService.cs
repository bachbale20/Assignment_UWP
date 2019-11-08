using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Assignment_UWP.Entity;
using Newtonsoft.Json;

namespace Assignment_UWP.Service
{
    class SongService:ISongService
    {
        public async Task<Song> CreateSong(MemberCredential memberCredential, Song song)
        {
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(song), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(ProjectConfiguration.SONG_CREATE_URL, content);
            var contents = await response.Result.Content.ReadAsStringAsync();
            var responseSong = JsonConvert.DeserializeObject<Song>(contents);
            Debug.WriteLine("Create success with id: " + responseSong.id);
            return song;

        }

        public List<Song> GetAllSong(MemberCredential memberCredential)
        {
             var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(memberCredential.token);
            var response = httpClient.GetAsync(ProjectConfiguration.SONG_GET_ALL).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<List<Song>>(response.Content.ReadAsStringAsync().Result);
        }

        public List<Song> GetMineSongs(MemberCredential memberCredential)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(memberCredential.token);
            var response = httpClient.GetAsync(ProjectConfiguration.SONG_GET_MINE).GetAwaiter().GetResult();
            var listSong = JsonConvert.DeserializeObject<List<Song>>(response.Content.ReadAsStringAsync().Result);
            return listSong;
        }

       
    }
}
