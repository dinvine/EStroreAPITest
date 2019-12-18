Feature: UserAccountRegister
	In order to log into my account in EStore
	As an API visitor
	I want to register a new account

@mytag
Scenario: successful register with valid username and password
	Given generate the "random" username and "random" password
	When visit the register API "/authentication/register" with the username and password
	Then should get  response  status of OK
	And get response body with "code"   equal to "200"
	And with new account number in datas
	And with item named "message"  contains "success"

Scenario: fail to register with existing username
	Given generate the "existing" username and "random" password
	When visit the register API "/authentication/register" with the username and password
	Then should get  response  status of OK
	And get response body with "code"   equal to "0"
	And get response body with "error"   equal to "True"
	And with item named "message"  contains "registered"

Scenario: fail to register with empty username
	Given generate the "empty" username and "random" password
	When visit the register API "/authentication/register" with the username and password
	Then should get  response  status of OK
	And get response body with "code"   equal to "0"
	And get response body with "error"   equal to "True"
	And with item named "message"  contains "error"

Scenario: fail to register with empty password
	Given generate the "random" username and "empty" password
	When visit the register API "/authentication/register" with the username and password
	Then should get  response  status of OK
	And get response body with "code"   equal to "0"
	And get response body with "error"   equal to "True"
	And with item named "message"  contains "error"