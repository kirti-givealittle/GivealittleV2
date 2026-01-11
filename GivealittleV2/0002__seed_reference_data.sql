USE [Gen2]
GO
INSERT [dbo].[EmailTemplates] ([TemplateId], [TemplateKey], [TemplateType], [SubjectTemplate], [BodyTemplate], [IsHtml], [IsActive], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (N'ac9190c0-51eb-f011-9bee-f46d3fb7dc74', N'OTP_VERIFICATION', N'OTP', N'Your One-Time Password (OTP) for {{OtpPurpose}}', N'<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Account Verification</title>
</head>

<body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333; background-color: #f7f9fc; margin: 0; padding: 0;">
    <div style="max-width: 600px; margin: 40px auto; background: #ffffff; padding: 30px; border-radius: 6px; text-align: center; box-shadow: 0 0 5px rgba(0,0,0,0.05);">
        
        <p>Hi {{ToFirstName}},</p>

        <p>
            We received a request for <strong>{{OtpPurpose}}</strong>.
            Please use the following One-Time Password (Otp) to continue:
        </p>

        <p style="font-size: 24px; font-weight: bold; letter-spacing: 3px;">
            {{Otp}}
        </p>

        <p>
            This OTP is valid for <strong>{{OtpExpiryInMinutes}} minutes</strong>.
            Please do not share this code with anyone.
        </p>

        <p>
            If you did not request this, you can safely ignore this email.
        </p>

        <p>
            Thanks,<br/>
            <strong>Security Team</strong>
        </p>

        <hr/>

        <p style="font-size: 12px; color: #777;">
            Need help? Contact us at <a href="mailto:info@givealittle.co.nz" style="color: #1a73e8; text-decoration: none;">info@givealittle.co.nz</a>
        </p>

    </div>
</body>
</html>', 1, 1, N'OTP email used for authentication flows such as registration, login, and password reset.', CAST(N'2026-01-06T22:48:01.0000000' AS DateTime2), CAST(N'2026-01-08T04:21:37.0000000' AS DateTime2))
INSERT [dbo].[EmailTemplates] ([TemplateId], [TemplateKey], [TemplateType], [SubjectTemplate], [BodyTemplate], [IsHtml], [IsActive], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (N'b3babe0e-72eb-f011-9bee-f46d3fb7dc74', N'OTP_ACCOUNT_REGISTRATION_WITH_VERIFICATION_LINK', N'OTP', N'You''re almost there !', N'<!DOCTYPE html>
    <html>
    <head>
        <meta charset="UTF-8">
        <title>Account Verification</title>
    </head>
    <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333;">
        <p>Hi {{ToFirstName}},</p>

        <p>
            We received a request for <strong>{{OtpPurpose}}</strong>.
            Please click on the following link to verify your account with us.
        </p>

        <p style="font-size: 24px; font-weight: bold; letter-spacing: 3px;">
            https://localhost:44375/api/otp/registration/verify/{{OtpPurpose}}/{{UserEmail}}/{{Otp}}
        </p>

        <p>
            This link is valid for <strong>{{OtpExpiryInMinutes}} minutes</strong>.
            Please do not share this link with anyone.
        </p>

        <p>
            If you did not request this, please let us know at {{SupportEmail}}.
        </p>

        <p>
            Thanks,<br/>
            <strong>Security Team</strong>
        </p>

        <hr/>

        <p style="font-size: 12px; color: #777;">
            Need help? Contact us at {{SupportEmail}}
        </p>
    </body>
    </html>', 1, 1, N'Link sent to every users who are creating a cause page. Once clicked, it will verify the account with us.', CAST(N'2026-01-07T02:39:16.0000000' AS DateTime2), CAST(N'2026-01-08T03:36:59.0000000' AS DateTime2))
INSERT [dbo].[EmailTemplates] ([TemplateId], [TemplateKey], [TemplateType], [SubjectTemplate], [BodyTemplate], [IsHtml], [IsActive], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (N'dc78e2f5-2aec-f011-9bef-f46d3fb7dc74', N'OTP_VERIFICATION_FAILED', N'OTP', N'Your Account verification failed.', N'<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Account Verification</title>
</head>

<body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333; background-color: #f7f9fc; margin: 0; padding: 0;">
    <div style="max-width: 600px; margin: 40px auto; background: #ffffff; padding: 30px; border-radius: 6px; text-align: center; box-shadow: 0 0 5px rgba(0,0,0,0.05);">
        
        <p style="font-size: 18px; margin-bottom: 10px;">Hi,</p>
<p style="font-size: 20px; font-weight: bold; color: #d32f2f; margin: 20px 0;"> Account Verification Failed.</p>
<p style="font-size: 16px; margin: 15px 0;"> We were unable to verify your account at this time. </p>
<p style="font-size: 16px; margin: 15px 0;"> This can happen if: </p>
<ul style="text-align: left; display: inline-block; font-size: 15px; color: #444; margin: 10px 0 20px;">
	<li>The verification link has expired</li>
	<li>The verification link was entered incorrectly</li>
	<li>The verification link has already been used</li>
</ul>
<p style="font-size: 16px; margin: 15px 0;"> Please request a new link and try again. </p>
<p style="font-size: 16px; margin: 15px 0;"> If you did not attempt this action, you can safely ignore this message. </p>

        <p style="font-size: 12px; color: #777;">
            Need help? Contact us at <a href="mailto:info@givealittle.co.nz" style="color: #1a73e8; text-decoration: none;">info@givealittle.co.nz</a>
        </p>

    </div>
</body>
</html>', 1, 1, N'Template used to inform OTP verification was not succesfull', CAST(N'2026-01-08T00:42:52.0000000' AS DateTime2), CAST(N'2026-01-08T03:52:05.0000000' AS DateTime2))
INSERT [dbo].[EmailTemplates] ([TemplateId], [TemplateKey], [TemplateType], [SubjectTemplate], [BodyTemplate], [IsHtml], [IsActive], [Description], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (N'e9bd2527-2bec-f011-9bef-f46d3fb7dc74', N'OTP_VERIFIED', N'OTP', N'Your account Verified', N'<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Account Verification</title>
</head>

<body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333; background-color: #f7f9fc; margin: 0; padding: 0;">
    <div style="max-width: 600px; margin: 40px auto; background: #ffffff; padding: 30px; border-radius: 6px; text-align: center; box-shadow: 0 0 5px rgba(0,0,0,0.05);">
        
        <p style="font-size: 18px; margin-bottom: 10px;">Hi,</p>

        <p style="font-size: 20px; font-weight: bold; color: #2e7d32; margin: 20px 0;">
            Verification Successful.
        </p>

        <p style="font-size: 16px; margin: 15px 0;">
            Your account has been successfully verified.
        </p>

        <p style="font-size: 16px; margin: 15px 0;">
            You can safely close this window or continue using the application.
        </p>

        <hr style="border: none; border-top: 1px solid #eee; margin: 25px 0;">

        <p style="font-size: 12px; color: #777;">
            Need help? Contact us at <a href="mailto:info@givealittle.co.nz" style="color: #1a73e8; text-decoration: none;">info@givealittle.co.nz</a>
        </p>

    </div>
</body>
</html>', 1, 1, N'Template used to inform OTP verification was not succesfull', CAST(N'2026-01-08T00:44:14.0000000' AS DateTime2), CAST(N'2026-01-08T03:47:38.0000000' AS DateTime2))
GO
INSERT [dbo].[GlobalConfig] ([Id], [Key], [Value], [KeyGroup], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (N'6e0dccd2-5feb-f011-9bee-f46d3fb7dc74', N'SYSTEM_EMAIL', N'gvaltltest@gmail.com', N'Email', CAST(N'2026-01-07T00:28:44.9493998' AS DateTime2), CAST(N'2026-01-07T00:44:09.6181700' AS DateTime2))
INSERT [dbo].[GlobalConfig] ([Id], [Key], [Value], [KeyGroup], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (N'3c8fefc7-61eb-f011-9bee-f46d3fb7dc74', N'SYSTEM_EMAIL_CONFIG_KEY', N'A73B5411-5FEB-F011-9BEE-F46D3FB7DC74', N'Email', CAST(N'2026-01-07T00:42:45.7206204' AS DateTime2), CAST(N'2026-01-07T02:11:16.8137563' AS DateTime2))
INSERT [dbo].[GlobalConfig] ([Id], [Key], [Value], [KeyGroup], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (N'b2617d19-64eb-f011-9bee-f46d3fb7dc74', N'SYSTEM_EMAIL_PROVIDER', N'GMAIL', N'Email', CAST(N'2026-01-07T00:59:21.5389746' AS DateTime2), CAST(N'2026-01-07T00:59:21.5389746' AS DateTime2))
GO
INSERT [dbo].[GmailOAuthConfigs] ([GmailConfigId], [ClientId], [ClientSecret], [RefreshToken], [AccessToken], [AccessTokenExpiresAtUtc], [CreatedAtUtc], [UpdatedAtUtc]) VALUES (N'a73b5411-5feb-f011-9bee-f46d3fb7dc74', N'815169081856-v787apd5h488cjr6u0mf9016s82a15ag.apps.googleusercontent.com', N'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', N'1//04J0VGo80vPxtCgYIARAAGAQSNwF-L9IrdE-VdMXdkygioJ8ifaDc0kxQhAUwCeyvWeliRFa9lE-Lm42oaXIhaXFGKcvogOGIjXA', N'ya29.a0Aa7pCA_ZqZui7_g6epvZIrrABu-0HSYFEP7YLVyA34NHa-7KlgVOEnvJe4MkCC49RJTqRzoRAKDFV4XtixdgAKlxBIagpkFUBNhTxoYn1lXTGU1_vbeblSK0jUvRj-tKve2AS4MwsWtH_sz3Ag9-DtwWu5axCbkgs6j5TaTVXrTxHHOZq-_BhdCmtDjQj68O7SEaLhEaCgYKAZ0SARUSFQHGX2MiNarYfLfjs57FjyWWzaallA0206', NULL, CAST(N'2026-01-07T00:23:20.0000000' AS DateTime2), CAST(N'2026-01-07T00:23:20.0000000' AS DateTime2))
GO
