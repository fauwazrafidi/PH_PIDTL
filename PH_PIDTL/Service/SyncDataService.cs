using Newtonsoft.Json.Linq;

using SHARED;

namespace Polynic.Service
{
    public class SyncDataService
    {
        private readonly HttpClient _httpClient;
        private readonly List<PH_PIDTL> _pH_PIDTL = new List<PH_PIDTL>();

        public SyncDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task RetrieveData()
        {
            Console.WriteLine("Testing");
            try
            {
                var retrieveUrl = "/api/RetrieveData/GetData";

                string[] targetFields = {
                    "REMARK2", "ITEMCODE","DESCRIPTION",
                    "DESCRIPTION2","BATCH","LOCATION",
                    "QTY","UOM"
                };



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving or processing data: {ex.Message}");
            }

        }
        private async Task ProcessApiData(string apiUrl, string[] targetFields)
        {
            var response = await _httpClient.GetAsync(apiUrl);

            if (response != null && response.IsSuccessStatusCode)
            {
                var apiData = await response.Content.ReadAsStringAsync();
                var jsonArray = JArray.Parse(apiData);

                var schemaObject = jsonArray.FirstOrDefault(obj => obj["__rowType"].ToString() == "META");
                var dataObjects = jsonArray.Where(obj => obj["__rowType"].ToString() == "DATA");
                var schemaFields = schemaObject["data"].Select(field => field["name"].ToString()).ToList();

               
            }
            else
            {
                Console.WriteLine($"Failed to retrieve data from the API. Status code: {response?.StatusCode}");
            }
        }

    }
}
