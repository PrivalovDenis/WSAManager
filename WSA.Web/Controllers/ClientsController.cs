using AutoMapper;
using WSAManager.Core.Entities;
using WSAManager.Core.Services;
using WSAManager.Dto.Dtos;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Swashbuckle;
using Swashbuckle.Swagger.Annotations;
using System.Web.Http.Description;
using System.Net;

namespace WSAManager.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Clients")]
    public class ClientsController : ApiController
    {
        private readonly IClientService _clientService;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="service"></param>
        public ClientsController(IClientService service)
        {
            _clientService = service;
        }

        /// <summary>
        /// Get Client List
        /// </summary>
        /// <returns>Returns List typeof(ClientDto)</returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetClients()
        {
            try
            {
                var clients = await _clientService.GetAllAsync();

                var clientsDto = new List<ClientDto>();

                Mapper.Map(clients, clientsDto);

                return Ok(clientsDto);
            }
            catch (Exception ex)
            {
                //TODO: Log something here
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get CLient By Client Id
        /// </summary>
        /// <param name="id">Client Identifier</param>
        /// <returns>Retruns instance typeof(ClientDto)</returns>
        [Route("ById/{id:int}")]
        public async Task<IHttpActionResult> GetClient(int id)
        {
            try
            {
                Client prod = await _clientService.GetByIdAsync(id);

                if (prod == null)
                {
                    return NotFound();
                }

                var clientDto = new ClientDto();

                Mapper.Map(prod, clientDto);

                return Ok(clientDto);
            }
            catch (Exception ex)
            {
                //TODO: Log something here
                return InternalServerError();
            }
        }

        /// <summary>
        /// Update exiting Client By Id
        /// </summary>
        /// <param name="id">Client Identifier</param>
        /// <param name="clientDto">Clint entity</param>
        /// <returns>Returns instance typeof(ClientDto)</returns>
        [HttpPut]
        public async Task<IHttpActionResult> PutClient(int id, ClientDto clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clientDto.Id)
            {
                return BadRequest();
            }

            try
            {
                Client client = await _clientService.GetByIdAsync(id);

                client.FirstName = clientDto.FirstName;
                client.LastName = clientDto.LastName;

                await _clientService.UpdateAsync(client);
              
                return Ok(clientDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Create new Client
        /// </summary>
        /// <param name="clientDto">Client entity</param>
        /// <returns>Returns instance typeof(ClientDto)</returns>
        [HttpPost]
        public async Task<IHttpActionResult> PostClient(ClientDto clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try {
                Client client = new Client();

                client.FirstName = clientDto.FirstName;
                client.LastName = clientDto.LastName;

                client = await _clientService.AddAsync(client);

                clientDto.Id = client.Id;

                return CreatedAtRoute("ApiRoute", new { id = clientDto.Id }, clientDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                //Log something her...
                return InternalServerError();
            }
        }

        /// <summary>
        /// Delete Client By id
        /// </summary>
        /// <param name="id">Client Identifier</param>
        /// <returns>Returns 200 if Deleted</returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            try
            {
                Client client = await _clientService.GetByIdAsync(id);
                if (client == null)
                {
                    return NotFound();
                }

                await _clientService.DeleteAsync(client);

                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                //Log something here..
                return InternalServerError();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _clientService.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IsClientExists(int id)
        {
            return _clientService.GetByIdAsync(id) != null;
        }
        
    }
}