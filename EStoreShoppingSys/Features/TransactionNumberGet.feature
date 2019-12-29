Feature: TransactionNumberGet
	In order to make transaction order
	As a user
	I want to get the transaction number
@mytag
Scenario: success  to obtain transaction number with valid credential
	Given TransactionNumber Register And Login And CreateCart
	When TransactionNumber visit the transaction number API with valid credential
	Then TransactionNumber should give  response of 'OK'
	Then  TransactionNumber should give json with 'datas' containing items 'transactionNumber'

Scenario: fail  to obtain transaction number with invalid credential
	Given TransactionNumber Register And Login And CreateCart
	When TransactionNumber visit the cart info API with invalid credential
	Then TransactionNumber should give  response of 'TokenError'
