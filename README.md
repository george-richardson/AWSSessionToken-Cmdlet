# Build

Open in visual studio and restore Nuget packages and build. 

# Install

Open the output folder in powershell: 

```powershell
Import-Module ./AWSSessionToken.psd1
```

The cmdlet looks for access keys at environment variable `DEVOPS_AWS_ACCESS_KEY_ID` first and then `AWS_ACCESS_KEY_ID`.
The cmdlet looks for secret keys at environment variable `DEVOPS_AWS_SECRET_ACCESS_KEY` first and then `AWS_SECRET_ACCESS_KEY`.
The cmdlet looks for MFA device serial or ARN at `DEVOPS_AWS_MFA_DEVICE`. 
    
# Usage

Get session token using just your MFA token code.

```powershell
Get-AWSSessionToken 123456
```

Get session token with MFA token code and a duration of a minute (in seconds).

```powershell
Get-AWSSessionToken 123456 60
```

# Known Issue

This cmdlet does not currently work alongside the official AWS tools for powershell.