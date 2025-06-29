using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contexts
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            configurationBuilder.Conventions.Add(_ => new CustomIndexNamingConvention());
        }

        /// <summary>
        /// EF7 에서 Add-Migration 할 때 Index 의 네이밍이 중복될 경우 자동으로 뒤에 0 부터 숫자를 넣어준다.
        /// 숫자가 뒤에 붙지 않도록 강제로 인덱스 이름을 설정한다.
        /// </summary>
        private class CustomIndexNamingConvention : IModelFinalizingConvention
        {
            public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
            {
                foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
                {
                    foreach (var index in entityType.GetIndexes())
                    {
                        index.SetDatabaseName(index.Name);
                    }
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseMySql(ConnectionString.s_ServerVersion, (Microsoft.EntityFrameworkCore.Infrastructure.MySqlDbContextOptionsBuilder builder) => {
                    builder.CommandTimeout(999999);
                });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var memberInfo = property.PropertyInfo ?? (MemberInfo)property.FieldInfo;
                    if (memberInfo == null) continue;
                    var defaultValue = Attribute.GetCustomAttribute(memberInfo, typeof(DefaultValueAttribute)) as DefaultValueAttribute;
                    if (defaultValue == null) continue;
                    if (defaultValue.Value == null) property.SetDefaultValueSql("null");
                    else property.SetDefaultValue(defaultValue.Value);
                }
            }

            modelBuilder.FixDatesMaterialized();
        }

        public override int SaveChanges()
        {
            this.ChangeTracker.FixDatesSaveChanges();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.ChangeTracker.FixDatesSaveChanges();

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
