﻿using Application.Mail;
using Application.Models;
using Application.Models.Mail;
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




