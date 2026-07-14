IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Companies] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [Phone] nvarchar(max) NULL,
    [Tax] nvarchar(max) NULL,
    [Status] tinyint NOT NULL DEFAULT CAST(1 AS tinyint),
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Companies] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Departments] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(50) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [ParentId] int NULL,
    [Status] tinyint NOT NULL DEFAULT CAST(1 AS tinyint),
    [Description] nvarchar(200) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((sysdatetime())),
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(200) NULL,
    [UpdatedBy] nvarchar(200) NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Departments_Parent] FOREIGN KEY ([ParentId]) REFERENCES [Departments] ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [DepartmentId] int NOT NULL,
    [EmployeeId] nvarchar(50) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Avatar] nvarchar(500) NULL,
    [Password] nvarchar(200) NOT NULL,
    [VerificationToken] nvarchar(255) NULL,
    [EmailVerifiedAt] datetime2 NULL,
    [LastLoginAt] datetime2 NULL,
    [RefreshToken] nvarchar(500) NULL,
    [RefreshTokenExpiresAt] datetime2 NULL,
    [LockCount] tinyint NOT NULL,
    [LockedAt] datetime2 NULL,
    [Status] tinyint NOT NULL DEFAULT CAST(1 AS tinyint),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((sysdatetime())),
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    [CreatedBy] nvarchar(200) NULL,
    [UpdatedBy] nvarchar(200) NULL,
    CONSTRAINT [PK__Users__3214EC07C0381A25] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id])
);
GO

CREATE INDEX [IX_Departments_ParentDepartmentId] ON [Departments] ([ParentId]);
GO

CREATE UNIQUE INDEX [UQ__Departme__A25C5AA7256E0E11] ON [Departments] ([Code]);
GO

CREATE INDEX [IX_Users_DepartmentId] ON [Users] ([DepartmentId]);
GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
GO

CREATE UNIQUE INDEX [UQ__Users__7AD04F10D9D27159] ON [Users] ([EmployeeId]);
GO

CREATE UNIQUE INDEX [UQ__Users__A9D105349A5F0144] ON [Users] ([Email]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260713041059_Create_Company_User_Deparment_Table', N'8.0.20');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[Departments].[UQ__Departme__A25C5AA7256E0E11]', N'IX_Departments_Code', N'INDEX';
GO

ALTER TABLE [Departments] ADD [CompanyId] int NOT NULL DEFAULT 0;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Companies]') AND [c].[name] = N'Code');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Companies] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Companies] ALTER COLUMN [Code] nvarchar(450) NOT NULL;
GO

CREATE INDEX [IX_Departments_CompanyId] ON [Departments] ([CompanyId]);
GO

CREATE UNIQUE INDEX [IX_Companies_Code] ON [Companies] ([Code]);
GO

ALTER TABLE [Departments] ADD CONSTRAINT [FK_Departments_Companies] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([Id]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260713113440_AddCompanyIdToDepartment', N'8.0.20');
GO

COMMIT;
GO

