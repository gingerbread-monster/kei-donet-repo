using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Ex02.Services.Interfaces;
using Ex02.Models;

namespace Ex02.Pages
{
    #region ��������� ��������� �������� (����������)
    public class Example4Model : PageModel
    {
        readonly IEnumerable<IUserNotifierService> _notifiers;
        readonly IOptions<UserNotificationSettings> _notificationSettings;

        public Example4Model(
            IEnumerable<IUserNotifierService> notifiers,
            IOptions<UserNotificationSettings> notificationSettings)
        {
            _notifiers = notifiers;
            _notificationSettings = notificationSettings;
        }

        public void OnGet()
        {
            var notificationService = _notifiers
                .FirstOrDefault(service =>
                    service.ServiceName == _notificationSettings.Value.CurrentUserNotificationService);

            string result = notificationService?.NotifyUser("����");

            ViewData["notificationMessage"] = result;
        }
    }
    #endregion

    #region ��������� ������������ ���������� (�� ������ ����������� � ������ ����������)
    //public class Example4Model : PageModel
    //{
    //    readonly IUserNotifierService _userNotifierService;

    //    public Example4Model(IUserNotifierService userNotifierService)
    //    {
    //        _userNotifierService = userNotifierService;
    //    }

    //    public void OnGet()
    //    {
    //        string result = _userNotifierService.NotifyUser("����");

    //        ViewData["notificationMessage"] = result;
    //    }
    //}
    #endregion
}
