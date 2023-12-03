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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230826174426_addTables')
BEGIN
    CREATE TABLE [DM_CHAT_LIEU] (
        [Id] int NOT NULL IDENTITY,
        [ChatLieu] nvarchar(max) NULL,
        [MoTa] nvarchar(max) NULL,
        CONSTRAINT [PK_DM_CHAT_LIEU] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230826174426_addTables')
BEGIN
    CREATE TABLE [DM_TRANG_SUC] (
        [Id] int NOT NULL IDENTITY,
        [LoaiTrangSuc] nvarchar(max) NULL,
        [MoTa] nvarchar(max) NULL,
        CONSTRAINT [PK_DM_TRANG_SUC] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230826174426_addTables')
BEGIN
    CREATE TABLE [SAN_PHAM] (
        [Id] uniqueidentifier NOT NULL,
        [ImgUrl] nvarchar(max) NULL,
        [TenSanPham] nvarchar(max) NOT NULL,
        [TrongLuongSanPham] real NOT NULL,
        [ChatLieu_ID] int NOT NULL,
        [LoaiTrangSuc_ID] int NOT NULL,
        [MoTa] nvarchar(max) NULL,
        [CreatedDate] datetime NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_SAN_PHAM] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SAN_PHAM_DM_CHAT_LIEU_ChatLieu_ID] FOREIGN KEY ([ChatLieu_ID]) REFERENCES [DM_CHAT_LIEU] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_SAN_PHAM_DM_TRANG_SUC_LoaiTrangSuc_ID] FOREIGN KEY ([LoaiTrangSuc_ID]) REFERENCES [DM_TRANG_SUC] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230826174426_addTables')
BEGIN
    CREATE INDEX [IX_SAN_PHAM_ChatLieu_ID] ON [SAN_PHAM] ([ChatLieu_ID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230826174426_addTables')
BEGIN
    CREATE INDEX [IX_SAN_PHAM_LoaiTrangSuc_ID] ON [SAN_PHAM] ([LoaiTrangSuc_ID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230826174426_addTables')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230826174426_addTables', N'7.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230922035213_changeProperties')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SAN_PHAM]') AND [c].[name] = N'TrongLuongSanPham');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [SAN_PHAM] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [SAN_PHAM] ALTER COLUMN [TrongLuongSanPham] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230922035213_changeProperties')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230922035213_changeProperties', N'7.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230922232744_changeProperties2')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230922232744_changeProperties2', N'7.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230922234321_changeProperties3')
BEGIN
    ALTER TABLE [SAN_PHAM] ADD [UpdatedDate] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230922234321_changeProperties3')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230922234321_changeProperties3', N'7.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231012174937_addTableUser')
BEGIN
    CREATE TABLE [KIMCUC_USER] (
        [TaiKhoan] nvarchar(450) NOT NULL,
        [MatKhau] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NULL,
        [HoTen] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_KIMCUC_USER] PRIMARY KEY ([TaiKhoan])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231012174937_addTableUser')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231012174937_addTableUser', N'7.0.10');
END;
GO

COMMIT;
GO

