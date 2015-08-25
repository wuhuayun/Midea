function setMainUploadStatus(returnCode, htmlSelector)
{
	if (!htmlSelector)
	{
		htmlSelector = "#uploadStatusCn";
	}

	if (returnCode && returnCode != "0")
	{
		$(htmlSelector).addClass("input-validation-error");
	}
	else
	{
		$(htmlSelector).removeClass("input-validation-error");
	}
}

function setRowStyle_ReturnCode(index, rowData)
{
	if (rowData.returnCode && rowData.returnCode != "0")
	{
		return 'background-color:#ffeeee; color:#ff0000;';
	}
}