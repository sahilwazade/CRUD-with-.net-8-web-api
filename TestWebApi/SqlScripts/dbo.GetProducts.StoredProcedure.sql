USE [TestSahil]
GO
/****** Object:  StoredProcedure [dbo].[GetProducts]    Script Date: 1/6/2025 10:18:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetProducts]
	 @Message NVARCHAR(500) OUTPUT
as
BEGIN
	IF EXISTS (SELECT 1 FROM Products)
	BEGIN
        SET @Message = 'Products retrieved successfully.'
		SELECT Id, Name, Price FROM Products 
    END
	ELSE
    BEGIN
        SET @Message = 'No products found.'
    END
END
GO
