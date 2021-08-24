using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        IMailService _mailService = null;
        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public bool SendEmail(MailData mailData)
        {
            return _mailService.SendEmail(mailData);
        }

    }
    }




