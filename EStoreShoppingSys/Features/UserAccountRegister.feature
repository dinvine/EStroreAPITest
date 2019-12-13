Feature: UserAccountRegister
	In order to log into my account in EStore
	As an API visitor
	I want to register a new account

@mytag
Scenario: successful register with valid username and password
	Given visitor has the username and password prepared
	When visit the register API with the username and password
	Then vistor should get the response of success and new account number.

Scenario: fail to register with existing username
	Given visitor has the existing username
	When visit the register API with the existing username and password
	Then vistor should get the response of fail: erro=true;  code=0 .

Scenario: fail to register with empty username or password
	Given visitor has not provide the username or password
	When visit the register API with the empty username or password
	Then vistor should get the response of fail: erro=true;  code=0 .
