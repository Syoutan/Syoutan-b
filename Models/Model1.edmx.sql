
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/12/2017 14:59:41
-- Generated from EDMX file: D:\take\Documents\Visual Studio 2017\Projects\WebApplicationTest3\WebApplicationTest3\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ProductManage1];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_buy_product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[buy] DROP CONSTRAINT [FK_buy_product];
GO
IF OBJECT_ID(N'[dbo].[FK_buy_supplier]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[buy] DROP CONSTRAINT [FK_buy_supplier];
GO
IF OBJECT_ID(N'[dbo].[FK_product_category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[product] DROP CONSTRAINT [FK_product_category];
GO
IF OBJECT_ID(N'[dbo].[FK_sale_customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[sale] DROP CONSTRAINT [FK_sale_customer];
GO
IF OBJECT_ID(N'[dbo].[FK_product_maker]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[product] DROP CONSTRAINT [FK_product_maker];
GO
IF OBJECT_ID(N'[dbo].[FK_sale_product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[sale] DROP CONSTRAINT [FK_sale_product];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[buy]', 'U') IS NOT NULL
    DROP TABLE [dbo].[buy];
GO
IF OBJECT_ID(N'[dbo].[category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[category];
GO
IF OBJECT_ID(N'[dbo].[customer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[customer];
GO
IF OBJECT_ID(N'[dbo].[maker]', 'U') IS NOT NULL
    DROP TABLE [dbo].[maker];
GO
IF OBJECT_ID(N'[dbo].[product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[product];
GO
IF OBJECT_ID(N'[dbo].[sale]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sale];
GO
IF OBJECT_ID(N'[dbo].[supplier]', 'U') IS NOT NULL
    DROP TABLE [dbo].[supplier];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[Buy_View_1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Buy_View_1];
GO
IF OBJECT_ID(N'[dbo].[Product_View_1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product_View_1];
GO
IF OBJECT_ID(N'[dbo].[Sale_View_1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sale_View_1];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'buy'
CREATE TABLE [dbo].[buy] (
    [id] int IDENTITY(1,1) NOT NULL,
    [product_id] int  NOT NULL,
    [supplier_id] int  NOT NULL,
    [value] decimal(19,4)  NOT NULL,
    [qnt] int  NOT NULL,
    [date] datetime  NOT NULL
);
GO

-- Creating table 'category'
CREATE TABLE [dbo].[category] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'customer'
CREATE TABLE [dbo].[customer] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(100)  NOT NULL,
    [address] nvarchar(100)  NULL,
    [mailaddress] nchar(100)  NULL
);
GO

-- Creating table 'maker'
CREATE TABLE [dbo].[maker] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(200)  NOT NULL,
    [address1] nvarchar(100)  NULL,
    [TEL] int  NULL,
    [PostNo] int  NULL
);
GO

-- Creating table 'product'
CREATE TABLE [dbo].[product] (
    [id] int IDENTITY(1,1) NOT NULL,
    [pcode] nvarchar(50)  NOT NULL,
    [name] nvarchar(100)  NOT NULL,
    [value] decimal(19,4)  NULL,
    [stok] int  NOT NULL,
    [maker_id] int  NOT NULL,
    [category_id] int  NULL
);
GO

-- Creating table 'sale'
CREATE TABLE [dbo].[sale] (
    [id] int IDENTITY(1,1) NOT NULL,
    [product_id] int  NOT NULL,
    [customer_id] int  NOT NULL,
    [value] decimal(19,4)  NOT NULL,
    [qnt] int  NOT NULL,
    [date] datetime  NOT NULL
);
GO

-- Creating table 'supplier'
CREATE TABLE [dbo].[supplier] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(100)  NOT NULL,
    [address] nvarchar(100)  NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Buy_View_1'
CREATE TABLE [dbo].[Buy_View_1] (
    [id] int  NOT NULL,
    [value] decimal(19,4)  NOT NULL,
    [qnt] int  NOT NULL,
    [date] datetime  NOT NULL,
    [pcode] nvarchar(50)  NOT NULL,
    [name] nvarchar(100)  NOT NULL,
    [stok] int  NOT NULL,
    [Expr1] nvarchar(100)  NOT NULL,
    [Expr2] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'Product_View_1'
CREATE TABLE [dbo].[Product_View_1] (
    [id] int  NOT NULL,
    [name] nvarchar(100)  NOT NULL,
    [value] decimal(19,4)  NULL,
    [stok] int  NOT NULL,
    [pcode] nvarchar(50)  NOT NULL,
    [Expr1] nvarchar(200)  NOT NULL,
    [Expr2] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'Sale_View_1'
CREATE TABLE [dbo].[Sale_View_1] (
    [id] int  NOT NULL,
    [value] decimal(19,4)  NOT NULL,
    [qnt] int  NOT NULL,
    [date] binary(8)  NOT NULL,
    [pcode] nvarchar(50)  NOT NULL,
    [name] nvarchar(100)  NOT NULL,
    [Expr1] nvarchar(200)  NOT NULL,
    [stok] int  NOT NULL,
    [Expr2] nvarchar(100)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'buy'
ALTER TABLE [dbo].[buy]
ADD CONSTRAINT [PK_buy]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'category'
ALTER TABLE [dbo].[category]
ADD CONSTRAINT [PK_category]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'customer'
ALTER TABLE [dbo].[customer]
ADD CONSTRAINT [PK_customer]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'maker'
ALTER TABLE [dbo].[maker]
ADD CONSTRAINT [PK_maker]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'product'
ALTER TABLE [dbo].[product]
ADD CONSTRAINT [PK_product]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'sale'
ALTER TABLE [dbo].[sale]
ADD CONSTRAINT [PK_sale]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'supplier'
ALTER TABLE [dbo].[supplier]
ADD CONSTRAINT [PK_supplier]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [id], [value], [qnt], [date], [pcode], [name], [stok], [Expr1], [Expr2] in table 'Buy_View_1'
ALTER TABLE [dbo].[Buy_View_1]
ADD CONSTRAINT [PK_Buy_View_1]
    PRIMARY KEY CLUSTERED ([id], [value], [qnt], [date], [pcode], [name], [stok], [Expr1], [Expr2] ASC);
GO

-- Creating primary key on [id], [name], [stok], [pcode], [Expr1], [Expr2] in table 'Product_View_1'
ALTER TABLE [dbo].[Product_View_1]
ADD CONSTRAINT [PK_Product_View_1]
    PRIMARY KEY CLUSTERED ([id], [name], [stok], [pcode], [Expr1], [Expr2] ASC);
GO

-- Creating primary key on [id], [value], [qnt], [date], [pcode], [name], [Expr1], [stok], [Expr2] in table 'Sale_View_1'
ALTER TABLE [dbo].[Sale_View_1]
ADD CONSTRAINT [PK_Sale_View_1]
    PRIMARY KEY CLUSTERED ([id], [value], [qnt], [date], [pcode], [name], [Expr1], [stok], [Expr2] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [product_id] in table 'buy'
ALTER TABLE [dbo].[buy]
ADD CONSTRAINT [FK_buy_product]
    FOREIGN KEY ([product_id])
    REFERENCES [dbo].[product]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_buy_product'
CREATE INDEX [IX_FK_buy_product]
ON [dbo].[buy]
    ([product_id]);
GO

-- Creating foreign key on [supplier_id] in table 'buy'
ALTER TABLE [dbo].[buy]
ADD CONSTRAINT [FK_buy_supplier]
    FOREIGN KEY ([supplier_id])
    REFERENCES [dbo].[supplier]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_buy_supplier'
CREATE INDEX [IX_FK_buy_supplier]
ON [dbo].[buy]
    ([supplier_id]);
GO

-- Creating foreign key on [category_id] in table 'product'
ALTER TABLE [dbo].[product]
ADD CONSTRAINT [FK_product_category]
    FOREIGN KEY ([category_id])
    REFERENCES [dbo].[category]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_product_category'
CREATE INDEX [IX_FK_product_category]
ON [dbo].[product]
    ([category_id]);
GO

-- Creating foreign key on [customer_id] in table 'sale'
ALTER TABLE [dbo].[sale]
ADD CONSTRAINT [FK_sale_customer]
    FOREIGN KEY ([customer_id])
    REFERENCES [dbo].[customer]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_sale_customer'
CREATE INDEX [IX_FK_sale_customer]
ON [dbo].[sale]
    ([customer_id]);
GO

-- Creating foreign key on [maker_id] in table 'product'
ALTER TABLE [dbo].[product]
ADD CONSTRAINT [FK_product_maker]
    FOREIGN KEY ([maker_id])
    REFERENCES [dbo].[maker]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_product_maker'
CREATE INDEX [IX_FK_product_maker]
ON [dbo].[product]
    ([maker_id]);
GO

-- Creating foreign key on [product_id] in table 'sale'
ALTER TABLE [dbo].[sale]
ADD CONSTRAINT [FK_sale_product]
    FOREIGN KEY ([product_id])
    REFERENCES [dbo].[product]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_sale_product'
CREATE INDEX [IX_FK_sale_product]
ON [dbo].[sale]
    ([product_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------