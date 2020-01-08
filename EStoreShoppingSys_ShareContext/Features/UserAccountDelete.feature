Feature: UserAccountDelete
	In order to never use the account anymore
	As user
	I want to   delete the account number


@mytag
Scenario: success to delete account by valid credential
	Given  Register And Login And Create Cart
	When delete  account
	Then should get  response of 'OK'
	And should get response comform with model 'UserAccountDelete'
	# (X)  assume the response return {datas:{accountnumber:}} , but the real API return the datas:null , so the code below can not be tested.
	# And AccountDelete should give json with 'datas' containing items 'accountNumber'

Scenario: fail to delete account by invalid credential
	Given  Register And Login And Create Cart
	When AccountDelete delete the account number with invalid token
	# (X)  assume the response return fail , but the real API return the success , so the code below can not be tested.
	Then should get  response of 'CredentialError'


Scenario: fail to delete account by invalid account_number
	Given  Register And Login And Create Cart
	When AccountDelete delete the account number with invalid account_number
	Then should get  response of 'unexpected'


	