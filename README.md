
# 사용 라이브러리

- 로그 관련
	1. ZLogger (파일 로그)
	2. NotifyLogger (텔레그램 알림 로그)
	
- 데이터 직렬화
	1. MemoryPack 

- DB 관련
	1. EF Core (스키마 관리, Migration)
    2. SQLKata (쿼리 빌더, QueryFactory 로 쿼리 실행) 

- 플랫폼 로그인
	1. Firebase Admin SDK (서버)
	2. Firebase Authentication (클라이언트)


# 패키지 콘솔 관리자 Migration 명령어 샘플

Add-Migration -Project Repository -StartupProject Repository -Context Repository.Contexts.GlobalDbContext -OutputDir GlobalDBMigrations -Name ModifyAccountInfoTable
Script-Migration -Idempotent -Context Repository.Contexts.GlobalDbContext -StartupProject Repository -Project Repository -Output "C:\\Migrations\\GlobaldbMigration.sql" 20250629051340_Initial 20250726030817_ModifyAccountInfoTable
Script-Migration -Idempotent -Context Repository.Contexts.GlobalDbContext -StartupProject Repository -Project Repository -Output "C:\\Migrations\\GlobaldbMigration.sql" 0 20250629051340_Initial
Update-Database	-Project Repository -StartupProject Repository -Context Repository.Contexts.GlobalDbContext -Connection "server=127.0.0.1;port=3306;userid=root;password=root;database=globaldb"

Add-Migration -Project Repository -StartupProject Repository -Context Repository.Contexts.GameDbContext -OutputDir GameDBMigrations -Name Initial
Script-Migration -Idempotent -Context Repository.Contexts.GameDbContext -StartupProject Repository -Project Repository -Output "C:\\Migrations\\GamedbMigration.sql" 0 20230522111740_Initial
Update-Database	-Project Repository -StartupProject Repository -Context Repository.Contexts.GameDbContext -Connection "server=127.0.0.1;port=3306;userid=root;password=root;database=gamedb"
