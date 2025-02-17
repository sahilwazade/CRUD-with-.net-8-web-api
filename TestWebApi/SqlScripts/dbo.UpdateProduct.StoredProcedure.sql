USE [TestSahil]
GO
/****** Object:  StoredProcedure [dbo].[UpdateProduct]    Script Date: 1/6/2025 10:18:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateProduct]
	@Id int,
	@Name VARCHAR(50),
	@Price INT,
	@Message VARCHAR(500) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT 1 FROM PRODUCTS WHERE Id = @Id)
	BEGIN 
		UPDATE PRODUCTS SET Name = @Name, Price = @Price WHERE Id = @Id
		SET @Message = 'Product updated.'
	END
	ELSE
	BEGIN
		SET @Message = 'Product with given id is not found.'
	END
END
GO
