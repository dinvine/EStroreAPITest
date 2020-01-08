Feature: UserLogin
	In order to view the shopping list
	As a user
	I want to get my token by providing my username and password.

@mytag
Scenario: success to obtain token by valid credential
	Given register with 'random' username and 'random' password
	When get token with 'valid' credential
	Then should get  response of 'OK'
	And should get response comform with model 'UserLogin'
	And add  item:  ['datas'] ['accessToken,tokenType,expiresIn'] in response body to scenario context
	And should keep value constant for keys 'accountNumber'

Scenario: fail to obtain token by invalid credential
	Given get the unregisted username and password
	When get token with 'invalid' credential
	Then should get  response of 'UsernameError'


	