USE [TestSahil]
GO
/****** Object:  StoredProcedure [dbo].[DeleteProduct]    Script Date: 1/6/2025 10:18:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteProduct]
	@Id int,
	@Message VARCHAR(500) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT 1 FROM PRODUCTS WHERE Id = @Id)
		BEGIN 
			DELETE FROM PRODUCTS WHERE Id = @Id
			SET @Message = 'Product Deleted.'
		END
	ELSE
		BEGIN
			SET @Message = 'Product with given id is not found.'
		END
END
GO
