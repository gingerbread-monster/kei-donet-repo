using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Ex04.DataAccess.Example3.Entities;
using Ex04.DataAccess.Example3.Enums;

namespace Ex04.DataAccess.Example3.Extensions
{
    public static class Extensions
    {
        public static void SeedExample3(this ModelBuilder modelBuilder)
        {
            #region Значения для UserEntity
            modelBuilder.Entity<UserEntity>().HasData(new UserEntity[]
            {
                new()
                {
                    Id = 1,
                    Email = "vasya@mail.com"
                }
            });
            #endregion

            #region Значения для UserPaymentInfoEntity
            modelBuilder.Entity<UserPaymentInfoEntity>().HasData(
                new UserPaymentInfoEntity[]
                {
                    new()
                    {
                        UserId = 1,
                        PaymentSystem = "Платежная система",
                        BankAccountNumber = "Номер банковского счета"
                    }
                });
            #endregion

            #region Значения для RouteEntity
            modelBuilder.Entity<RouteEntity>().HasData(
                new()
                {
                    Id = 1,
                    Name = "Первый маршрут",
                    DateCreated = DateTimeOffset.Parse("01/01/2070 14:00:00",
                        CultureInfo.GetCultureInfo("en-GB").DateTimeFormat)
                },
                new()
                {
                    Id = 2,
                    Name = "Второй маршрут",
                    DateCreated = new(
                        year: 2070, month: 1, day: 1,
                        hour: 14, minute: 0, second: 0, offset: TimeSpan.Zero)
                });
            #endregion

            #region Значения для RouteSubscriberEntity
            modelBuilder.Entity<RouteSubscriberEntity>().HasData(
                new RouteSubscriberEntity[]
                {
                    new()
                    {
                        Id = 1,
                        UserId = 1,
                        RouteId = 1
                    }
                });
            #endregion

            #region Значения для TaskListEntity
            modelBuilder.Entity<TaskListEntity>().HasData(
                new TaskListEntity[]
                {
                    new()
                    {
                        Id = 1,
                        Name = "Список задач 1",
                        RouteId = 1
                    }
                });
            #endregion

            #region Значения для TaskEntity
            modelBuilder.Entity<TaskEntity>().HasData(
                new()
                {
                    Id = 1,
                    TaskListId = 1,
                    Description = "Купить билеты",
                },
                new()
                {
                    Id = 2,
                    TaskListId = 1,
                    PriorityLevel = TaskPriorityLevel.High,
                    Description = "Позвать друзей",
                },
                new()
                {
                    Id = 3,
                    TaskListId = 1,
                    Description = "Покушать перед поездкой",
                    PriorityLevel = TaskPriorityLevel.Urgent,
                    IsCompleted = true
                },
                new()
                {
                    Id = 4,
                    TaskListId = 1,
                    Description = "Собрать необходимые вещи",
                    PriorityLevel = TaskPriorityLevel.Normal
                },
                new()
                {
                    Id = 5,
                    TaskListId = 1,
                    Description = "Задача 5",
                },
                new()
                {
                    Id = 6,
                    TaskListId = 1,
                    Description = "Задача 6",
                },
                new()
                {
                    Id = 7,
                    TaskListId = 1,
                    Description = "Задача 7",
                },
                new()
                {
                    Id = 8,
                    TaskListId = 1,
                    Description = "Задача 8",
                },
                new()
                {
                    Id = 9,
                    TaskListId = 1,
                    Description = "Задача 9",
                },
                new()
                {
                    Id = 10,
                    TaskListId = 1,
                    Description = "Задача 10",
                });
            #endregion

            #region Значения для TaskAssigneeEntity
            modelBuilder.Entity<TaskAssigneeEntity>().HasData(
                new()
                {
                    TaskId = 1,
                    RouteSubscriberId = 1,
                },
                new()
                {
                    TaskId = 2,
                    RouteSubscriberId = 1,
                },
                new()
                {
                    TaskId = 3,
                    RouteSubscriberId = 1,
                },
                new()
                {
                    TaskId = 4,
                    RouteSubscriberId = 1,
                },
                new()
                {
                    TaskId = 5,
                    RouteSubscriberId = 1,
                },
                new()
                {
                    TaskId = 8,
                    RouteSubscriberId = 1,
                },
                new()
                {
                    TaskId = 9,
                    RouteSubscriberId = 1,
                });
            #endregion
        }
    }
}
