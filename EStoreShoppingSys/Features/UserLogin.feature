Feature: UserLogin
	In order to view the shopping list
	As a user
	I want to get my token by providing my username and password.

@mytag
Scenario: success to obtain token by valid credential
	Given UserLogin generate the "random" username and "random" password
	When UserLogin visit the register API "/authentication/register" with the username and password
	And visit the token API "/authentication/token" with the  username and password and browserid
	Then TokenAPI should return response  status of "OK"
	And TokenAPI should return json with "code" equal to "200"
	And TokenAPI should return json with "datas" containing items 'accessToken,tokenType,expiresIn,accountNumber'
	And And TokenAPI should return json with "message" containing substring "success"


Scenario: fail to obtain token by invalid credential
	Given UserLogin generate the "unexisting" username and "random" password
	When visit the token API "/authentication/token" with the  username and password and browserid
	Then TokenAPI should return response  status of "OK"
	And TokenAPI should return json with "code" equal to "0"
	And TokenAPI should return json with "error" equal to "True"
	And And TokenAPI should return json with "message" containing substring "error"	