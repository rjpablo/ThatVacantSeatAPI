﻿using Dribbly.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dribbly.Model.Courts
{
    [Table("Photos")]
    public class PhotoModel : BaseEntityModel
    {
        public string Url { get; set; }
        public string UploadedById { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}