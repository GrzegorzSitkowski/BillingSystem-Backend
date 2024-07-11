CREATE TABLE [dbo].[Invoices]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Amount] FLOAT NOT NULL,
	[CustomerId] INT NOT NULL,
	[CustomerName] VARCHAR (50) NOT NULL,
	[CreateDate] DATETIMEOFFSET NOT NULL,
	[CreatedBy] INT NOT NULL,
	[DueDate] DATETIMEOFFSET NOT NULL,
	[StatusInvoice] VARCHAR(30) NOT NULL,
	[StatusPayment] VARCHAR(30) NOT NULL,
	CONSTRAINT [PK_Invoices] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Invoices_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id]) ON DELETE CASCADE,
)
