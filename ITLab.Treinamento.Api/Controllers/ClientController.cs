using ITLab.Treinamento.Api.Core.Entities;
using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using ITLab.Treinamento.Api.Core.Infrastructure.Excel;
using ITLab.Treinamento.Api.Core.Upload;
using ITLab.Treinamento.Api.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ITLab.Treinamento.Api.Controllers
{
    public class ClientController : BaseController
    {
        private static class ResponseMessages
        {
            public const string other_client_already_exists = "alerts-app:error.other_client_already_exists";
            public const string client_not_found = "alerts-app:error.client_not_found";
            public const string client_document_required = "alerts-app:error.client_document_required";
        }

        [HttpGet]
        public JsonResult Get(ClientFilterViewModel filter)
        {
            using (var context = new AppDbContext())
            {
                var query = (filter ?? new ClientFilterViewModel { orderByColumn = "Name" }).Apply(context);
                var recordsTotal = query.Count();

                if (!string.IsNullOrWhiteSpace(filter.orderByColumn))
                {
                    query = query.OrderByPropertyName(filter.orderByColumn, filter.orderByAsc)
                                 .Skip(filter.start)
                                 .Take(filter.length);
                }

                var clients = query.Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.CNPJ,
                    c.CPF,
                    c.Telephone,
                    c.Email,
                    c.Active
                }).ToArray();

                if (!string.IsNullOrWhiteSpace(filter.orderByColumn))
                {
                    var data = new
                    {
                        recordsTotal,
                        table = clients
                    };

                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                return Json(clients, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public void Excel(ClientFilterViewModel filter)
        {
            var userId = UserLoggedId;
            var username = UserLoggedUserName;

            Task.Run(() =>
             {
                 try
                 {
                     using (var context = new AppDbContext().WithUsername(username))
                     {
                         var query = filter.Apply(context);
                         var recordsTotal = query.Count();

                         // esse valor deve variar conforme o desempenho da consulta
                         // quanto menor o desempenho da consulta menor deve ser o valor 
                         // para evitar timeout de banco de dados
                         // consultas com poucos registros ou alta performance talvez nem seja necessário
                         // paginação
                         var recordsByPage = 100;
                         var totalPages = (int)Math.Ceiling((double)recordsTotal / (double)recordsByPage);

                         var clients = new List<dynamic>();
                         for (int i = 0; i < totalPages; i++)
                         {
                             clients.AddRange(query.Skip(i).Take(recordsByPage).Select(c => new
                             {
                                 c.Id,
                                 c.Name,
                                 c.CNPJ,
                                 c.CPF,
                                 c.Telephone,
                                 c.Email,
                                 c.Active
                             }).ToArray());
                         }

                         // Generate file
                         // Forma A: Simplesmente exporta o que veio da consulta
                         var excelGenerator = new ExcelGenerator("Customers", UploadHelper.GetDirectoryTempPathOrCreateIfNotExists(), username);
                         var fileName = excelGenerator.Generate(clients);

                         // Forma B: Formata os dados que veio da consulta para exportar
                         //excelGenerator.Generate(clients, (e) =>
                         //{
                         //    e.Cells[1, 1].Value = "Id";
                         //    e.Cells[1, 2].Value = "CPF/CNPJ";
                         //    e.Cells[1, 3].Value = "E-mail";
                         //    e.Cells[1, 4].Value = "Telefone";
                         //    e.Cells[1, 5].Value = "Status";
                         //}, (e, i, data) =>
                         //{
                         //    e.Cells[i, 1].Value = data.Id;
                         //    e.Cells[i, 2].Value = data.CPF ?? data.CNPJ;
                         //    e.Cells[i, 3].Value = data.Email;
                         //    e.Cells[i, 4].Value = data.Telephone;
                         //    e.Cells[i, 5].Value = data.Active ? "Ativo" : "Inativo";
                         //});

                         // Generate notification
                         var user = new User { Id = userId };

                         context.Users.Attach(user);
                         user.NotifyFileGenerated(fileName);

                         context.Configuration.ValidateOnSaveEnabled = false;
                         context.SaveChanges();
                     }
                 }
                 catch (Exception ex)
                 {
                     // TODA ROTINA ASSINCRONA PRECISA TER TRATAMENTO DE ERRO EXPLICITO
                     var logger = LogManager.GetLogger(GetType());
                     logger.Error(ex);
                 }
             });
        }

        public JsonResult GetById(int id)
        {
            using (var context = new AppDbContext())
            {
                var client = context.Clients.Find(id);

                return Json(client, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Post(ClientViewModel newClient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (var context = new AppDbContext())
            {
                var clientExists = context.Clients.Any(p => p.Name == newClient.Name);
                if (clientExists)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return Json(new { message = ResponseMessages.other_client_already_exists }, JsonRequestBehavior.AllowGet);
                }

                var hasDocument = newClient.Type == (int)ClientTypeEnum.Person ? newClient.CPF.HasValue : newClient.CNPJ.HasValue;
                if (!hasDocument)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { message = ResponseMessages.client_document_required }, JsonRequestBehavior.AllowGet);
                }

                var client = new Client
                {
                    Name = newClient.Name.Trim(),
                    Email = newClient.Email.Trim(),
                    Telephone = newClient.Telephone,
                    BirthDate = newClient.BirthDate,
                    Active = newClient.Active,
                    CreationDate = DateTime.Now
                };

                if (newClient.Type == (int)ClientTypeEnum.Person)
                    client.CPF = newClient.CPF;
                else
                    client.CNPJ = newClient.CNPJ;

                context.Clients.Add(client);
                context.SaveChanges();

                return Json(client, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Put(ClientViewModel editClient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (var context = new AppDbContext())
            {
                var client = context.Clients.SingleOrDefault(c => c.Id == editClient.Id);

                if (client == null)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { message = ResponseMessages.client_not_found }, JsonRequestBehavior.AllowGet);
                }

                var otherClientExists = context.Clients.Any(p => p.Name == editClient.Name && p.Id != editClient.Id);
                if (otherClientExists)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return Json(new { message = ResponseMessages.other_client_already_exists }, JsonRequestBehavior.AllowGet);
                }

                client.Id = editClient.Id.Value;
                client.Name = editClient.Name.Trim();
                client.Email = editClient.Email.Trim();
                client.Telephone = editClient.Telephone;
                client.BirthDate = editClient.BirthDate;
                client.Active = editClient.Active;

                if (editClient.Type == (int)ClientTypeEnum.Person)
                    client.CPF = editClient.CPF;
                else
                    client.CNPJ = editClient.CNPJ;

                context.SaveChanges();

                return Json(client, JsonRequestBehavior.AllowGet);
            }
        }
    }
}