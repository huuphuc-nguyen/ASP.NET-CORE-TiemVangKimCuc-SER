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

CREATE TABLE [DM_CHAT_LIEU] (
    [Id] int NOT NULL IDENTITY,
    [ChatLieu] nvarchar(max) NULL,
    [MoTa] nvarchar(max) NULL,
    CONSTRAINT [PK_DM_CHAT_LIEU] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [DM_TRANG_SUC] (
    [Id] int NOT NULL IDENTITY,
    [LoaiTrangSuc] nvarchar(max) NULL,
    [MoTa] nvarchar(max) NULL,
    CONSTRAINT [PK_DM_TRANG_SUC] PRIMARY KEY ([Id])
);
GO

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
GO

CREATE INDEX [IX_SAN_PHAM_ChatLieu_ID] ON [SAN_PHAM] ([ChatLieu_ID]);
GO

CREATE INDEX [IX_SAN_PHAM_LoaiTrangSuc_ID] ON [SAN_PHAM] ([LoaiTrangSuc_ID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230826174426_addTables', N'7.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE SAN_PHAM ADD CONSTRAINT SoftDeleteFilter CHECK (IsDeleted = 0)
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230911132913_SoftDeleteFilter', N'7.0.10');
GO

COMMIT;
GO

