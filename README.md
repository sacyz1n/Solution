
# ��� ���̺귯��

- �α� ����
	1. ZLogger (���� �α�)
	2. NotifyLogger (�ڷ��׷� �˸� �α�)
	
- ������ ����ȭ
	1. MemoryPack 

- DB ����
	1. EF Core (��Ű�� ����, Migration)
    2. SQLKata (���� ����, QueryFactory �� ���� ����) 

- �÷��� �α���
	1. Firebase Admin SDK (����)
	2. Firebase Authentication (Ŭ���̾�Ʈ)


# ��Ű�� �ܼ� ������ Migration ��ɾ� ����

Add-Migration -Project Repository -StartupProject Repository -Context Repository.Contexts.GlobalDbContext -OutputDir GlobalDBMigrations -Name ModifyAccountInfoTable
Script-Migration -Idempotent -Context Repository.Contexts.GlobalDbContext -StartupProject Repository -Project Repository -Output "C:\\Migrations\\GlobaldbMigration.sql" 20250629051340_Initial 20250726030817_ModifyAccountInfoTable
Script-Migration -Idempotent -Context Repository.Contexts.GlobalDbContext -StartupProject Repository -Project Repository -Output "C:\\Migrations\\GlobaldbMigration.sql" 0 20250629051340_Initial
Update-Database	-Project Repository -StartupProject Repository -Context Repository.Contexts.GlobalDbContext -Connection "server=127.0.0.1;port=3306;userid=root;password=root;database=globaldb"

Add-Migration -Project Repository -StartupProject Repository -Context Repository.Contexts.GameDbContext -OutputDir GameDBMigrations -Name Initial
Script-Migration -Idempotent -Context Repository.Contexts.GameDbContext -StartupProject Repository -Project Repository -Output "C:\\Migrations\\GamedbMigration.sql" 0 20230522111740_Initial
Update-Database	-Project Repository -StartupProject Repository -Context Repository.Contexts.GameDbContext -Connection "server=127.0.0.1;port=3306;userid=root;password=root;database=gamedb"
