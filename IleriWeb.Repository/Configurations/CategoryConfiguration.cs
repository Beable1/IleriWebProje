﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IleriWeb.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Repository.Configurations
{
    internal class CategoryConfiguration:IEntityTypeConfiguration<Category>
	{

		public void Configure(EntityTypeBuilder<Category> builder) { 
		
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).UseIdentityColumn();
			builder.Property(x =>x.Name).IsRequired().HasMaxLength(30);
		}
	}
}
