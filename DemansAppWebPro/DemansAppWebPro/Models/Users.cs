﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace DemansAppWebPro.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public string EmergencyPhone { get; set; }

        public string UserName { get; set; }
        
        public string Surname { get; set; }

        public string Phone { get; set; }

        public bool? Sex { get; set; }//true:KADIN - false:ERKEK
        public int Status { get; set; }//1:AKTİF - 0:PASİF

        public string Password { get; set; }

    }
}
