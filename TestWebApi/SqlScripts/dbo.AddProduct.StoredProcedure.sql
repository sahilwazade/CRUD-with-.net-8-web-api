USE [TestSahil]
GO
/****** Object:  StoredProcedure [dbo].[AddProduct]    Script Date: 1/6/2025 10:18:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddProduct]
	@Name VARCHAR(50),
	@Price INT,
	@Message VARCHAR(500) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	SET @Name = REPLACE(TRIM(@Name), '  ', ' ')

	IF EXISTS(SELECT 1 FROM PRODUCTS WHERE REPLACE(TRIM(Name), '  ', ' ') LIKE '%' + @Name + '%')
	BEGIN 
		SET @Message = 'Product already exist.'
		RETURN 1;
	END

	INSERT INTO PRODUCTS(Name, Price) VALUES(@Name, @Price)
	SET @Message = 'Product added successfully.'
	RETURN 0;
END
GO
