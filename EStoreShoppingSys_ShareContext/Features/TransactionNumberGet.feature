Feature: TransactionNumberGet
	In order to make transaction order
	As a user
	I want to get the transaction number
@mytag
Scenario: success  to obtain transaction number with valid credential
	Given Register And Login And Create Cart
	When get transaction number
	Then should get  response of 'OK'
	And should get response comform with model 'TransactionNumber'
	And add  item:  ['datas'] ['transactionNumber'] in response body to scenario context

Scenario: fail  to obtain transaction number with invalid credential
	Given Register And Login And Create Cart
	When TransactionNumber get with invalid credential
	Then should get  response of 'CredentialError'
