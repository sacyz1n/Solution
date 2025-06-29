using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Repository.Contexts
{
    public static class DbContextExtensions
    {

        public static void FixDatesMaterialized(this ModelBuilder modelBuilder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.IsKeyless)
                {
                    continue;
                }

                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
        }


        /// <summary>
        /// 데이터 저장 시 UTC 가 아닌 경우 UTC 로 저장되도록 수정하는 함수
        /// </summary>
        /// <param name="changeTracker"></param>
        public static void FixDatesSaveChanges(this ChangeTracker changeTracker)
        {
            foreach (var entityEntry in changeTracker.Entries())
            {
                if (entityEntry.State != EntityState.Added && entityEntry.State != EntityState.Modified)
                {
                    continue;
                }

                foreach (var property in entityEntry.Properties)
                {
                    if (property.CurrentValue is DateTime)
                    {
                        DateTime tmp = (DateTime)property.CurrentValue;
                        if (tmp.Kind != DateTimeKind.Utc)
                        {
                            property.CurrentValue = tmp = tmp.ToUniversalTime();
                        }
                    }
                }
            }
        }
    }
}
