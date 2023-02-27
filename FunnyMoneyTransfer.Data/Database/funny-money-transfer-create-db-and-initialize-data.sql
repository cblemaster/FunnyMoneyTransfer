USE master
GO

DECLARE @SQL nvarchar(1000);
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = N'funny_money_transfer')
BEGIN
    SET @SQL = N'USE funny_money_transfer;

                 ALTER DATABASE funny_money_transfer SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                 USE master;

                 DROP DATABASE funny_money_transfer;';
    EXEC (@SQL);
END;

CREATE DATABASE funny_money_transfer
GO

USE funny_money_transfer
GO

CREATE TABLE transfer_types
(
	id					int				IDENTITY(1,1)		NOT NULL,
	[description]	    varchar(10) 						NOT NULL,
 
	CONSTRAINT PK_transfer_types PRIMARY KEY(id),
	CONSTRAINT UC_transfer_types_transfer_type_description UNIQUE(description) -- transfer type descriptions must be unique
)
GO

CREATE TABLE transfer_statuses
(
	id					int				IDENTITY(1,1)		NOT NULL,
	[description]	    varchar(10) 						NOT NULL,
 
	CONSTRAINT PK_transfer_statuses PRIMARY KEY(id),
	CONSTRAINT UC_transfer_statuses_transfer_status_description UNIQUE(description) -- transfer status descriptions must be unique
)
GO

CREATE TABLE users 
(
	id					int				IDENTITY(1,1)		NOT NULL,
	username			varchar(50)							NOT NULL,
	password_hash		varchar(200)						NOT NULL,
	create_date			datetime							NOT NULL,
 
	CONSTRAINT PK_users PRIMARY KEY(id),
	CONSTRAINT UC_users_username UNIQUE(username), -- usernames must be unique	
)
GO

CREATE TABLE accounts
(
	id					int				IDENTITY(1,1)		NOT NULL,
	[user_id]			int									NOT NULL,
	create_date			datetime							NOT NULL,
 
	CONSTRAINT PK_accounts PRIMARY KEY(id),
	CONSTRAINT FK_accounts_users FOREIGN KEY([user_id]) REFERENCES users(id)
		ON UPDATE NO ACTION
		ON DELETE NO ACTION,
	CONSTRAINT UC_accounts_user_id UNIQUE([user_id]), --needed for relationship to users table (one to one)
)
GO

CREATE TABLE transfers
(
	id					int				IDENTITY(1,1)		NOT NULL,
	transfer_type_id	int									NOT NULL,
	transfer_status_id	int									NOT NULL,
	account_id_from		int									NOT NULL,
	account_id_to		int									NOT NULL,
	amount				decimal								NOT NULL,
	create_date			datetime							NOT NULL,
 
	CONSTRAINT PK_transfers PRIMARY KEY(id),
	CONSTRAINT FK_transfers_transfer_types FOREIGN KEY(transfer_type_id) REFERENCES transfer_types(id)
		ON UPDATE NO ACTION
		ON DELETE NO ACTION,
	CONSTRAINT FK_transfers_transfer_statuses FOREIGN KEY(transfer_status_id) REFERENCES transfer_statuses(id)
		ON UPDATE NO ACTION
		ON DELETE NO ACTION,
	CONSTRAINT FK_transfers_accounts_to FOREIGN KEY(account_id_to) REFERENCES accounts(id)
		ON UPDATE NO ACTION
		ON DELETE NO ACTION,
	CONSTRAINT FK_transfers_accounts_from FOREIGN KEY(account_id_from) REFERENCES accounts(id)
		ON UPDATE NO ACTION
		ON DELETE NO ACTION,
	CONSTRAINT CK_transfers_not_same_account CHECK((account_id_from<>account_id_to)), -- transfers must have different 'from' and 'to' accounts
	CONSTRAINT CK_transfers_amount_gt_0 CHECK((amount > 0)) -- transfer amounts must be greater than zero
)
GO

CREATE TABLE starting_balances
(
	id					int				IDENTITY(1,1)		NOT NULL,
	account_id			int									NOT NULL,
	amount				decimal								NOT NULL,
	create_date			datetime							NOT NULL,
	 
	CONSTRAINT PK_starting_balances PRIMARY KEY(id),
	CONSTRAINT UC_account_id UNIQUE(account_id), -- needed for relationship to accounts table (one to one)
	CONSTRAINT FK_starting_balances_accounts FOREIGN KEY(account_id) REFERENCES accounts(id)
		ON UPDATE NO ACTION
		ON DELETE NO ACTION,
)
GO

-- load data for transfer types and transfer statuses
INSERT INTO transfer_statuses ([description]) VALUES ('Pending');
INSERT INTO transfer_statuses ([description]) VALUES ('Approved');
INSERT INTO transfer_statuses ([description]) VALUES ('Rejected');

INSERT INTO transfer_types ([description]) VALUES ('Request');
INSERT INTO transfer_types ([description]) VALUES ('Send');
