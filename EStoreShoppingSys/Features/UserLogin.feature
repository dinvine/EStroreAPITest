Feature: UserLogin
	In order to view the shopping list
	As a user
	I want to get my token by providing my username and password.

@mytag
Scenario: success to obtain token by valid credential
	Given get the existing username and password on 'RegisterEndPoint'
	When visit the token API 'TokenEndPoint' with the  username and password and browserid
	Then TokenAPI  should give  response of 'OK'
	And TokenAPI should give json with 'datas' containing items 'accessToken,tokenType,expiresIn,accountNumber'

Scenario: fail to obtain token by invalid credential
	Given get the unregisted username and password on 'RegisterEndPoint'
	When visit the token API 'TokenEndPoint' with the  username and password and browserid
	Then TokenAPI  should give  response of 'CredentialError'
