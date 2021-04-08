using BaseProject.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.DAO.Service
{
    public interface IEmailService
    {
        Task SendTestEmail(UserEmailOptions userEmailOptions);

        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);

        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);

        UserEmailOptions ReturnConfirmationBody(UserEmailOptions userEmailOptions);

        UserEmailOptions ReturnForgotPasswordBody(UserEmailOptions userEmailOptions);

    }
}
