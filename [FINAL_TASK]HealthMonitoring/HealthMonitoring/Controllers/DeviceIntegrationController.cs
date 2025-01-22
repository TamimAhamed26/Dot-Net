using BAL.DTOs;
using BAL.Services;
using HealthMonitoring.Auth;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/deviceintegration")]
    public class DeviceIntegrationController : ApiController
    {
        [UserAccess]
        [HttpPost]
        [Route("")]
        public HttpResponseMessage SyncDeviceData(DeviceDataDTO deviceDataDto)
        {
            var username = AuthService.GetLoggedInUserName();

            if (string.IsNullOrEmpty(username))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User not authenticated.");
            }

            if (deviceDataDto == null || string.IsNullOrEmpty(deviceDataDto.DeviceName) || string.IsNullOrEmpty(deviceDataDto.MetricType))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid device data.");
            }

            deviceDataDto.Username = username;

            bool isSynced = DeviceIntegrationService.SyncDeviceData(deviceDataDto);

            if (isSynced)
            {
                return Request.CreateResponse(HttpStatusCode.Created, "Device data synced successfully.");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to sync device data.");
            }
        }

        [UserAccess]
        [HttpGet]
        [Route("paged")]
        public HttpResponseMessage GetDeviceDataByPage(int page, int pageSize)
        {
            var username = AuthService.GetLoggedInUserName();

            if (string.IsNullOrEmpty(username))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User not authenticated.");
            }

            var deviceData = DeviceIntegrationService.GetDeviceDataByPage(username, page, pageSize);

            if (deviceData == null || deviceData.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No device data found for the user.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, deviceData);
        }

        // GET: api/devicedata/download?page={page}&pageSize={pageSize}
        [HttpGet]
        [Route("download")]
        public HttpResponseMessage DownloadPaginatedData(int page, int pageSize)
        {
            try
            {
                // Fetch paginated data
                var paginatedData = DeviceIntegrationService.GetPaginatedDeviceData("jane", page, pageSize);

                // Convert data to CSV
                var csvContent = ConvertToCSV(paginatedData);

                // Create HttpResponseMessage with CSV content
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(csvContent, Encoding.UTF8, "text/csv")
                };

                // Set the file download name
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = $"DeviceData_Page{page}.csv"
                };

                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private string ConvertToCSV(List<DeviceDataDTO> data)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id,Username,DeviceName,MetricType,Value,Timestamp");

            foreach (var item in data)
            {
                sb.AppendLine($"{item.Id},{item.Username},{item.DeviceName},{item.MetricType},{item.Value},{item.Timestamp}");
            }

            return sb.ToString();
        }
    }
}

    

