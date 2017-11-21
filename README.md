# Dynamics365-Customizing-Downloader
Build Status:

![Build Status](https://vss-hueppauff.visualstudio.com/_apis/public/build/definitions/c6b6194e-ccf7-4bfb-b69b-08e22f70ac52/8/badge)

A simple WPF Application which Downloads and Extracts Dynamics365 (properly also Xrm2016 or earlier, but it is not tested) Customizing Solutions.
The whole Application is built around the XRM SDK and the Solution Packager.

See more https://msdn.microsoft.com/en-us/library/jj602987.aspx

#### Usage

##### CRM Connection:
- Open the App and choose "new" in the connection DropBox.

- Add your Connectionstring (See https://msdn.microsoft.com/en-us/library/mt608573.aspx) and press the "Connect" Button

- Rename the Connection if you want to and press "Save"

##### Download Solutions
- Back in the Main Dialog, choose your created Connection

- After a short loading time you should see all your Solutions within the chosen CRM Organization

- Select those you want to Download (must be unmanaged) and press "Download"

- Within the new Dialog, choose a local Folder and click "Download".
#### Possible Dataloss! Every Folder/File within your chosen Folder which will colide with the Solution Names will be deleted. To avoid any issues choose an empty Folder for the extraction.

-> Your Solutions should be Downloaded and Extracted now, any Error will be reported within the Status TextBox.
