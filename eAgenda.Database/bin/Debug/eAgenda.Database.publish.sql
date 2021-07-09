﻿/*
Deployment script for master

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "master"
:setvar DefaultFilePrefix "master"
:setvar DefaultDataPath "C:\Users\Arkhandyr\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSqlLocalDB\"
:setvar DefaultLogPath "C:\Users\Arkhandyr\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSqlLocalDB\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Creating Table [dbo].[TBCOMPROMISSO]...';


GO
CREATE TABLE [dbo].[TBCOMPROMISSO] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Local]       VARCHAR (300)  NULL,
    [Data]        DATETIME       NOT NULL,
    [HoraInicio]  BIGINT         NOT NULL,
    [HoraTermino] BIGINT         NOT NULL,
    [Link]        VARCHAR (1000) NULL,
    [Assunto]     VARCHAR (300)  NULL,
    [Id_Contato]  INT            NULL,
    CONSTRAINT [PK__TBCOMPRO__3214EC074FE80D80] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[TBCONTATO]...';


GO
CREATE TABLE [dbo].[TBCONTATO] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Nome]     VARCHAR (100) NOT NULL,
    [Email]    VARCHAR (100) NOT NULL,
    [Telefone] VARCHAR (20)  NOT NULL,
    [Cargo]    VARCHAR (100) NULL,
    [Empresa]  VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[TBTarefa]...';


GO
CREATE TABLE [dbo].[TBTarefa] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Titulo]        VARCHAR (500) NOT NULL,
    [DataCriacao]   DATETIME      NOT NULL,
    [DataConclusao] DATETIME      NULL,
    [Prioridade]    INT           NOT NULL,
    [Percentual]    INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Foreign Key [dbo].[FK_TBCOMPROMISSO_TBCONTATO]...';


GO
ALTER TABLE [dbo].[TBCOMPROMISSO] WITH NOCHECK
    ADD CONSTRAINT [FK_TBCOMPROMISSO_TBCONTATO] FOREIGN KEY ([Id_Contato]) REFERENCES [dbo].[TBCONTATO] ([Id]);


GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[TBCOMPROMISSO] WITH CHECK CHECK CONSTRAINT [FK_TBCOMPROMISSO_TBCONTATO];


GO
PRINT N'Update complete.';


GO
