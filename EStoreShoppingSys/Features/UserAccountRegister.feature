Feature: UserAccountRegister
	In order to log into my account in EStore
	As an API visitor
	I want to register a new account

@mytag
Scenario: successful register with valid username and password
	When register on 'RegisterEndPoint' with 'random' username and 'random' password
	Then register should get  response of 'OK'
	And register should get  ['datas'] including 'accountNumber'

Scenario: fail to register with existing username
	When register on 'RegisterEndPoint' with 'existing' username and 'random' password
	Then register should get  response of 'RegisteredError'

Scenario: fail to register with empty username
	When register on 'RegisterEndPoint' with 'empty' username and 'random' password
	Then register should get  response of 'UsernameError'

Scenario: fail to register with empty password
	When register on 'RegisterEndPoint' with 'random' username and 'empty' password
	Then register should get  response of 'PasswordError'