
CREATE VIEW [dbo].[vwQATWorkFlow]
AS
SELECT        tW.Active, tW.Facility, tW.PackagingLine, tW.QATDefnID, tW.SerialConnID, tW.TestSeq, tW.TCPConnID, tw.ExceptionCode
				,tD.Alert, tD.AllowOverride, tD.NoOfLanes, tD.NoOfSamples, tD.NoteID ,tD.QATDefnDescription, tD.QATEntryPoint 
				,tD.TestFormID, ISNULL(tD.TestFormTitle, '') AS TestFormTitle, tD.InProcFreqType, tD.InProcNoOfFreq, tD.TestSpecID
				,tF.FormName, tF.TableName, tF.InterfaceFormID, tF.TestCategory, tN.Note
FROM          dbo.tblQATWorkFlow AS tW LEFT OUTER JOIN
                dbo.tblQATDefinition AS tD ON tW.Facility = tD.Facility AND tW.QATDefnID = tD.QATDefnID LEFT OUTER JOIN
                dbo.tblQATForm AS tF ON tD.Facility = tF.Facility AND tD.TestFormID = tF.TestFormID LEFT OUTER JOIN
                dbo.tblQATNote AS tN ON tW.Facility = tN.Facility AND tD.NoteID = tN.NoteID

GO

