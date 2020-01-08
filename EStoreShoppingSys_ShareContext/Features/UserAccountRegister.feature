Feature: UserAccountRegister
	In order to log into my account in EStore
	As an API visitor
	I want to register a new account

@mytag
Scenario: successful register with valid username and password
	When register with 'random' username and 'random' password
	Then should get  response of 'OK'
	And should get response comform with model 'UserAccount'
	And add  item:  ['datas'] ['accountNumber'] in response body to scenario context

Scenario: fail to register with existing username
	When register with 'existing' username and 'random' password
	Then should get  response of 'RegisteredError'

Scenario: fail to register with empty username
	When register with 'empty' username and 'random' password
	Then should get  response of 'UsernameError'

Scenario: fail to register with empty password
	When register with 'random' username and 'empty' password
	Then should get  response of 'PasswordError'