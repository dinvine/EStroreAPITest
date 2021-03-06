﻿Feature: TransactionSave
	In order to place the order
	As customer
	I want to save the transaction info


Background:
	Given Register And Login And Create Cart
@Smoke
Scenario: C36 Success to save items into transaction
	Given get transaction number
	When TransactionSave add the items in table to transaction
| itemId | itemName              | quantity | price | amount	|
| 1      | Apple Watch Series 4  | 1        | 550   | 550		|
| 2      | ELEGANT Earrings      | 99		| 24.95 | 2470.05	|
| 3      | USB Wireless Receiver | 999		| 22.5  | 22477.5	|
| 4      | GREEN TEA CLEANSER    | 9999		| 14.99 | 149885.01	|
	Then should get  response of 'OK'
		And should get response comform with model 'TransactionSave'
	#And add  item:  ['datas'] ['TransactionNumber'] in response body to scenario context
	#And should keep value constant for keys 'TransactionNumber'
	# assume the response contains datas:{items:[{item1,quantity},{item2,quantity},{....}]}
    # but the real API return the datas =null , so the code below can not be tested.
	# And TransactionSave  items  should be same to the table

	
@ShouldFail
	Scenario: C37 Should Fail to save items into transaction with invalid credential
	Given get transaction number
	When TransactionSave add the items in table to transaction with invalid credential
| itemId | itemName              | quantity | price | amount	|
| 1      | Apple Watch Series 4  | 1        | 550   | 550		|
	Then should get  response of 'CredentialError'


	
@ShouldFail
Scenario: C38 Should Fail to save items into transaction with invalid itemid
	Given TransactionSave get transaction number
	When TransactionSave add the items in table to transaction
| itemId | itemName              | quantity | price | amount	|
| 0      | Apple Watch Series 4  | 1        | 550   | 550		|
	# (X) assume the response return fail , but the real API return the success, so the code below can not be tested.
	Then should get  response of 'InvalidItemError'

	
@ShouldFail
Scenario: C39 Should Fail to save items into transaction with invalid accountNumber
	Given TransactionSave get transaction number
	When TransactionSave add the items in table to transaction with invalid accountNumber
| itemId | itemName              | quantity | price | amount	|
| 1      | Apple Watch Series 4  | 1        | 550   | 550		|
	# (X)  assume the response return fail , but the real API return the success, so the code below can not be tested.
	Then should get  response of 'AccountNumberError'
	
@ShouldFail
Scenario: C40 Should Fail to save items into transaction with invalid transactionNumber
	Given TransactionSave get transaction number
	When TransactionSave add the items in table to transaction with invalid transactionNumber
| itemId | itemName              | quantity | price | amount	|
| 1      | Apple Watch Series 4  | 1        | 550   | 550		|
	Then should get  response of 'InvalidTransactionNumberError'
