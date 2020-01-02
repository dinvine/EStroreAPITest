Feature: UserAccountDelete
	In order to never use the account anymore
	As user
	I want to   delete the account number


@mytag
Scenario: success to delete account by valid credential
	Given  Register And Login And Create Cart
	When delete  account
	Then AccountDelete  should give  response of 'OK'
	# (X)  assume the response return {datas:{accountnumber:}} , but the real API return the datas:null , so the code below can not be tested.
	# And AccountDelete should give json with 'datas' containing items 'accountNumber'

Scenario: fail to delete account by invalid credential
	Given  Register And Login And Create Cart
	When AccountDelete delete the account number with invalid token
	# (X)  assume the response return fail , but the real API return the success , so the code below can not be tested.
	Then AccountDelete  should give  response of 'TokenError'


Scenario: fail to delete account by invalid account_number
	Given  Register And Login And Create Cart
	When AccountDelete delete the account number with invalid account_number
	Then AccountDelete  should give  response of 'unexpected'


	