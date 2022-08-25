using Amazon.EC2.Model;
using Amazon.EC2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;
using webapiec2.Models;
using Microsoft.AspNetCore.Http;

namespace webapiec2.Controllers
{
    [ApiController]
    [Route("/api/core/ec-2")]
    public class EC2Controller : ControllerBase
    {
        private static string accessKeyID = "XXXX";
        private static string secretKey = "XXXXXX+fh";
        private static string unauthorizedMsg = "Invalid Token, Please Try Again";
        public EC2Controller()
        {
           
        }

        [HttpPost("list-all-servers")]
        public async Task<IActionResult> list(ListRequest request)
        {
            var _EC2Response = new EC2Response();
            try
            {
                if (request == null || string.IsNullOrEmpty(request.AuthToken) || request.AuthToken != "sdawdgbgeae")
                {
                    _EC2Response.IsSuccess = false;
                    _EC2Response.ErrorMessage = unauthorizedMsg;
                    return StatusCode(StatusCodes.Status401Unauthorized, _EC2Response);
                }
                var credentials = new BasicAWSCredentials(accessKeyID, secretKey);
                var eC2Client = new AmazonEC2Client(credentials, Amazon.RegionEndpoint.USEast1);

                var result = await GetInstanceDescriptions(eC2Client);
                _EC2Response.Response = result;
                return Ok(_EC2Response);
            }
            catch (Exception ex)
            {
                _EC2Response.IsSuccess = false;
                _EC2Response.ErrorMessage = ex.Message;
                //_EC2Response.InnnerException = ex.InnerException;

                return StatusCode(StatusCodes.Status500InternalServerError, _EC2Response);

            }
        }

        [HttpPost("start-by-id")]
        public async Task<IActionResult> StartInstanceById(OperationDetails request)
        {
            var _EC2Response = new EC2Response();
            try
            {
                if (request == null || string.IsNullOrEmpty(request.AuthToken) || request.AuthToken != "sdawdgbgeae")
                {
                    _EC2Response.IsSuccess = false;
                    _EC2Response.ErrorMessage = unauthorizedMsg;
                    return StatusCode(StatusCodes.Status401Unauthorized, _EC2Response);
                }
                if (request._instanceIds == null || request._instanceIds.Count < 1)
                {
                    _EC2Response.IsSuccess = false;
                    _EC2Response.ErrorMessage = "Instance Ids are not provided";
                    return StatusCode(StatusCodes.Status400BadRequest, _EC2Response);
                }

                var credentials = new BasicAWSCredentials(accessKeyID, secretKey);
                var eC2Client = new AmazonEC2Client(credentials, Amazon.RegionEndpoint.USEast1);

                var _startInstancesRequest = new StartInstancesRequest
                {
                    InstanceIds = request._instanceIds,
                };

                var response = await eC2Client.StartInstancesAsync(_startInstancesRequest);

                var instanceDetailEC2 = new List<InstanceDetailEC2>();

                if (response.StartingInstances.Count > 0)
                {
                    var instances = response.StartingInstances;
                    instances.ForEach(i =>
                    {
                        instanceDetailEC2.Add(new InstanceDetailEC2
                        {
                            _instanceId = i.InstanceId,
                            _code = i.CurrentState.Code,
                            _state = i.CurrentState.Name
                        });
                    });
                }
                _EC2Response.Response = instanceDetailEC2;
                return Ok(_EC2Response);
            }
            catch (Exception ex)
            {
                _EC2Response.IsSuccess = false;
                _EC2Response.ErrorMessage = ex.Message;
                //_EC2Response.InnnerException = ex.InnerException;

                return StatusCode(StatusCodes.Status500InternalServerError, _EC2Response);
            }
        }

        private async Task<List<InstanceDetailEC2>> GetInstanceDescriptions(AmazonEC2Client client)
        {
            var request = new DescribeInstancesRequest();

            var paginator = client.Paginators.DescribeInstances(request);
            var instanceDetails = new List<InstanceDetailEC2>();
            await foreach (var response in paginator.Responses)
            {
                foreach (var reservation in response.Reservations)
                {
                    foreach (var instance in reservation.Instances)
                    {
                        instanceDetails.Add(
                            new InstanceDetailEC2
                            {
                                _code = instance.State.Code,
                                _instanceId = instance.InstanceId,
                                _state = instance.State.Name,
                                _isntanceName = instance.KeyName,
                                _ipAddress = instance.PublicIpAddress
                            });
                    }
                }
            }
            return instanceDetails;
        }

        [HttpPost("stop-by-id")]
        public async Task<IActionResult> StopInstanceById(OperationDetails request)
        {
            var _EC2Response = new EC2Response();
            try
            {
                if (request == null || string.IsNullOrEmpty(request.AuthToken) || request.AuthToken != "sdawdgbgeae")
                {
                    _EC2Response.IsSuccess = false;
                    _EC2Response.ErrorMessage = unauthorizedMsg;
                    return StatusCode(StatusCodes.Status401Unauthorized, _EC2Response);
                }
                if (request._instanceIds == null || request._instanceIds.Count < 1)
                {
                    _EC2Response.IsSuccess = false;
                    _EC2Response.ErrorMessage = "Instance Ids are not provided";
                    return StatusCode(StatusCodes.Status400BadRequest, _EC2Response);
                }

                var credentials = new BasicAWSCredentials(accessKeyID, secretKey);
                var eC2Client = new AmazonEC2Client(credentials, Amazon.RegionEndpoint.USEast1);

                var _stopInstancesRequest = new StopInstancesRequest
                {
                    InstanceIds = request._instanceIds,
                };

                var response = await eC2Client.StopInstancesAsync(_stopInstancesRequest);

                var instanceDetailEC2 = new List<InstanceDetailEC2>();

                if (response.StoppingInstances.Count > 0)
                {
                    var instances = response.StoppingInstances;
                    instances.ForEach(i =>
                    {
                        instanceDetailEC2.Add(new InstanceDetailEC2
                        {
                            _instanceId = i.InstanceId,
                            _code = i.CurrentState.Code,
                            _state = i.CurrentState.Name
                        });
                    });
                }
                _EC2Response.Response = instanceDetailEC2;
                return Ok(_EC2Response);
            }
            catch (Exception ex)
            {
                _EC2Response.IsSuccess = false;
                _EC2Response.ErrorMessage = ex.Message;
                //_EC2Response.InnnerException = ex.InnerException;

                return StatusCode(StatusCodes.Status500InternalServerError, _EC2Response);
            }
        }

    }
}

