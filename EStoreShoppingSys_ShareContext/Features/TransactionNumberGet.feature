Feature: TransactionNumberGet
	In order to make transaction order
	As a user
	I want to get the transaction number
@mytag
Scenario: success  to obtain transaction number with valid credential
	Given Register And Login And Create Cart
	When get transaction number
	Then should get  response of 'OK'
	Then  TransactionNumber should give json with 'datas' containing items 'transactionNumber'

Scenario: fail  to obtain transaction number with invalid credential
	Given Register And Login And Create Cart
	When TransactionNumber get with invalid credential
	Then should get  response of 'TokenError'
