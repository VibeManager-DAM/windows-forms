﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeManager.Models.Controllers
{
    class Orm
    {
        public static VibeEntities db = new VibeEntities();


        public static string ErrorMessage(SqlException sqlException)
        {
            string message = "";

            switch (sqlException.Number)
            {
                case 2:
                    message = "The server is not operational.";
                    break;
                case 547:
                    message = "The record cannot be deleted because it has related records.";
                    break;
                case 4060:
                    message = "Could not connect to the database.";
                    break;
                case 18456:
                    message = "Login failed.";
                    break;
                case 2601:
                    message = "A record with the same value already exists.";
                    break;
                default:
                    message = sqlException.Number + " - " + sqlException.Message;
                    break;
            }

            return message;
        }
    }
}
