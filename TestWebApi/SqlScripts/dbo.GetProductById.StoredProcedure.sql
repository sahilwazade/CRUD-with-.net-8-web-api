USE [TestSahil]
GO
/****** Object:  StoredProcedure [dbo].[GetProductById]    Script Date: 1/6/2025 10:18:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetProductById]
	@id INT,
	@Message VARCHAR(500) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT 1 FROM PRODUCTS WHERE Id = @id)
		BEGIN
			SET @Message = 'Product retrieved successfully.'
			SELECT Id, Name, Price FROM PRODUCTS WHERE Id = @id
		END
	ELSE
		BEGIN
			SET @Message = 'Product not found based on Id'
		END
END
GO
